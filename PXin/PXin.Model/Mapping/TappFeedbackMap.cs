using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TappFeedbackMap : EntityTypeConfiguration<TappFeedback>
    {
        public TappFeedbackMap()
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
            this.Property(t => t.Message)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Image)
                    .IsOptional()
                    .HasMaxLength(2000);
            this.Property(t => t.Client)
                    .IsRequired();
            this.Property(t => t.Version)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remark)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Title)
                    .IsOptional()
                    .HasMaxLength(200);
            this.Property(t => t.Opnodeid)
                    .IsOptional();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Mobile)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TAPP_FEEDBACK", DbContextHelper.GetOwnerByTableName("TAPP_FEEDBACK"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Message).HasColumnName("MESSAGE");
            this.Property(t => t.Image).HasColumnName("IMAGE");
            this.Property(t => t.Client).HasColumnName("CLIENT");
            this.Property(t => t.Version).HasColumnName("VERSION");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remark).HasColumnName("REMARK");
            this.Property(t => t.Title).HasColumnName("TITLE");
            this.Property(t => t.Opnodeid).HasColumnName("OPNODEID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Mobile).HasColumnName("MOBILE");
        }
    }
}
