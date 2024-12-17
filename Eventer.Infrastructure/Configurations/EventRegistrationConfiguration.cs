using Eventer.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Eventer.Infrastructure.Configurations
{
    public class EventRegistrationConfiguration : IEntityTypeConfiguration<EventRegistration>
    {
        public void Configure(EntityTypeBuilder<EventRegistration> builder)
        {
            builder.ToTable("EventRegistrations");

            builder.HasKey(er => er.Id);

            builder.Property(er => er.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(er => er.Surname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(er => er.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(er => er.DateOfBirth)
                .IsRequired();

            builder.Property(er => er.RegistrationDate)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany(u => u.EventRegistrations)
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
