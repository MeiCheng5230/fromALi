using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TbossMeethisMap : EntityTypeConfiguration<TbossMeethis>
    {
        public TbossMeethisMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Infoid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Mobileno)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Name)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.JoinPersons)
                    .IsOptional()
                    .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("TBOSS_MEETHIS", DbContextHelper.GetOwnerByTableName("TBOSS_MEETHIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Mobileno).HasColumnName("MOBILENO");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Name).HasColumnName("NAME");
            this.Property(t => t.JoinPersons).HasColumnName("JOINPERSONS");
                  }
    }
}
