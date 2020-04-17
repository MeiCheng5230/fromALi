using System.Web.Mvc;
using Common.Mvc;
namespace PXin.Web.Controllers
{
  /// <summary>
  /// 
  /// </summary>
  public class TestController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    public ActionResult Index()
    {
      //RongFacade facade = new RongFacade();
      // var user = facade.GetPxUserDB(3434909);
      //facade.GtPush(user, "ssss", "yyyyy");
      //LiveFacade live = new LiveFacade();
      //live.Transfer();
      // var wcf = live.GetWcfProxy();
      //int appid = wcf.AppId;
      //bool s = facade.IsFilterWord("丽媛离");
      //bool s2 = facade.IsFilterWord("森林");
      //
      //
      //
      //string pushString = JsonConvert.SerializeObject(new PushDataDemo());
      //string ret = RongCloudServer.PushMessage(AppConfig.AppKey, AppConfig.AppSecret, pushString);
      //ViewBag.PushString = pushString;
      //ViewBag.PushResult = ret;

      //System.IO.FileStream fs = System.IO.File.OpenRead("C:\\Users\\win 10\\Desktop\\QQ视频20181123141144.mp4");//传文件的路径即可
      //System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
      //byte[] bt = br.ReadBytes(Convert.ToInt32(fs.Length));
      //string base64String = Convert.ToBase64String(bt);
      //br.Close();
      //fs.Close();
      //ViewBag.base64image = base64String;



      return View();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    public ActionResult Live()
    {
      return View();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    public ActionResult Doc()
    {
      return View();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    public ActionResult LiveDoc()
    {
      return View();
    }
    /// <summary>
    /// 融云公众号测试
    /// </summary>
    /// <returns></returns>
    [Anonymous]
    public ActionResult RongPublic()
    {
      return Content("OK");
    }
  }
  /// <summary>
  /// 
  /// </summary>
  public class PushDataDemo
  {
    /// <summary>
    /// 
    /// </summary>
    public PushDataDemo()
    {
      platform = new string[] { "ios", "android" };
      fromuserid = "3735222";
      audience = new Audience { userid = new string[] { "3517894", "3795784" } };
      message = new Message { content = "{\"content\":\"1111\",\"extra\":\"aa\"}", objectName = "RC:TxtMsg" };
      notification = new Notification() { alert = "this is a push" };
    }
    /// <summary>
    /// 
    /// </summary>
    public string[] platform { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string fromuserid { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Audience audience { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Message message { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Notification notification { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class Audience
  {
    /// <summary>
    /// 
    /// </summary>
    public string[] userid { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class Message
  {
    /// <summary>
    /// 
    /// </summary>
    public string content { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string objectName { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class Notification
  {
    /// <summary>
    /// 
    /// </summary>
    public string alert { get; set; }
  }

}
