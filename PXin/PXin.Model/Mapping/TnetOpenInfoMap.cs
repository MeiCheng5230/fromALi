using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TnetOpenInfoMap : EntityTypeConfiguration<TnetOpenInfo>
    {
        public TnetOpenInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Payment)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Fromtime)
                    .IsRequired();
            this.Property(t => t.Endtime)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TNET_OPEN_INFO", DbContextHelper.GetOwnerByTableName("TNET_OPEN_INFO"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Payment).HasColumnName("PAYMENT");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Fromtime).HasColumnName("FROMTIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
