using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatLivegiftTypeMap : EntityTypeConfiguration<TchatLivegiftType>
    {
        public TchatLivegiftTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Giftname)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Giftpic)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Giftpurseid)
                .IsRequired();
            this.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(10, 2);
            this.Property(t => t.Remarks)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Giftcategory)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_LIVEGIFT_TYPE", DbContextHelper.GetOwnerByTableName("TCHAT_LIVEGIFT_TYPE"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Giftname).HasColumnName("GIFTNAME");
            this.Property(t => t.Giftpic).HasColumnName("GIFTPIC");
            this.Property(t => t.Giftpurseid).HasColumnName("GIFTPURSEID");
            this.Property(t => t.Price).HasColumnName("PRICE");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Giftcategory).HasColumnName("GIFTCATEGORY");
        }
    }
}
