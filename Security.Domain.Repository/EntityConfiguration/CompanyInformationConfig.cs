using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Security.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Security.Domain.Repository.EntityConfiguration
{
    public class CompanyInformationConfig: IEntityTypeConfiguration<CompanyInformation>
    {
        public void Configure(EntityTypeBuilder<CompanyInformation> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.User).WithOne(b => b.CompanyInformation).HasForeignKey<UserRegister>(b => b.Id);
        }
    }
}
