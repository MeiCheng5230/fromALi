using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinEmoticonOrderMap : EntityTypeConfiguration<TpxinEmoticonOrder>
    {
        public TpxinEmoticonOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Materialid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired();
            this.Property(t => t.Transferid)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TPXIN_EMOTICON_ORDER", DbContextHelper.GetOwnerByTableName("TPXIN_EMOTICON_ORDER"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Materialid).HasColumnName("MATERIALID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Transferid).HasColumnName("TRANSFERID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
