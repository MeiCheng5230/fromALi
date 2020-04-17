using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TnetPurseConfigMap : EntityTypeConfiguration<TnetPurseConfig>
    {
        public TnetPurseConfigMap()
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
                    .HasMaxLength(100);
            this.Property(t => t.Picurl)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Pursetype)
                    .IsRequired();
            this.Property(t => t.Subid)
                    .IsRequired();
            this.Property(t => t.Currencytype)
                    .IsRequired();
            this.Property(t => t.Showunit)
                    .IsRequired();
            this.Property(t => t.Showunitname)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Sqldata)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Codedata)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Isshow)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Islocal)
                    .IsRequired();
            this.Property(t => t.Sortnum)
                    .IsRequired();
            this.Property(t => t.Bgpic)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TNET_PURSE_CONFIG", DbContextHelper.GetOwnerByTableName("TNET_PURSE_CONFIG"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Showname).HasColumnName("SHOWNAME");
            this.Property(t => t.Picurl).HasColumnName("PICURL");
            this.Property(t => t.Pursetype).HasColumnName("PURSETYPE");
            this.Property(t => t.Subid).HasColumnName("SUBID");
            this.Property(t => t.Currencytype).HasColumnName("CURRENCYTYPE");
            this.Property(t => t.Showunit).HasColumnName("SHOWUNIT");
            this.Property(t => t.Showunitname).HasColumnName("SHOWUNITNAME");
            this.Property(t => t.Sqldata).HasColumnName("SQLDATA");
            this.Property(t => t.Codedata).HasColumnName("CODEDATA");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Isshow).HasColumnName("ISSHOW");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Islocal).HasColumnName("ISLOCAL");
            this.Property(t => t.Sortnum).HasColumnName("SORTNUM");
            this.Property(t => t.Bgpic).HasColumnName("BGPIC");
                  }
    }
}
