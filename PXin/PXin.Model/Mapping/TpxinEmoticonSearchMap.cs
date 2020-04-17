using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinEmoticonSearchMap : EntityTypeConfiguration<TpxinEmoticonSearch>
    {
        public TpxinEmoticonSearchMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Showname)
                    .IsRequired()
                    .HasMaxLength(200);
            this.Property(t => t.Times)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("TPXIN_EMOTICON_SEARCH", DbContextHelper.GetOwnerByTableName("TPXIN_EMOTICON_SEARCH"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Showname).HasColumnName("SHOWNAME");
            this.Property(t => t.Times).HasColumnName("TIMES");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
