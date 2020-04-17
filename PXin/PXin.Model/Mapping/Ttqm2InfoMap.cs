using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class Ttqm2InfoMap : EntityTypeConfiguration<Ttqm2Info>
    {
        public Ttqm2InfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Pwd)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Price)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Fee)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Modifytime)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TTQM2_INFO", DbContextHelper.GetOwnerByTableName("TTQM2_INFO"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Pwd).HasColumnName("PWD");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Fee).HasColumnName("FEE");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
                  }
    }
}
