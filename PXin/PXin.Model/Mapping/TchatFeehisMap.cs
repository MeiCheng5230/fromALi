using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatFeehisMap : EntityTypeConfiguration<TchatFeehis>
    {
        public TchatFeehisMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Feetype)
                    .IsRequired();
            this.Property(t => t.Businesstype)
                    .IsRequired();
            this.Property(t => t.Groupid)
                    .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired()
                    .HasPrecision(10, 2);
            this.Property(t => t.Receiver)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(10, 2);
            this.Property(t => t.Sendtime)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Sequenceid)
                    .IsOptional()
                    .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("TCHAT_FEEHIS", DbContextHelper.GetOwnerByTableName("TCHAT_FEEHIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Feetype).HasColumnName("FEETYPE");
            this.Property(t => t.Businesstype).HasColumnName("BUSINESSTYPE");
            this.Property(t => t.Groupid).HasColumnName("GROUPID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Receiver).HasColumnName("RECEIVER");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Sendtime).HasColumnName("SENDTIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Sequenceid).HasColumnName("SEQUENCEID");
        }
    }
}
