using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinSvcLimitMap : EntityTypeConfiguration<TpxinSvcLimit>
    {
        public TpxinSvcLimitMap()
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
            this.Property(t => t.Totalamount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Localamount1)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Localamount2)
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
            this.ToTable("TPXIN_SVC_LIMIT", DbContextHelper.GetOwnerByTableName("TPXIN_SVC_LIMIT"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Totalamount).HasColumnName("TOTALAMOUNT");
            this.Property(t => t.Localamount1).HasColumnName("LOCALAMOUNT1");
            this.Property(t => t.Localamount2).HasColumnName("LOCALAMOUNT2");
            this.Property(t => t.Fromtime).HasColumnName("FROMTIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
