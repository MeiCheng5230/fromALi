using Common.Facade;
using Common.Mvc;
using PXin.Facade;
using PXin.Facade.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PXin.Facade.CommonService
{
    /// <summary>
    /// 
    /// </summary>
    public class FileService
    {
        private static Log log = new Log(typeof(FileService));

        /// <summary>
        /// 
        /// </summary>
        public const string TempDir = "tempfile";
        /// <summary>
        /// 
        /// </summary>
        public const string UserAuthDir = "userauth";
        /// <summary>
        /// 
        /// </summary>
        public const string UserPhotoDir = "userphoto";

        /// <summary>
        /// 
        /// </summary>
        public const string UserFeedback = "userfeedback";

        /// <summary>
        /// 
        /// </summary>
        public const string DriverLicense = "driverlicense";

        /// <summary>
        /// 获取根目录路径
        /// </summary>
        /// <returns></returns>
        public static string GetPhysicsRootDir()
        {
            string physicsFilePath = System.Web.Hosting.HostingEnvironment.MapPath(AppConfig.FileRootDir);
            return physicsFilePath;
        }

        /// <summary>
        /// 获取图片Url
        /// </summary>
        public static string GetAppPhoto(string photo)
        {
            if (photo.IsNullOrWhiteSpace())
            {
                return "http://global.p.cn/img/default_user1.png";//默认头像
            }
            else
            {
                if (photo.StartsWith("http"))
                {
                    return photo;
                }
                return AppConfig.ImageBaseUrl + photo;
            }
        }

        /// <summary>
        /// 获取图片物理地址绝对路径
        /// </summary>
        public static string GetPhysicsFilePath(string photo)
        {
            if (photo.IsNullOrWhiteSpace())
            {
                return "";
            }
            FileService fileService = new FileService();
            fileService.FilePath = photo;
            return fileService.PhysicsFilePath;
        }

        /// <summary>
        /// 相对物理路径，images2是图片站点的根目录，所以不应该包含images2
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 相对物理路径
        /// </summary>
        public string Image2FilePath
        {
            get
            {
                return AppConfig.FileRootDir + FilePath;
            }
        }

        /// <summary>
        /// 绝对物理路径，存在服务器上的完整路径
        /// </summary>
        public string PhysicsFilePath
        {
            get
            {
                string physicsFilePath = System.Web.Hosting.HostingEnvironment.MapPath(AppConfig.FileRootDir + FilePath);
                return physicsFilePath;
            }
        }

        /// <summary>
        /// 文件夹绝对物理路径，存在服务器上的完整路径
        /// </summary>
        public string PhysicsDirPath
        {
            get
            {
                string physicsFilePath = System.Web.Hosting.HostingEnvironment.MapPath(AppConfig.FileRootDir + FilePath);
                return physicsFilePath;
            }
        }

        /// <summary>
        /// 互联网访问路径
        /// </summary>
        public string FullUrl
        {
            get
            {
                return AppConfig.ImageBaseUrl + FilePath;
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool SaveFile(ReqUploadFile req)
        {
            string saveDir = TempDir;
            if (req.ImageActionType == FileActionType.身份证正面图片 || req.ImageActionType == FileActionType.身份证反面图片 || req.ImageActionType == FileActionType.百度人脸识别图片 || req.ImageActionType == FileActionType.驾驶证行驶证 || req.ImageActionType == FileActionType.驾驶证副页)
            {
                saveDir = TempDir;//先存到临时目录,提交认证时移到 UserAuthDir
            }
            else if (req.ImageActionType == FileActionType.用户头像)
            {
                saveDir = UserPhotoDir;
            }
            else if (req.ImageActionType == FileActionType.意见反馈)
            {
                saveDir = UserFeedback;
            }

            string dirPath = $"/{saveDir}/{DateTime.Now.ToString("yyyyMMdd")}";
            string dirHostPath = System.Web.Hosting.HostingEnvironment.MapPath(AppConfig.FileRootDir + dirPath);
            if (!Directory.Exists(dirHostPath))
            {
                Directory.CreateDirectory(dirHostPath);
            }

            FilePath = $"{dirPath}/{Guid.NewGuid().ToString("N")}.{req.Typeid}";

            var isOk = Helper.Base64StringToImage(req.Content, PhysicsFilePath, req.Typeid);//保存文件
            if (!isOk) return false;

            return true;
        }

        /// <summary>
        /// 临时目录转到正式目录并补全绝对路径
        /// </summary>
        public string CombinePicUrl(string pic, DateTime date, FileActionType fileActionType)
        {
            if (string.IsNullOrEmpty(pic))
            {
                return "";
            }

            pic = ChangePicDir(pic, date, fileActionType);

            pic = AppConfig.ImageBaseUrl.TrimEnd('/') + "/" + pic.TrimStart('/');
            return pic;
        }

        private string ChangePicDir(string pic, DateTime date, FileActionType fileActionType)
        {
            string dir = FileService.GetPhysicsRootDir();
            string saveFilePath = FileService.TempDir;
            if (fileActionType == FileActionType.身份证反面图片 || fileActionType == FileActionType.身份证正面图片 || fileActionType == FileActionType.百度人脸识别图片)
            {
                saveFilePath = FileService.UserAuthDir;
            }
            else if (fileActionType == FileActionType.意见反馈)
            {
                saveFilePath = FileService.UserFeedback;
            }
            else if (fileActionType == FileActionType.用户头像)
            {
                saveFilePath = FileService.UserPhotoDir;
            }
            else if (fileActionType == FileActionType.驾驶证行驶证 || fileActionType == FileActionType.驾驶证副页)
            {
                saveFilePath = FileService.DriverLicense;
            }
            string destDir = Path.Combine(dir, saveFilePath, date.ToString("yyyyMMdd"));
            if (!Directory.Exists(destDir))
            {
                try
                {
                    Directory.CreateDirectory(destDir);
                }
                catch (Exception ex)
                {
                    log.Info("创建目录失败," + destDir, ex);
                }
            }
            pic = pic.TrimStart('/').Replace(AppConfig.FileRootDir, "").Replace("/", "\\");
            string srcFile = Path.Combine(dir, pic);
            string destFile = Path.Combine(destDir, Path.GetFileName(pic));
            if (srcFile.Replace("\\", "/").Equals(destFile.Replace("\\", "/"), StringComparison.OrdinalIgnoreCase))
            {
                return pic.Replace("\\", "/");
            }
            try
            {
                if (File.Exists(srcFile))
                {
                    File.Move(srcFile, destFile);
                }
            }
            catch (Exception ex)
            {
                log.Info($"Move文件失败,马上再次尝试Copy,{srcFile}->{destFile}", ex);
                try
                {
                    File.Copy(srcFile, destFile);
                    log.Info($"Copy文件成功,{srcFile}->{destFile}");
                }
                catch (Exception ex2)
                {
                    log.Info($"Copy文件也失败,直接返回临时目录文件,{srcFile}->{destFile}", ex2);
                    return srcFile.Replace(dir, "").Replace("\\", "/");
                }
            }
            return destFile.Replace(dir, "").Replace("\\", "/");
        }

        /// <summary>
        /// 将服务器本地图片转为Base64格式
        /// </summary>
        /// <param name="photoPhysicalPath">绝对路径</param>
        /// <returns></returns>
        public static string GetImageBase64(string photoPhysicalPath)
        {
            if (!File.Exists(photoPhysicalPath))
            {
                return string.Empty;
            }
            string base64String = string.Empty;
            Bitmap bmp1 = new Bitmap(photoPhysicalPath);
            using (MemoryStream ms1 = new MemoryStream())
            {
                bmp1.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr1 = new byte[ms1.Length];
                ms1.Position = 0;
                ms1.Read(arr1, 0, (int)ms1.Length);
                ms1.Close();
                base64String = Convert.ToBase64String(arr1);
            }
            return base64String;
        }
    }
}
