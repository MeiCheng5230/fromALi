using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Common.Mvc;

namespace PXin.Model.Mapping
{
    public class TnetNodeinfoMap : EntityTypeConfiguration<TnetNodeinfo>
    {
        public TnetNodeinfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Nodeid);
            // Properties
            this.Property(t => t.Nodeid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Idcardno)
                .IsOptional()
                .HasMaxLength(50);
            this.Property(t => t.Birthday)
                    .IsOptional();
            this.Property(t => t.Email)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Idcardaddr)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Contactname)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Contacttel)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Company)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Comregionid)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Companyaddr)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Famregionid)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Familyaddr)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Othregionid)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Otheraddr)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Defaultaddr)
                    .IsOptional();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Name)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Sex)
                    .IsOptional();
            this.Property(t => t.Nation)
                    .IsOptional()
                    .HasMaxLength(200);
            this.Property(t => t.Beginvalidity)
                    .IsOptional();
            this.Property(t => t.Endvalidity)
                    .IsOptional();
            this.Property(t => t.Issuing)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TNET_NODEINFO", DbContextHelper.GetOwnerByTableName("TNET_NODEINFO"));
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Idcardno).HasColumnName("IDCARDNO");
            this.Property(t => t.Birthday).HasColumnName("BIRTHDAY");
            this.Property(t => t.Email).HasColumnName("EMAIL");
            this.Property(t => t.Idcardaddr).HasColumnName("IDCARDADDR");
            this.Property(t => t.Contactname).HasColumnName("CONTACTNAME");
            this.Property(t => t.Contacttel).HasColumnName("CONTACTTEL");
            this.Property(t => t.Company).HasColumnName("COMPANY");
            this.Property(t => t.Comregionid).HasColumnName("COMREGIONID");
            this.Property(t => t.Companyaddr).HasColumnName("COMPANYADDR");
            this.Property(t => t.Famregionid).HasColumnName("FAMREGIONID");
            this.Property(t => t.Familyaddr).HasColumnName("FAMILYADDR");
            this.Property(t => t.Othregionid).HasColumnName("OTHREGIONID");
            this.Property(t => t.Otheraddr).HasColumnName("OTHERADDR");
            this.Property(t => t.Defaultaddr).HasColumnName("DEFAULTADDR");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.Sex).HasColumnName("SEX");
            this.Property(t => t.Nation).HasColumnName("NATION");
            this.Property(t => t.Beginvalidity).HasColumnName("BEGINVALIDITY");
            this.Property(t => t.Endvalidity).HasColumnName("ENDVALIDITY");
            this.Property(t => t.Issuing).HasColumnName("ISSUING");
        }
    }
}
