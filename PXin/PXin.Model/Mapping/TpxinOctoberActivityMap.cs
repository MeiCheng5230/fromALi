using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Map
{
    public class TpxinOctoberActivityMap : EntityTypeConfiguration<TpxinOctoberActivity>
    {
        public TpxinOctoberActivityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.Id)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Amount)
                    .IsRequired()
                    .HasPrecision(10, 2 );
            this.Property(t => t.Transferids)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Transfertime)
                    .IsOptional();
            this.Property(t => t.Pnodeid)
                    .IsRequired();
            this.Property(t => t.Pnote)
                    .IsRequired()
                    .HasMaxLength(100);
            this.Property(t => t.Pamount)
                    .IsRequired()
                    .HasPrecision(10, 2 );
            this.Property(t => t.Ptransferids)
                    .IsOptional()
                    .HasMaxLength(50);
            this.Property(t => t.Ptransfertime)
                    .IsOptional();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Expressno)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Sendtime)
                    .IsOptional();
            this.Property(t => t.ActivityId)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_OCTOBER_ACTIVITY", DbContextHelper.GetOwnerByTableName("TPXIN_OCTOBER_ACTIVITY"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Amount).HasColumnName("AMOUNT");
            this.Property(t => t.Transferids).HasColumnName("TRANSFERIDS");
            this.Property(t => t.Transfertime).HasColumnName("TRANSFERTIME");
            this.Property(t => t.Pnodeid).HasColumnName("PNODEID");
            this.Property(t => t.Pnote).HasColumnName("PNOTE");
            this.Property(t => t.Pamount).HasColumnName("PAMOUNT");
            this.Property(t => t.Ptransferids).HasColumnName("PTRANSFERIDS");
            this.Property(t => t.Ptransfertime).HasColumnName("PTRANSFERTIME");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Expressno).HasColumnName("EXPRESSNO");
            this.Property(t => t.Sendtime).HasColumnName("SENDTIME");
            this.Property(t => t.ActivityId).HasColumnName("ACTIVITY_ID");
        }
    }
}
