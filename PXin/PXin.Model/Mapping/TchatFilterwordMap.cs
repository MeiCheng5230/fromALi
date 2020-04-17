using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TchatFilterwordMap : EntityTypeConfiguration<TchatFilterword>
    {
        public TchatFilterwordMap()
        {
            this.Property(t => t.Id)
             .IsOptional();
            this.Property(t => t.Filterword)
                    .IsOptional()
                    .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("TCHAT_FILTERWORD", DbContextHelper.GetOwnerByTableName("TCHAT_FILTERWORD"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Filterword).HasColumnName("FILTERWORD");
        }
    }
}
