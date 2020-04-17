using Common.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using PXin.Model;

namespace PXin.Model.Mapping
{
    public class TnetReginfoExtMap : EntityTypeConfiguration<TnetReginfoExt>
    {
        public TnetReginfoExtMap()
        {
            // Primary Key
            this.HasKey(t => t.Extid);
            // Properties
            this.Property(t => t.Extid)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
           this.Property(t => t.Createtime)
                      .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                this.Property(t => t.Nodeid)
                    .IsRequired();
            this.Property(t => t.Weixin)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Createtime)
                    .IsRequired();
            this.Property(t => t.Brightness)
                    .IsRequired()
                    .HasPrecision(13, 2 );
            this.Property(t => t.Gradeid)
                    .IsRequired();
            this.Property(t => t.Province)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.City)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Area)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Country)
                    .IsOptional();
            this.Property(t => t.Gtclientid)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Devicetoken)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.BaiduFaceGroupId)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.BaiduFaceToken)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.BaiduFaceIdcard)
                    .IsOptional()
                    .HasMaxLength(100);
            this.Property(t => t.Idcardpic1)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Idcardpic2)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Idcardpic3)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Token)
                    .IsOptional()
                    .HasMaxLength(400);
            this.Property(t => t.Hbnum)
                    .IsRequired();

            // Table & Column Mappings
            this.ToTable("TNET_REGINFO_EXT", DbContextHelper.GetOwnerByTableName("TNET_REGINFO_EXT"));
            this.Property(t => t.Extid).HasColumnName("EXTID");
            this.Property(t => t.Nodeid).HasColumnName("NODEID");
            this.Property(t => t.Weixin).HasColumnName("WEIXIN");
            this.Property(t => t.Createtime).HasColumnName("CREATETIME");
            this.Property(t => t.Brightness).HasColumnName("BRIGHTNESS");
            this.Property(t => t.Gradeid).HasColumnName("GRADEID");
            this.Property(t => t.Province).HasColumnName("PROVINCE");
            this.Property(t => t.City).HasColumnName("CITY");
            this.Property(t => t.Area).HasColumnName("AREA");
            this.Property(t => t.Country).HasColumnName("COUNTRY");
            this.Property(t => t.Gtclientid).HasColumnName("GTCLIENTID");
            this.Property(t => t.Devicetoken).HasColumnName("DEVICETOKEN");
            this.Property(t => t.BaiduFaceGroupId).HasColumnName("BAIDU_FACE_GROUP_ID");
            this.Property(t => t.BaiduFaceToken).HasColumnName("BAIDU_FACE_TOKEN");
            this.Property(t => t.BaiduFaceIdcard).HasColumnName("BAIDU_FACE_IDCARD");
            this.Property(t => t.Idcardpic1).HasColumnName("IDCARDPIC1");
            this.Property(t => t.Idcardpic2).HasColumnName("IDCARDPIC2");
            this.Property(t => t.Idcardpic3).HasColumnName("IDCARDPIC3");
            this.Property(t => t.Token).HasColumnName("TOKEN");
            this.Property(t => t.Hbnum).HasColumnName("HBNUM");
                  }
    }
}
