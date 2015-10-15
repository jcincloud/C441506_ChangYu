using Newtonsoft.Json;
using ProcCore;
using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace DotWeb.Api
{
    public class GetActionController : BaseApiController
    {
        
        //public async Task<IHttpActionResult> GetCustomerVisit([FromUri]ParmGetCustomerVisit parm)
        //{
        //    db0 = getDB0();
        //    try
        //    {
        //        int page_size = 10;

        //        var items = from x in db0.VisitDetail
        //                    orderby x.start_time descending, x.customer_id
        //                    select (new CustomerVisit()
        //                    {
        //                        customer_name = x.Customer.customer_name,
        //                        visit_date = x.Visit.visit_date,
        //                        state = x.state,
        //                        visit_start = x.start_time,
        //                        visit_end = x.end_time,
        //                        cumulative_time = x.cumulative_time,
        //                        users_id = x.users_id,
        //                        user_name = ""
        //                    });
        //        #region 驗證業務端只能看到自己的資料
        //        var getRoles = db0.AspNetUsers.FirstOrDefault(x => x.Id == this.UserId).AspNetRoles.Select(x => x.Name);

        //        if (!getRoles.Contains("Admins") & !getRoles.Contains("Managers"))
        //        {
        //            items = items.Where(x => x.users_id == this.UserId);
        //        }
        //        #endregion
        //        if (parm.start_date != null && parm.end_date != null)
        //        {
        //            DateTime end = ((DateTime)parm.end_date).AddDays(1);
        //            items = items.Where(x => x.visit_date >= parm.start_date && x.visit_date < end);
        //        }
        //        if (parm.users_id != null)
        //        {
        //            items = items.Where(x => x.users_id == parm.users_id);
        //        }
        //        if (parm.customer_name != null)
        //        {
        //            items = items.Where(x => x.customer_name.Contains(parm.customer_name));
        //        }


        //        int page = (parm.page == 0 ? 1 : parm.page);
        //        int startRecord = PageCount.PageInfo(page, page_size, items.Count());
        //        var resultItems = await items.Skip(startRecord).Take(page_size).ToListAsync();

        //        foreach (var item in resultItems)
        //        {
        //            string User_Name = db0.AspNetUsers.FirstOrDefault(x => x.Id == item.users_id).user_name_c;
        //            item.user_name = User_Name;
        //        }


        //        return Ok(new
        //        {
        //            rows = resultItems,
        //            total = PageCount.TotalPage,
        //            page = PageCount.Page,
        //            records = PageCount.RecordCount,
        //            startcount = PageCount.StartCount,
        //            endcount = PageCount.EndCount
        //        });
        //    }
        //    finally
        //    {
        //        db0.Dispose();
        //    }
        //}

    }
    #region Parm

    #endregion
}
