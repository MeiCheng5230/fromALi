using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblUserJxsStockconfigMap : EntityTypeConfiguration<TblUserJxsStockconfig>
    {
        public TblUserJxsStockconfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Fromtime)
                    .IsRequired();
            this.Property(t => t.Endtime)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Isfirst)
                    .IsRequired();
            this.Property(t => t.Dp)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Dos)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Lsm)
                    .IsRequired();
            this.Property(t => t.Pfm)
                    .IsRequired();
            this.Property(t => t.Rate)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Subtitle)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Isrenew)
                    .IsRequired();
            this.Property(t => t.Isallowcua)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TBL_USER_JXS_STOCKCONFIG", DbContextHelper.GetOwnerByTableName("TBL_USER_JXS_STOCKCONFIG"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Fromtime).HasColumnName("FROMTIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Isfirst).HasColumnName("ISFIRST");
            this.Property(t => t.Dp).HasColumnName("DP");
            this.Property(t => t.Dos).HasColumnName("DOS");
            this.Property(t => t.Lsm).HasColumnName("LSM");
            this.Property(t => t.Pfm).HasColumnName("PFM");
            this.Property(t => t.Rate).HasColumnName("RATE");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Title).HasColumnName("TITLE");
            this.Property(t => t.Subtitle).HasColumnName("SUBTITLE");
            this.Property(t => t.Isrenew).HasColumnName("ISRENEW");
            this.Property(t => t.Isallowcua).HasColumnName("ISALLOWCUA");
                  }
    }
}
