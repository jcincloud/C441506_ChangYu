using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using ProcCore.HandleResult;
using DotWeb.CommSetup;
using DotWeb.Controller;
namespace DotWeb.Controllers
{
    public class InquireController : WebUserController
    {
        public ActionResult Index()
        {
            return View("Inquire");
        }
        public string sendMail(InquireMail md)
        {
            ResultInfo r = new ResultInfo();
            try
            {
                using (db0 = getDB0())
                {
                    #region 信件發送
                    string Body = getMailBody("Email", md);//套用信件版面
                    Boolean mail;
                    mail = Mail_Send(md.email, //寄信人
                                    openLogic().getReceiveMails(), //收信人
                                    CommWebSetup.MailTitle, //信件標題
                                    Body, //信件內容
                                    true); //是否為html格式
                    if (mail == false)
                    {
                        r.result = false;
                        r.message = "信箱號碼不正確或送信失敗!";
                        return defJSON(r);
                    }
                    #endregion
                }
                r.result = true;

            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
            }
            return defJSON(r);
        }
    }
    public class InquireMail
    {
        public string company_name { get; set; }
        public string contact_person { get; set; }
        public string job_title { get; set; }
        public string tel { get; set; }
        public string tel_ex { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public int[] contact_type { get; set; }
        public string content { get; set; }
    }
}
