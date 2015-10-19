using DotWeb.Helpers;
using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotWeb.Api
{
    public class ProductController : ajaxApi<Product, q_Product>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.Product.FindAsync(id);
                r = new ResultInfo<Product>() { data = item };
            }

            return Ok(r);
        }
        public async Task<IHttpActionResult> Get([FromUri]q_Product q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var qr = db0.Product
                    .OrderByDescending(x => new { x.Product_Category_L1.l1_sort, x.Product_Category_L2.l2_sort, x.sort }).AsQueryable();

                if (q.name != null)
                {
                    qr = qr.Where(x => x.product_name.Contains(q.name));
                }
                if (q.i_Hide != null)
                {
                    qr = qr.Where(x => x.i_Hide == q.i_Hide);
                }
                if (q.l1_id != null)
                {
                    qr = qr.Where(x => x.l1_id == q.l1_id);
                }
                if (q.l2_id != null)
                {
                    qr = qr.Where(x => x.l2_id == q.l2_id);
                }
                var result = qr.Select(x => new m_Product()
                {
                    product_id = x.product_id,
                    l1_id = x.l1_id,
                    l2_id = x.l2_id,
                    l1_name = x.Product_Category_L1.l1_name,
                    l2_name = x.Product_Category_L2.l2_name,
                    product_name = x.product_name,
                    sort=x.sort,
                    i_Hide = x.i_Hide
                });

                int page = (q.page == null ? 1 : (int)q.page);
                int position = PageCount.PageInfo(page, this.defPageSize, qr.Count());
                var segment = await result.Skip(position).Take(this.defPageSize).ToListAsync();

                return Ok<GridInfo<m_Product>>(new GridInfo<m_Product>()
                {
                    rows = segment,
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                });
            }
            #endregion
        }
        public async Task<IHttpActionResult> Put([FromBody]Product md)
        {
            ResultInfo r = new ResultInfo();

            try
            {
                db0 = getDB0();

                item = await db0.Product.FindAsync(md.product_id);
                item.l1_id = md.l1_id;
                item.l2_id = md.l2_id;
                item.product_name = md.product_name;
                item.product_content = md.product_content;
                item.sort = md.sort;
                item.i_Hide = md.i_Hide;

                item.i_UpdateUserID = this.UserId;
                item.i_UpdateDateTime = DateTime.Now;
                item.i_UpdateDeptID = this.departmentId;

                await db0.SaveChangesAsync();
                r.result = true;
                r.id = md.product_id;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.ToString();
            }
            finally
            {
                db0.Dispose();
            }
            return Ok(r);
        }
        public async Task<IHttpActionResult> Post([FromBody]Product md)
        {
            ResultInfo r = new ResultInfo();

            md.product_id = GetNewId(ProcCore.Business.CodeTable.Product);

            if (!ModelState.IsValid)
            {
                r.message = ModelStateErrorPack();
                r.result = false;
                return Ok(r);
            }
            try
            {
                #region working a
                db0 = getDB0();


                md.i_InsertUserID = this.UserId;
                md.i_InsertDateTime = DateTime.Now;
                md.i_InsertDeptID = this.departmentId;
                md.i_Lang = "zh-TW";
                db0.Product.Add(md);
                await db0.SaveChangesAsync();

                r.result = true;
                r.id = md.product_id;
                return Ok(r);
                #endregion
            }
            catch (DbEntityValidationException ex)
            {
                r.result = false;

                foreach (var m in ex.EntityValidationErrors)
                {

                    foreach (var n in m.ValidationErrors)
                    {
                        r.message += "[" + n.PropertyName + ":" + n.ErrorMessage + "]";
                    }
                }
                return Ok(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return Ok(r);
            }
            finally
            {
                db0.Dispose();
                //tx.Dispose();
            }
        }
        public async Task<IHttpActionResult> Delete([FromUri]int[] ids)
        {
            ResultInfo r = new ResultInfo();
            try
            {
                db0 = getDB0();

                foreach (var id in ids)
                {
                    item = new Product() { product_id = id };
                    db0.Product.Attach(item);
                    db0.Product.Remove(item);
                }

                await db0.SaveChangesAsync();

                r.result = true;
                return Ok(r);
            }
            catch (DbUpdateException ex)
            {
                r.result = false;
                if (ex.InnerException != null)
                {
                    r.message = Resources.Res.Log_Err_Delete_DetailExist
                        + "\r\n" + getErrorMessage(ex);
                }
                else
                {
                    r.message = ex.Message;
                }
                return Ok(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return Ok(r);
            }
            finally
            {
                db0.Dispose();
            }
        }
    }
}
