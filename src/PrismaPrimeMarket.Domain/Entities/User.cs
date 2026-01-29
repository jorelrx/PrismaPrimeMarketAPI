using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Events;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.ValueObjects;

namespace PrismaPrimeMarket.Domain.Entities;

/// <summary>
/// Entidade que representa um usuário do sistema.
/// Herda de IdentityUser para integração com ASP.NET Core Identity.
/// Implementa IBaseEntity e IAggregateRoot para compatibilidade com repositórios base.
/// </summary>
public class User : IdentityUser<Guid>, IBaseEntity, IAggregateRoot
{
    // Propriedades de domínio adicionais
    public string FirstName { get; private set; }
    public string? LastName { get; private set; }
    public CPF? CPF { get; private set; }
    public PhoneNumber? Phone { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? ProfilePictureUrl { get; private set; }

    // Controle de timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    // Controle de status
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Domain Events
    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    // Construtor privado para EF Core
    private User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    // Construtor privado usado pelo factory method
    private User(
        string userName,
        string firstName,
        string? email = null,
        string? lastName = null,
        CPF? cpf = null,
        PhoneNumber? phone = null,
        DateTime? birthDate = null)
    {
        Id = Guid.NewGuid();
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName ?? string.Empty;
        CPF = cpf;
        Phone = phone;
        BirthDate = birthDate;
        CreatedAt = DateTime.UtcNow;
        IsActive = true; // Ativo por padrão quando não requer confirmação de e-mail
        IsDeleted = false;
        EmailConfirmed = string.IsNullOrEmpty(email); // Confirmado se não houver e-mail
        PhoneNumberConfirmed = false;
        TwoFactorEnabled = false;
        LockoutEnabled = true;
    }

    /// <summary>
    /// Factory method para criar um novo usuário
    /// </summary>
    public static User Create(
        string userName,
        string firstName,
        string? email = null,
        string? lastName = null,
        CPF? cpf = null,
        PhoneNumber? phone = null,
        DateTime? birthDate = null)
    {
        // Validações de domínio
        ValidateUserName(userName);
        ValidateName(firstName, nameof(firstName));

        if (!string.IsNullOrEmpty(email))
            ValidateEmail(email);

        if (!string.IsNullOrEmpty(lastName))
            ValidateName(lastName, nameof(lastName));

        ValidateBirthDate(birthDate);

        var user = new User(userName, firstName, email, lastName, cpf, phone, birthDate);

        // Adiciona evento de domínio
        user.AddDomainEvent(new UserRegisteredEvent(user.Id, user.Email ?? string.Empty, user.UserName!));

        return user;
    }

    /// <summary>
    /// Atualiza as informações do perfil do usuário
    /// </summary>
    public void UpdateProfile(
        string firstName,
        string? lastName = null,
        CPF? cpf = null,
        PhoneNumber? phone = null,
        DateTime? birthDate = null)
    {
        ValidateName(firstName, nameof(firstName));
        if (!string.IsNullOrWhiteSpace(lastName))
            ValidateName(lastName, nameof(lastName));
        ValidateBirthDate(birthDate);

        FirstName = firstName;
        LastName = lastName;
        CPF = cpf;
        Phone = phone;
        BirthDate = birthDate;
        UpdateTimestamp();
    }

    /// <summary>
    /// Ativa a conta do usuário após confirmação de e-mail
    /// </summary>
    public void Activate()
    {
        if (IsActive)
            throw new DomainException("O usuário já está ativo");

        if (IsDeleted)
            throw new DomainException("Não é possível ativar um usuário excluído");

        IsActive = true;
        UpdateTimestamp();

        AddDomainEvent(new UserActivatedEvent(Id, Email!));
    }

    /// <summary>
    /// Desativa a conta do usuário
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("O usuário já está inativo");

        IsActive = false;
        UpdateTimestamp();
    }

    /// <summary>
    /// Marca o usuário como excluído (soft delete)
    /// </summary>
    public void Delete()
    {
        if (IsDeleted)
            throw new DomainException("O usuário já está excluído");

        IsDeleted = true;
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    /// <summary>
    /// Restaura um usuário excluído
    /// </summary>
    public void Restore()
    {
        if (!IsDeleted)
            throw new DomainException("O usuário não está excluído");

        IsDeleted = false;
        DeletedAt = null;
        UpdateTimestamp();
    }

    /// <summary>
    /// Atualiza a foto de perfil
    /// </summary>
    public void UpdateProfilePicture(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
            throw new DomainException("A URL da foto de perfil não pode ser vazia");

        ProfilePictureUrl = pictureUrl;
        UpdateTimestamp();
    }

    /// <summary>
    /// Registra o último login do usuário
    /// </summary>
    public void RegisterLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        UpdateTimestamp();
    }

    /// <summary>
    /// Retorna o nome completo do usuário
    /// </summary>
    public string GetFullName() => $"{FirstName} {LastName}";

    // Implementação de IBaseEntity
    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    // Métodos auxiliares privados

    private static void ValidateEmail(string email)
    {
        // A validação completa será feita pelo Value Object Email quando usado
        if (email.Length > 254)
            throw new DomainException("O e-mail não pode ter mais de 254 caracteres");
    }

    private static void ValidateUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException("O nome de usuário não pode ser vazio");

        if (userName.Length < 3)
            throw new DomainException("O nome de usuário deve ter no mínimo 3 caracteres");

        if (userName.Length > 50)
            throw new DomainException("O nome de usuário não pode ter mais de 50 caracteres");
    }

    private static void ValidateName(string name, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException($"O {fieldName} não pode ser vazio");

        if (name.Length < 2)
            throw new DomainException($"O {fieldName} deve ter no mínimo 2 caracteres");

        if (name.Length > 100)
            throw new DomainException($"O {fieldName} não pode ter mais de 100 caracteres");
    }

    private static void ValidateBirthDate(DateTime? birthDate)
    {
        if (!birthDate.HasValue)
            return;

        if (birthDate.Value > DateTime.UtcNow)
            throw new DomainException("A data de nascimento não pode ser futura");

        var age = DateTime.UtcNow.Year - birthDate.Value.Year;
        if (age < 13)
            throw new DomainException("O usuário deve ter no mínimo 13 anos");

        if (age > 150)
            throw new DomainException("A data de nascimento é inválida");
    }

    // Domain Events Management
    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
