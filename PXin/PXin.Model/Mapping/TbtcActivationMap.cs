using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TbtcActivationMap : EntityTypeConfiguration<TbtcActivation>
    {
        public TbtcActivationMap()
        {
            // Primary Key
            this.HasKey(t => t.InfoId);
            // Properties
            this.Property(t => t.InfoId)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.CreateTime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.NodeId)
                    .IsRequired();
            this.Property(t => t.Code)
                    .IsRequired()
                    .HasMaxLength(400);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.UserId)
                    .IsOptional();
            this.Property(t => t.CreateTime)
                    .IsRequired();
            this.Property(t => t.UseTime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.EndTime)
                    .IsOptional();
            this.Property(t => t.Sid)
                    .IsRequired();
            this.Property(t => t.ActiveTime)
                    .IsRequired();
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Codetype)
                    .IsRequired();
            this.Property(t => t.BuynodeId)
                    .IsOptional();
            this.Property(t => t.Fromid)
                    .IsOptional();
            this.Property(t => t.Chgnodeid)
                    .IsOptional();
            this.Property(t => t.Chgtransferid)
                    .IsOptional();

            // Table & Column Mappings
            this.ToTable("TBTC_ACTIVATION", DbContextHelper.GetOwnerByTableName("TBTC_ACTIVATION"));
            this.Property(t => t.InfoId).HasColumnName("INFO_ID");
            this.Property(t => t.NodeId).HasColumnName("NODE_ID");
            this.Property(t => t.Code).HasColumnName("CODE");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.UserId).HasColumnName("USER_ID");
            this.Property(t => t.CreateTime).HasColumnName("CREATE_TIME");
            this.Property(t => t.UseTime).HasColumnName("USE_TIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.EndTime).HasColumnName("END_TIME");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.ActiveTime).HasColumnName("ACTIVE_TIME");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Codetype).HasColumnName("CODETYPE");
            this.Property(t => t.BuynodeId).HasColumnName("BUYNODE_ID");
            this.Property(t => t.Fromid).HasColumnName("FROMID");
            this.Property(t => t.Chgnodeid).HasColumnName("CHGNODEID");
            this.Property(t => t.Chgtransferid).HasColumnName("CHGTRANSFERID");
                  }
    }
}
