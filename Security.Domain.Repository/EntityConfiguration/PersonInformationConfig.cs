using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Models.EntityConfiguration
{
    public class PersonInformationConfig : IEntityTypeConfiguration<PersonalInformation>
    {
        public void Configure(EntityTypeBuilder<PersonalInformation> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a=>a.User).WithOne(b=>b.PersonalInformation).HasForeignKey<UserRegister>(b => b.Id);
        }
    }
}
