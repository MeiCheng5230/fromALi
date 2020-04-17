using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model.Mapping
{
    public class TchatUserFullMap : EntityTypeConfiguration<TchatUserFull>
    {
        public TchatUserFullMap()
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
            this.Property(t => t.Gtclientid)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Devicetoken)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Nickname)
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
            this.Property(t => t.Nodecode)
                    .IsOptional()
                    .HasMaxLength(40);
            this.Property(t => t.Nodename)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Mobileno)
                    .IsOptional()
                    .HasMaxLength(10);
            this.Property(t => t.Email)
                    .IsOptional()
                    .HasMaxLength(20);
            this.Property(t => t.Appphoto)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Gradeid)
                    .IsOptional();
            this.Property(t => t.Gradename)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Teamname)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.IsValidfriend)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TCHAT_USER_FULL", DbContextHelper.GetOwnerByTableName("TCHAT_USER_FULL"));
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Token).HasColumnName("TOKEN");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Gtclientid).HasColumnName("GTCLIENTID");
            this.Property(t => t.Devicetoken).HasColumnName("DEVICETOKEN");
            this.Property(t => t.Nickname).HasColumnName("NICKNAME");
            this.Property(t => t.Sex).HasColumnName("SEX");
            this.Property(t => t.Provinceid).HasColumnName("PROVINCEID");
            this.Property(t => t.Provincename).HasColumnName("PROVINCENAME");
            this.Property(t => t.Cityid).HasColumnName("CITYID");
            this.Property(t => t.Cityname).HasColumnName("CITYNAME");
            this.Property(t => t.Personalsign).HasColumnName("PERSONALSIGN");
            this.Property(t => t.Showrealname).HasColumnName("SHOWREALNAME");
            this.Property(t => t.Nodecode).HasColumnName("NODECODE");
            this.Property(t => t.Nodename).HasColumnName("NODENAME");
            this.Property(t => t.Mobileno).HasColumnName("MOBILENO");
            this.Property(t => t.Email).HasColumnName("EMAIL");
            this.Property(t => t.Appphoto).HasColumnName("APPPHOTO");
            this.Property(t => t.Gradeid).HasColumnName("GRADEID");
            this.Property(t => t.Gradename).HasColumnName("GRADENAME");
            this.Property(t => t.Teamname).HasColumnName("TEAMNAME");
            this.Property(t => t.IsValidfriend).HasColumnName("ISVALIDFRIEND");
        }
    }
}
