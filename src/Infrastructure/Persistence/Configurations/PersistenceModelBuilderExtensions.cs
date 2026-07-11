using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Solution.Infrastructure.Persistence.Configurations;

internal static class PersistenceModelBuilderExtensions
{
    public static PropertyBuilder<long> AsRowVersion(this PropertyBuilder<long> propertyBuilder) =>
        propertyBuilder
            .HasColumnName("row_version")
            .HasDefaultValue(1L)
            .IsConcurrencyToken();
}
