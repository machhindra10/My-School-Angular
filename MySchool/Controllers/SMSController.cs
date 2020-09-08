using Microsoft.AspNetCore.Mvc;
using MySchool.Models;
using System;
using System.IO;
using System.Net;

namespace MySchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase, IDisposable
    {
        private readonly ngSchoolContext _context;


        public SMSController(ngSchoolContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public string SendSMS(string message, string numbers)
        {
            SettingsOthersController settingsOthersController = new SettingsOthersController(_context);
            SettingsOther settings = settingsOthersController.GetSettingsOthersFirstOnly();
            string result = "";

            if(string.IsNullOrEmpty(settings.Smskey) || string.IsNullOrEmpty(settings.Smsprofileid) || string.IsNullOrEmpty(settings.Smssenderid))
            {
                result = "{\"Response\" :{\"Message\" : \"SMS Settings are not available. Please provide settings first.\" },  \"Status\" : \"ERROR\" }";
                return result;
            }

            if (string.IsNullOrEmpty(message))
            {
                result = "{\"Response\" :{\"Message\" : \"Message shouldn't be empty.\" },  \"Status\" : \"ERROR\" }";
                return result;
            }

            if (string.IsNullOrEmpty(numbers))
            {
                result = "{\"Response\" :{\"Message\" : \"Numbers are not provided.\" },  \"Status\" : \"ERROR\" }";
                return result;
            }

            string url = "http://nimbusit.info/api/pushsms.php";
            string strPost = "?user=" + settings.Smsprofileid + "&key=" + settings.Smskey + "&text=" + message + "&sender=" + settings.Smssenderid + "&mobile=" + numbers + "&output=json";

            //string url = "https://app.sainofirst.com/api/apis/bulk-sms";
            //string strPost = "?username=" + settings.Smsusername + "&token=" + settings.Smskey + "&message=" + message + "&senderid=" + settings.Smssenderid + "&number=" + numbers + "&route=Promotional";


            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url + strPost);   //If post remove strPost from here and uncomment below lines

            //objRequest.Method = "POST";
            //objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
            //objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                //myWriter = new StreamWriter(objRequest.GetRequestStream());
                //myWriter.Write(strPost);
            }

            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (myWriter != null)
                {
                    myWriter.Close();
                }

                settings = null;
                settingsOthersController = null;
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }
    }
}