using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TnetUepayhisMap : EntityTypeConfiguration<TnetUepayhis>
    {
        public TnetUepayhisMap()
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
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.BusinessParams)
                    .IsRequired()
                    .HasMaxLength(500);
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(10, 2 );
            this.Property(t => t.Freezeids)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired()
                    .IsConcurrencyToken();//非常重要
            this.Property(t => t.Ordernoue)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Noticetime)
                    .IsOptional();
            this.Property(t => t.BusinessId)
                    .IsRequired();
            this.Property(t => t.Unit)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TNET_UEPAYHIS", DbContextHelper.GetOwnerByTableName("TNET_UEPAYHIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.BusinessParams).HasColumnName("BUSINESS_PARAMS");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Freezeids).HasColumnName("FREEZEIDS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Ordernoue).HasColumnName("ORDERNOUE");
            this.Property(t => t.Noticetime).HasColumnName("NOTICETIME");
            this.Property(t => t.BusinessId).HasColumnName("BUSINESS_ID");
            this.Property(t => t.Unit).HasColumnName("UNIT");
                  }
    }
}
