using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenKnowledgeMap : EntityTypeConfiguration<TpxinDarenKnowledge>
    {
        public TpxinDarenKnowledgeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Paytype)
                    .IsRequired();
            this.Property(t => t.Price)
                    .IsRequired();
            this.Property(t => t.Content)
                    .IsRequired()
                    .HasMaxLength(4000);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Modifytime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Voice)
                    .IsOptional()
                    .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_KNOWLEDGE", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_KNOWLEDGE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Title).HasColumnName("TITLE");
            this.Property(t => t.Paytype).HasColumnName("PAYTYPE");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Content).HasColumnName("CONTENT");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Voice).HasColumnName("VOICE");
                  }
    }
}
