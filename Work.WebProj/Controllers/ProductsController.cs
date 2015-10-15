﻿using System.Linq;
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
    public class ProductsController : WebUserController
    {
        public ActionResult list()
        {
            return View("Products_list");
        }
        public ActionResult content()
        {
            return View("Products_content");
        }
    }
}
