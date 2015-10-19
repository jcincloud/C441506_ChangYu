using DotWeb.CommSetup;
using DotWeb.Controller;
using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace DotWeb.Areas.Active.Controllers
{
    public class ProductDataController : AdminController
    {
        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View();
        }
        public ActionResult CategoryL1()
        {
            ActionRun();
            return View();
        }
        public ActionResult CategoryL2()
        {
            ActionRun();
            return View();
        }

        #endregion

        #region ajax call section

        public string aj_Init()
        {
            using (var db0 = getDB0())
            {
                return defJSON(new
                {
                    // options_equipment_category = db0.Equipment_Category.OrderBy(x=>x.sort)
                });
            }
        }
        public string l2_Init()
        {
            using (var db0 = getDB0())
            {
                return defJSON(new
                {
                    options_category_l1 = db0.Product_Category_L1.Where(x => !x.i_Hide).OrderByDescending(x => x.l1_sort).Select(x => new option() { val = x.product_category_l1_id, Lname = x.l1_name })
                });
            }
        }
        public string product_Init()
        {
            List<L1> items = new List<L1>();
            using (var db0 = getDB0())
            {
                items = db0.Product_Category_L1.Where(x => !x.i_Hide).OrderByDescending(x => x.l1_sort)
                               .Select(x => new L1()
                               {
                                   l1_id = x.product_category_l1_id,
                                   l1_name = x.l1_name
                               }).ToList();

                foreach (var item in items)
                {
                    item.l2_list = db0.Product_Category_L2.Where(x => !x.i_Hide & x.l1_id == item.l1_id).OrderByDescending(x => x.l2_sort)
                                                            .Select(x => new L2()
                                                            {
                                                                l2_id = x.product_category_l2_id,
                                                                l2_name = x.l2_name
                                                            }).ToList();
                }

                return defJSON(new
                {
                    options_category = items
                });
            }
        }
        #endregion

        #region ajax file section
        [HttpPost]
        public string axFUpload(int id, string filekind, string filename)
        {
            UpFileInfo r = new UpFileInfo();
            #region
            try
            {
                if (filekind == "Photo1")
                    handleImageSave(filename, id, ImageFileUpParm.ProductIndex, filekind, "ProductData", "Photo");
                if (filekind == "Photo2")
                    handleImageSave(filename, id, ImageFileUpParm.ProductImgs, filekind, "ProductData", "Photo");

                r.result = true;
                r.file_name = filename;
            }
            catch (LogicError ex)
            {
                r.result = false;
                r.message = getRecMessage(ex.Message);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
            }
            #endregion
            return defJSON(r);
        }

        [HttpPost]
        public string axFList(int id, string filekind)
        {
            SerializeFileList r = new SerializeFileList();

            r.files = listImgFiles(id, filekind, "ProductData", "Photo");
            r.result = true;
            return defJSON(r);
        }
        [HttpPost]
        public string axFSort(int id, string filekind, IList<JsonFileInfo> file_object)
        {
            ResultInfo r = new ResultInfo();

            rewriteJsonFile(id, filekind, "ProductData", "Photo", file_object);

            r.result = true;
            return defJSON(r);
        }
        [HttpPost]
        public string axFDelete(int id, string filekind, string filename)
        {
            ResultInfo r = new ResultInfo();
            DeleteSysFile(id, filekind, filename, ImageFileUpParm.NewsBasicSingle, "ProductData", "Photo");
            r.result = true;
            return defJSON(r);
        }


        [HttpGet]
        public FileResult axFDown(int id, string filekind, string filename)
        {
            string path_tpl = string.Format(upload_path_tpl_o, "ProductData", "Photo", id, filekind, filename);
            string server_path = Server.MapPath(path_tpl);
            FileInfo file_info = new FileInfo(server_path);
            FileStream file_stream = new FileStream(server_path, FileMode.Open, FileAccess.Read);
            string web_path = Url.Content(path_tpl);
            return File(file_stream, "application/*", file_info.Name);
        }
        #endregion
    }
}