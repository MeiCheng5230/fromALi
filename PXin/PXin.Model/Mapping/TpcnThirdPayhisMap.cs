using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpcnThirdPayhisMap : EntityTypeConfiguration<TpcnThirdPayhis>
    {
        public TpcnThirdPayhisMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Partnerid)
                    .IsRequired();
            this.Property(t => t.Paytype)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(13, 2 );
            this.Property(t => t.Orderno)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Subject)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Body)
                    .IsOptional()
                    .HasMaxLength(500);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Paystatus)
                    .IsRequired();
            this.Property(t => t.Transferids)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Notifyurl)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Storequest)
                    .IsRequired();
            this.Property(t => t.Nextnotifytime)
                    .IsRequired();
            this.Property(t => t.Notifyfailnumber)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPCN_THIRD_PAYHIS", DbContextHelper.GetOwnerByTableName("TPCN_THIRD_PAYHIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Partnerid).HasColumnName("PARTNERID");
            this.Property(t => t.Paytype).HasColumnName("PAYTYPE");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Orderno).HasColumnName("ORDERNO");
            this.Property(t => t.Subject).HasColumnName("SUBJECT");
            this.Property(t => t.Body).HasColumnName("BODY");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Paystatus).HasColumnName("PAYSTATUS");
            this.Property(t => t.Transferids).HasColumnName("TRANSFERIDS");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Notifyurl).HasColumnName("NOTIFYURL");
            this.Property(t => t.Storequest).HasColumnName("STOREQUEST");
            this.Property(t => t.Nextnotifytime).HasColumnName("NEXTNOTIFYTIME");
            this.Property(t => t.Notifyfailnumber).HasColumnName("NOTIFYFAILNUMBER");
                  }
    }
}
