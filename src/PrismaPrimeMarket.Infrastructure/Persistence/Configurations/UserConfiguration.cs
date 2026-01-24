using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismaPrimeMarket.Domain.Entities;

namespace PrismaPrimeMarket.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuração do Entity Framework Core para a entidade User
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Tabela
        builder.ToTable("Users");

        // Chave primária (herdada do IdentityUser)
        builder.HasKey(u => u.Id);

        // Propriedades obrigatórias
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired(false)
            .HasMaxLength(254);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(254);

        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(50);

        // Propriedades opcionais
        builder.Property(u => u.ProfilePictureUrl)
            .HasMaxLength(500);

        // Value Objects como Owned Entities
        builder.OwnsOne(u => u.CPF, cpf =>
        {
            cpf.Property(c => c.Number)
                .HasColumnName("CPF")
                .HasMaxLength(11);
        });

        builder.OwnsOne(u => u.Phone, phone =>
        {
            phone.Property(p => p.Number)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(15);

            phone.Property(p => p.CountryCode)
                .HasColumnName("PhoneCountryCode")
                .HasMaxLength(5)
                .HasDefaultValue("+55");
        });

        // Timestamps
        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(u => u.UpdatedAt)
            .IsRequired(false);

        builder.Property(u => u.LastLoginAt)
            .IsRequired(false);

        builder.Property(u => u.DeletedAt)
            .IsRequired(false);

        // Status flags
        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Índices
        builder.HasIndex(u => u.Email)
            .HasDatabaseName("IX_Users_Email");

        builder.HasIndex(u => u.UserName)
            .IsUnique()
            .HasDatabaseName("IX_Users_UserName");

        builder.HasIndex(u => u.NormalizedEmail)
            .HasDatabaseName("IX_Users_NormalizedEmail");

        builder.HasIndex(u => u.NormalizedUserName)
            .HasDatabaseName("IX_Users_NormalizedUserName");

        builder.HasIndex(u => u.IsDeleted)
            .HasDatabaseName("IX_Users_IsDeleted");

        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_Users_IsActive");

        // Ignora propriedades que não devem ser mapeadas
        builder.Ignore(u => u.DomainEvents);

        // Query Filter global para soft delete
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
