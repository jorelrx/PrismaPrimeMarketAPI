using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrismaPrimeMarket.Domain.Entities;

namespace PrismaPrimeMarket.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuração do Entity Framework Core para a entidade RefreshToken
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Tabela
        builder.ToTable("RefreshTokens");

        // Chave primária
        builder.HasKey(rt => rt.Id);

        // Propriedades
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(rt => rt.UserId)
            .IsRequired();

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(rt => rt.RevokedAt)
            .IsRequired(false);

        builder.Property(rt => rt.ReplacedByToken)
            .IsRequired(false)
            .HasMaxLength(500);

        // Relacionamentos
        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índices
        builder.HasIndex(rt => rt.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshTokens_Token");

        builder.HasIndex(rt => rt.UserId)
            .HasDatabaseName("IX_RefreshTokens_UserId");

        builder.HasIndex(rt => rt.ExpiresAt)
            .HasDatabaseName("IX_RefreshTokens_ExpiresAt");

        builder.HasIndex(rt => rt.IsRevoked)
            .HasDatabaseName("IX_RefreshTokens_IsRevoked");

        // Ignora propriedades que não devem ser mapeadas
        builder.Ignore(rt => rt.DomainEvents);
        builder.Ignore(rt => rt.IsActive);
        builder.Ignore(rt => rt.IsExpired);
    }
}
