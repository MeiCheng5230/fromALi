using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// 卡表
    /// </summary>
    public class TblcCentcardMap : EntityTypeConfiguration<TblcCentcard>
    {
        /// <summary>
        /// 
        /// </summary>
        public TblcCentcardMap()
        {
            // Primary Key
            this.HasKey(t => t.Idno);
            // Properties
            this.Property(t => t.Idno)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Cardno)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Status)
                    .IsRequired()
                    .IsConcurrencyToken();
            this.Property(t => t.Cardpwd)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Ispwdrequired)
                    .IsOptional();
            this.Property(t => t.Amount)
                    .IsOptional()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Sid)
                    .IsOptional();
            this.Property(t => t.Expiredtime)
                    .IsOptional();
            this.Property(t => t.Createdtime)
                    .IsOptional();
            this.Property(t => t.Areaid)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Status)
                    .IsOptional();
            this.Property(t => t.Usenodeid)
                    .IsOptional();
            this.Property(t => t.Usedate)
                    .IsOptional();
            this.Property(t => t.Productid)
                    .IsOptional();
            this.Property(t => t.Assignbusiid)
                    .IsOptional();
            this.Property(t => t.Assignvalue)
                    .IsOptional()
                    .HasPrecision(13, 2 );
            this.Property(t => t.Assignnodeid)
                    .IsOptional();
            this.Property(t => t.Assigntime)
                    .IsOptional();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Period)
                    .IsOptional()
                    .HasMaxLength(5);
            this.Property(t => t.Fromid)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TBLC_CENTCARD", DbContextHelper.GetOwnerByTableName("TBLC_CENTCARD"));
            this.Property(t => t.Idno).HasColumnName("IDNO");
            this.Property(t => t.Cardno).HasColumnName("CARDNO");
            this.Property(t => t.Cardpwd).HasColumnName("CARDPWD");
            this.Property(t => t.Ispwdrequired).HasColumnName("ISPWDREQUIRED");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Expiredtime).HasColumnName("EXPIREDTIME");
            this.Property(t => t.Createdtime).HasColumnName("CREATEDTIME");
            this.Property(t => t.Areaid).HasColumnName("AREAID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Usenodeid).HasColumnName("USENODEID");
            this.Property(t => t.Usedate).HasColumnName("USEDATE");
            this.Property(t => t.Productid).HasColumnName("PRODUCTID");
            this.Property(t => t.Assignbusiid).HasColumnName("ASSIGNBUSIID");
            this.Property(t => t.Assignvalue).HasColumnName("ASSIGNVALUE");
            this.Property(t => t.Assignnodeid).HasColumnName("ASSIGNNODEID");
            this.Property(t => t.Assigntime).HasColumnName("ASSIGNTIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Period).HasColumnName("PERIOD");
            this.Property(t => t.Fromid).HasColumnName("FROMID");
                  }
    }
}
