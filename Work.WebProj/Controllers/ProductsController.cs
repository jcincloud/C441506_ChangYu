using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using DotWeb.Controller;
using ProcCore.Business.DB0;

namespace DotWeb.Controllers
{
    public class ProductsController : WebUserController
    {
        public ActionResult list()
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
            }
            return View("Products_list", items);
        }
        public ActionResult content(int? l2_id)
        {
            ProductContentInfo info = new ProductContentInfo();

            using (var db0 = getDB0())
            {
                info.p_list = db0.Product_Category_L1.Where(x => !x.i_Hide).OrderByDescending(x => x.l1_sort)
                                               .Select(x => new L1()
                                               {
                                                   l1_id = x.product_category_l1_id,
                                                   l1_name = x.l1_name
                                               }).ToList();

                foreach (var item in info.p_list)
                {
                    item.l2_list = db0.Product_Category_L2.Where(x => !x.i_Hide & x.l1_id == item.l1_id).OrderByDescending(x => x.l2_sort)
                                                            .Select(x => new L2()
                                                            {
                                                                l2_id = x.product_category_l2_id,
                                                                l2_name = x.l2_name
                                                            }).ToList();
                    var l2_data = item.l2_list.Where(x => x.l2_id == l2_id).FirstOrDefault();
                    if (l2_data != null) {
                        ViewBag.l2_name = l2_data.l2_name;
                    }
                }
                info.p_data = db0.Product.Where(x => !x.i_Hide & x.l2_id == l2_id).OrderByDescending(x => x.sort).ToList();
                foreach (var item in info.p_data)
                {
                    item.imgsrc = GetImg(item.product_id, "Photo1", "ProductData", "Photo");//顯示列表圖
                    item.imgsrcs = GetImgs(item.product_id, "Photo2", "ProductData", "Photo");//顯示內頁圖
                }
            }
            ViewBag.l2_id = l2_id;
            return View("Products_content", info);
        }
    }
}
