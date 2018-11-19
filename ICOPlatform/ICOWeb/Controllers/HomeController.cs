using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using ICOWebCore.Controllers;
using Newtonsoft.Json;
using ICOCore.Entities.Extra;

namespace ICOWeb.Controllers
{
    public class HomeController : BaseFrontController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ValidateCaptcha()
        {
            var response = Request["g-recaptcha-response"];
            //secret that was generated in key value pair
            const string secret = "6Ld1YgkUAAAAAIbSQSBeM2uSV7sN5dUc3AWXV4-a";

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(reply);

            //when response is false check for the error message
            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0) return View("index");

                var error = captchaResponse.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        ViewBag.Message = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        ViewBag.Message = "The secret parameter is invalid or malformed.";
                        break;

                    case ("missing-input-response"):
                        ViewBag.Message = "The response parameter is missing.";
                        break;
                    case ("invalid-input-response"):
                        ViewBag.Message = "The response parameter is invalid or malformed.";
                        break;

                    default:
                        ViewBag.Message = "Error occured. Please try again";
                        break;
                }
            }
            else
            {
                return RedirectToAction("index", "home", new { area = "" });
            }

            return View("index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}