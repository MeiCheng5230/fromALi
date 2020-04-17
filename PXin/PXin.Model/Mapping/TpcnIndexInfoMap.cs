using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpcnIndexInfoMap : EntityTypeConfiguration<TpcnIndexInfo>
    {
        public TpcnIndexInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Periods)
                    .IsRequired();
            this.Property(t => t.Islocal)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Num)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Localnum)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Modifytime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Nexttime)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPCN_INDEX_INFO", DbContextHelper.GetOwnerByTableName("TPCN_INDEX_INFO"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Periods).HasColumnName("PERIODS");
            this.Property(t => t.Islocal).HasColumnName("ISLOCAL");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Localnum).HasColumnName("LOCALNUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Nexttime).HasColumnName("NEXTTIME");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
                  }
    }
}
