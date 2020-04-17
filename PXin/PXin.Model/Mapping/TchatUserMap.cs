using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TchatUserMap : EntityTypeConfiguration<TchatUser>
    {
        public TchatUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Createtime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Nodeid)
                .IsRequired();
            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(100);
            this.Property(t => t.Createtime)
                .IsRequired();
            this.Property(t => t.Nickname)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Gtclientid)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Devicetoken)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Sex)
                .IsOptional()
                .HasMaxLength(10);
            this.Property(t => t.Provinceid)
                .IsRequired();
            this.Property(t => t.Provincename)
                .IsOptional()
                .HasMaxLength(50);
            this.Property(t => t.Cityid)
                .IsRequired();
            this.Property(t => t.Cityname)
                .IsOptional()
                .HasMaxLength(50);
            this.Property(t => t.Personalsign)
                .IsOptional()
                .HasMaxLength(100);
            this.Property(t => t.Showrealname)
                .IsRequired();
            this.Property(t => t.IsValidfriend)
                .IsRequired();
            this.Property(t => t.IsSysNotice)
               .IsRequired();
            this.Property(t => t.IsNoticeDetail)
               .IsRequired();


            // Table & Column Mappings
            this.ToTable("TCHAT_USER", DbContextHelper.GetOwnerByTableName("TCHAT_USER"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Token).HasColumnName("TOKEN");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Nickname).HasColumnName("NICKNAME");
            this.Property(t => t.Gtclientid).HasColumnName("GTCLIENTID");
            this.Property(t => t.Devicetoken).HasColumnName("DEVICETOKEN");
            this.Property(t => t.Sex).HasColumnName("SEX");
            this.Property(t => t.Provinceid).HasColumnName("PROVINCEID");
            this.Property(t => t.Provincename).HasColumnName("PROVINCENAME");
            this.Property(t => t.Cityid).HasColumnName("CITYID");
            this.Property(t => t.Cityname).HasColumnName("CITYNAME");
            this.Property(t => t.Personalsign).HasColumnName("PERSONALSIGN");
            this.Property(t => t.Showrealname).HasColumnName("SHOWREALNAME");
            this.Property(t => t.IsValidfriend).HasColumnName("ISVALIDFRIEND");
            this.Property(t => t.IsSysNotice).HasColumnName("ISSYSNOTICE");
            this.Property(t => t.IsNoticeDetail).HasColumnName("ISNOTICEDETAIL");
        }
    }
}
