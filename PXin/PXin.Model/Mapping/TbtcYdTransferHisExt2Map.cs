using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TbtcYdTransferHisExt2Map : EntityTypeConfiguration<TbtcYdTransferHisExt2>
    {
        public TbtcYdTransferHisExt2Map()
        {
            // Primary Key
            this.HasKey(t => t.Extid);
            // Properties
            this.Property(t => t.Extid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Hisid)
                    .IsRequired();
            this.Property(t => t.Infoid)
                    .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Endtime)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TBTC_YD_TRANSFER_HIS_EXT2", DbContextHelper.GetOwnerByTableName("TBTC_YD_TRANSFER_HIS_EXT2"));
            this.Property(t => t.Extid).HasColumnName("EXTID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
