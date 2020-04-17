using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace PXin.Model.Mapping
{
    /// <summary>
    /// 充值码历史表
    /// </summary>
    public class TblcCentcardHisMap : EntityTypeConfiguration<TblcCentcardHis>
    {
        /// <summary>
        /// 
        /// </summary>
        public TblcCentcardHisMap()
        {
            // Primary Key
            this.HasKey(t => t.Hisid);
            // Properties
            this.Property(t => t.Hisid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Idno)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Opnodeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TBLC_CENTCARD_HIS", DbContextHelper.GetOwnerByTableName("TBLC_CENTCARD_HIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Idno).HasColumnName("IDNO");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Opnodeid).HasColumnName("OPNODEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
                  }
    }
}
