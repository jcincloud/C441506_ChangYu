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

        public async Task<IHttpActionResult> GetInsertRoles()
        {
            var system_roles = await roleManager.Roles.Where(x => x.Name != "Admins").ToListAsync();
            IList<RoleArray> obj = new List<RoleArray>();
            foreach (var role in system_roles)
            {
                obj.Add(new RoleArray() { role_id = role.Id, role_name = role.Name, role_use = false });
            }
            return Ok(obj);
        }

    }
    #region Parm

    #endregion
}
