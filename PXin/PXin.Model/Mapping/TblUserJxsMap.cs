using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TblUserJxsMap : EntityTypeConfiguration<TblUserJxs>
    {
        public TblUserJxsMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.Property(t => t.Createtime)
                       .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(t => t.Pinfoid)
                .IsOptional();
            this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Province)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.City)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Region)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.Note)
                    .IsRequired()
                    .HasMaxLength(25);
            this.Property(t => t.Opnodeid)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Typeid)
                    .IsRequired();
            this.Property(t => t.Sid)
                    .IsRequired();
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Lastdate)
                    .IsOptional();
            this.Property(t => t.Jsxname)
                    .IsOptional()
                    .HasMaxLength(25);
            this.Property(t => t.PicContract)
                    .IsOptional()
                    .HasMaxLength(1200);
            this.Property(t => t.PicCompany)
                    .IsOptional()
                    .HasMaxLength(1200);
            this.Property(t => t.PicHold)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.PicIdentfront)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.PicIdentback)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.PicLicense)
                    .IsOptional()
                    .HasMaxLength(200);
            this.Property(t => t.Stocknum)
                    .IsRequired();
            this.Property(t => t.Istrain)
                    .IsRequired();
            this.Property(t => t.Status2)
                    .IsRequired();
            this.Property(t => t.Znhy)
                    .IsRequired();
            this.Property(t => t.PicExt)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Status3)
                    .IsRequired();
            this.Property(t => t.Remarks3)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.LicenseStatus)
                    .IsRequired();
            this.Property(t => t.Znhyprice)
                    .IsRequired()
                    .HasPrecision(12, 2);
            this.Property(t => t.PicPermit)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.PicLease)
                    .IsOptional()
                    .HasMaxLength(200);
            this.Property(t => t.Nochecktime)
                    .IsRequired();
            this.Property(t => t.Isfirst)
                    .IsRequired();
            this.Property(t => t.Starttime)
                    .IsRequired();
            this.Property(t => t.Endtime)
                    .IsRequired();
            this.Property(t => t.Chgtypedate)
                    .IsRequired();
            this.Property(t => t.Stocknum2)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TBL_USER_JXS", DbContextHelper.GetOwnerByTableName("TBL_USER_JXS"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Pinfoid).HasColumnName("PINFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Province).HasColumnName("PROVINCE");
            this.Property(t => t.City).HasColumnName("CITY");
            this.Property(t => t.Region).HasColumnName("REGION");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Opnodeid).HasColumnName("OPNODEID");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Typeid).HasColumnName("TYPEID");
            this.Property(t => t.Sid).HasColumnName("SID");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Lastdate).HasColumnName("LASTDATE");
            this.Property(t => t.Jsxname).HasColumnName("JSXNAME");
            this.Property(t => t.PicContract).HasColumnName("PIC_CONTRACT");
            this.Property(t => t.PicCompany).HasColumnName("PIC_COMPANY");
            this.Property(t => t.PicHold).HasColumnName("PIC_HOLD");
            this.Property(t => t.PicIdentfront).HasColumnName("PIC_IDENTFRONT");
            this.Property(t => t.PicIdentback).HasColumnName("PIC_IDENTBACK");
            this.Property(t => t.PicLicense).HasColumnName("PIC_LICENSE");
            this.Property(t => t.Stocknum).HasColumnName("STOCKNUM");
            this.Property(t => t.Istrain).HasColumnName("ISTRAIN");
            this.Property(t => t.Status2).HasColumnName("STATUS2");
            this.Property(t => t.Znhy).HasColumnName("ZNHY");
            this.Property(t => t.PicExt).HasColumnName("PIC_EXT");
            this.Property(t => t.Status3).HasColumnName("STATUS3");
            this.Property(t => t.Remarks3).HasColumnName("REMARKS3");
            this.Property(t => t.LicenseStatus).HasColumnName("LICENSE_STATUS");
            this.Property(t => t.Znhyprice).HasColumnName("ZNHYPRICE");
            this.Property(t => t.PicPermit).HasColumnName("PIC_PERMIT");
            this.Property(t => t.PicLease).HasColumnName("PIC_LEASE");
            this.Property(t => t.Nochecktime).HasColumnName("NOCHECKTIME");
            this.Property(t => t.Isfirst).HasColumnName("ISFIRST");
            this.Property(t => t.Starttime).HasColumnName("STARTTIME");
            this.Property(t => t.Endtime).HasColumnName("ENDTIME");
            this.Property(t => t.Chgtypedate).HasColumnName("CHGTYPEDATE");
            this.Property(t => t.Stocknum2).HasColumnName("STOCKNUM2");
        }
    }
}
