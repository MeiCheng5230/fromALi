using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenKnowledgeHisMap : EntityTypeConfiguration<TpxinDarenKnowledgeHis>
    {
        public TpxinDarenKnowledgeHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Pinfoid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Pnodeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_KNOWLEDGE_HIS", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_KNOWLEDGE_HIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Pinfoid).HasColumnName("PINFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Pnodeid).HasColumnName("PNODEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
