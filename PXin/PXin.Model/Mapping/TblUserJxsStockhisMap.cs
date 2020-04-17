using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblUserJxsStockhisMap : EntityTypeConfiguration<TblUserJxsStockhis>
    {
        public TblUserJxsStockhisMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Batchnum)
                    .IsRequired();
            this.Property(t => t.Jsxid)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Stocktype)
                    .IsRequired();
            this.Property(t => t.Num)
                    .IsRequired();
            this.Property(t => t.Amountdp)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Amountdos)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Transferids)
                    .IsRequired()
                    .HasMaxLength(50);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Numcua)
                    .IsRequired();
            this.Property(t => t.Rate)
                    .IsRequired()
                    .HasPrecision(12, 2 );
            this.Property(t => t.Pcnnodecode)
                    .IsOptional()
                    .HasMaxLength(20);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Isshow)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TBL_USER_JXS_STOCKHIS", DbContextHelper.GetOwnerByTableName("TBL_USER_JXS_STOCKHIS"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Batchnum).HasColumnName("BATCHNUM");
            this.Property(t => t.Jsxid).HasColumnName("JSXID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Stocktype).HasColumnName("STOCKTYPE");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Amountdp).HasColumnName("AMOUNTDP");
            this.Property(t => t.Amountdos).HasColumnName("AMOUNTDOS");
            this.Property(t => t.Transferids).HasColumnName("TRANSFERIDS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Numcua).HasColumnName("NUMCUA");
            this.Property(t => t.Rate).HasColumnName("RATE");
            this.Property(t => t.Pcnnodecode).HasColumnName("PCNNODECODE");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Isshow).HasColumnName("ISSHOW");
        }
    }
}
