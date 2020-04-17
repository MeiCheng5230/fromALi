using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatRateMap : EntityTypeConfiguration<TchatRate>
    {
        public TchatRateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                   this.Property(t => t.Typeid)
                    .IsOptional();
            this.Property(t => t.Sender)
                    .IsOptional();
            this.Property(t => t.Receiver)
                    .IsOptional();
            this.Property(t => t.Rate)
                    .IsOptional()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Createtime)
                    .IsOptional();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TCHAT_RATE", DbContextHelper.GetOwnerByTableName("TCHAT_RATE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Sender).HasColumnName("SENDER");
            this.Property(t => t.Receiver).HasColumnName("RECEIVER");
            this.Property(t => t.Rate).HasColumnName("RATE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
