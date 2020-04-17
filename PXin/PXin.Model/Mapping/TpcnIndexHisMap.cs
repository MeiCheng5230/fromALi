using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpcnIndexHisMap : EntityTypeConfiguration<TpcnIndexHis>
    {
        public TpcnIndexHisMap()
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
            this.Property(t => t.Num)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Localnum)
                    .IsRequired()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Regnum)
                    .IsOptional();
            this.Property(t => t.Loginnum)
                    .IsOptional();
            this.Property(t => t.Loginipnum)
                    .IsOptional();
            this.Property(t => t.Beforenum)
                    .IsOptional()
                    .HasPrecision(16, 5 );
            this.Property(t => t.Beregnum)
                    .IsOptional();
            this.Property(t => t.Beloginnum)
                    .IsOptional();
            this.Property(t => t.Beloginipnum)
                    .IsOptional();
            this.Property(t => t.Benum)
                    .IsOptional()
                    .HasPrecision(16, 5 );

            // Table & Column Mappings
            this.ToTable("TPCN_INDEX_HIS", DbContextHelper.GetOwnerByTableName("TPCN_INDEX_HIS"));
            this.Property(t => t.Hisid).HasColumnName("HISID");
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Num).HasColumnName("NUM");
            this.Property(t => t.Localnum).HasColumnName("LOCALNUM");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Regnum).HasColumnName("REGNUM");
            this.Property(t => t.Loginnum).HasColumnName("LOGINNUM");
            this.Property(t => t.Loginipnum).HasColumnName("LOGINIPNUM");
            this.Property(t => t.Beforenum).HasColumnName("BEFORENUM");
            this.Property(t => t.Beregnum).HasColumnName("BEREGNUM");
            this.Property(t => t.Beloginnum).HasColumnName("BELOGINNUM");
            this.Property(t => t.Beloginipnum).HasColumnName("BELOGINIPNUM");
            this.Property(t => t.Benum).HasColumnName("BENUM");
                  }
    }
}
