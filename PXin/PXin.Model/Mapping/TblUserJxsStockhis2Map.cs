using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblUserJxsStockhis2Map : EntityTypeConfiguration<TblUserJxsStockhis2>
    {
        public TblUserJxsStockhis2Map()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Infoid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Totalamount)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Opnodeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TBL_USER_JXS_STOCKHIS2", DbContextHelper.GetOwnerByTableName("TBL_USER_JXS_STOCKHIS2"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Totalamount).HasColumnName("TOTALAMOUNT");
            this.Property(t => t.Opnodeid).HasColumnName("OPNODEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
