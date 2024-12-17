using Eventer.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Configurations
{
    public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
    {
        public void Configure(EntityTypeBuilder<EventCategory> builder)
        {
            builder.ToTable("EventCategories");

            builder.HasKey(ec => ec.Id);

            builder.Property(ec => ec.Name)
                .IsRequired()
                .HasMaxLength(40);

            builder.Property(ec => ec.Description)
                .HasMaxLength(500);
        }
    }
}
