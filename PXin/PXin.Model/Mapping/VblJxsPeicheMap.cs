using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Model.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class VblJxsPeicheMap : EntityTypeConfiguration<VblJxsPeiche>
    {

        public VblJxsPeicheMap()
        {
            this.HasKey(t => t.Infoid);
            this.Property(t => t.PeicheStatus)
                .IsRequired();
            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.PeicheStatusShow)
                .IsRequired();
            this.Property(t => t.ApprovalStatus)
                .IsRequired();
            this.Property(t => t.ApprovalStatusShow)
                .IsRequired();
            this.Property(t => t.FreezeStatus)
                .IsRequired();
            this.Property(t => t.FreezeStatusShow)
                .IsRequired();
            this.Property(t => t.PFM)
                .IsRequired();
            this.Property(t => t.SVC)
                .IsRequired();

            this.ToTable("VBL_JXS_PEICHE", DbContextHelper.GetOwnerByTableName("VBL_JXS_PEICHE"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.PeicheStatus).HasColumnName("PEICHESTATUS");
            this.Property(t => t.PeicheStatusShow).HasColumnName("PEICHESTATUSSHOW");
            this.Property(t => t.ApprovalStatus).HasColumnName("APPROVALSTATUS");
            this.Property(t => t.ApprovalStatusShow).HasColumnName("APPROVALSTATUSSHOW");
            this.Property(t => t.FreezeStatus).HasColumnName("FREEZESTATUS");
            this.Property(t => t.FreezeStatusShow).HasColumnName("FREEZESTATUSSHOW");
            this.Property(t => t.PFM).HasColumnName("PFM");
            this.Property(t => t.SVC).HasColumnName("SVC");
        }
    }
}
