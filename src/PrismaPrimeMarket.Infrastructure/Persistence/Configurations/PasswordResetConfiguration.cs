using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismaPrimeMarket.Domain.Entities;

namespace PrismaPrimeMarket.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuração do Entity Framework Core para a entidade PasswordReset
/// </summary>
public class PasswordResetConfiguration : IEntityTypeConfiguration<PasswordReset>
{
    public void Configure(EntityTypeBuilder<PasswordReset> builder)
    {
        // Tabela
        builder.ToTable("PasswordResets");

        // Chave primária
        builder.HasKey(pr => pr.Id);

        // Propriedades
        builder.Property(pr => pr.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pr => pr.UserId)
            .IsRequired();

        builder.Property(pr => pr.ExpiresAt)
            .IsRequired();

        builder.Property(pr => pr.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(pr => pr.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pr => pr.UsedAt)
            .IsRequired(false);

        // Relacionamentos
        builder.HasOne(pr => pr.User)
            .WithMany()
            .HasForeignKey(pr => pr.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(pr => pr.Token)
            .IsUnique()
            .HasDatabaseName("IX_PasswordResets_Token");

        builder.HasIndex(pr => pr.UserId)
            .HasDatabaseName("IX_PasswordResets_UserId");

        builder.HasIndex(pr => pr.ExpiresAt)
            .HasDatabaseName("IX_PasswordResets_ExpiresAt");

        builder.HasIndex(pr => pr.IsUsed)
            .HasDatabaseName("IX_PasswordResets_IsUsed");

        // Ignora propriedades que não devem ser mapeadas
        builder.Ignore(pr => pr.DomainEvents);
        builder.Ignore(pr => pr.IsValid);
        builder.Ignore(pr => pr.IsExpired);
    }
}
