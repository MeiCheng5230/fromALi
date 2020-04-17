using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TpxinDarenInfoMap : EntityTypeConfiguration<TpxinDarenInfo>
    {
        public TpxinDarenInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Infoid);
            // Properties
            this.Property(t => t.Infoid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Introduce)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Specializedpic)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Greetings)
                    .IsOptional()
                    .HasMaxLength(200);
            this.Property(t => t.Welcome)
                    .IsOptional()
                    .HasMaxLength(500);
            this.Property(t => t.Status)
                    .IsRequired();
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Modifytime)
                    .IsRequired();
            this.Property(t => t.Note)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Remarks)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Introducepic)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Typename)
                    .IsOptional()
                    .HasMaxLength(1000);
            this.Property(t => t.Position)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Company)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Ischange)
                    .IsRequired();
            this.Property(t => t.Voiceaddress)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Protectrate)
                    .IsRequired();
            this.Property(t => t.Praisenum)
                    .IsRequired();
            this.Property(t => t.Browsenum)
                    .IsRequired();
            this.Property(t => t.Voicebrowsenum)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TPXIN_DAREN_INFO", DbContextHelper.GetOwnerByTableName("TPXIN_DAREN_INFO"));
            this.Property(t => t.Infoid).HasColumnName("INFOID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Introduce).HasColumnName("INTRODUCE");
            this.Property(t => t.Specializedpic).HasColumnName("SPECIALIZEDPIC");
            this.Property(t => t.Greetings).HasColumnName("GREETINGS");
            this.Property(t => t.Welcome).HasColumnName("WELCOME");
            this.Property(t => t.Status).HasColumnName("STATUS");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Modifytime).HasColumnName("MODIFYTIME");
            this.Property(t => t.Note).HasColumnName("NOTE");
            this.Property(t => t.Remarks).HasColumnName("REMARKS");
            this.Property(t => t.Introducepic).HasColumnName("INTRODUCEPIC");
            this.Property(t => t.Typename).HasColumnName("TYPENAME");
            this.Property(t => t.Position).HasColumnName("POSITION");
            this.Property(t => t.Company).HasColumnName("COMPANY");
            this.Property(t => t.Ischange).HasColumnName("ISCHANGE");
            this.Property(t => t.Voiceaddress).HasColumnName("VOICEADDRESS");
            this.Property(t => t.Protectrate).HasColumnName("PROTECTRATE");
            this.Property(t => t.Praisenum).HasColumnName("PRAISENUM");
            this.Property(t => t.Browsenum).HasColumnName("BROWSENUM");
            this.Property(t => t.Voicebrowsenum).HasColumnName("VOICEBROWSENUM");
                  }
    }
}
