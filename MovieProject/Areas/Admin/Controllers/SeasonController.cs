using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Models.EF;
using PagedList.Mvc;
using PagedList;
using Models.Dao;
using System.Web.ModelBinding;

namespace MovieProject.Areas.Admin.Controllers
{
    public class SeasonController : BaseController
    {
        // GET: Admin/Season
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        [HashCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int page = 1, int pageSize = 10)
        {


            var dao = new SeasonDao();
            var model = dao.ListPg(page, pageSize);
            return View(model);
        }

        public ActionResult getList(int id)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var hs = db.Seasons.SingleOrDefault(x => x.Season_id == id);
            var result = JsonConvert.SerializeObject(hs, Formatting.Indented, jss);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        [HashCredential(RoleID = "EDIT_USER")]
        [HashCredential(RoleID = "ADD_USER")]
        public ActionResult Add(Season model, string submit)
        {
            if (submit == "Thêm")
            {
                if (model != null)
                {
                    model.Name = model.Name.ToString();
                    model.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    model.Status = model.Status;
                    db.Seasons.Add(model);
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Thêm thể loại thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Cập Nhật")
            {
                if (model != null)
                {
                    var list = db.Seasons.SingleOrDefault(x => x.Season_id == model.Season_id);
                    list.Name = model.Name;
                    list.CreatedDate = model.CreatedDate.GetValueOrDefault(System.DateTime.Now);
                    list.Status = model.Status;
                    db.SaveChanges();
                    model = null;
                }
                SetAlert("Sửa thể loại thành công", "success");
                return RedirectToAction("Index");
            }
            else if (submit == "Tìm")
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    List<Season> list = GetData().Where(s => s.Name.Contains(model.Name)).
                        ToList();
                    return View("Index", list);
                }
                else
                {
                    List<Season> list = GetData();
                    return View("Index", list);
                }
            }
            else
            {
                List<Season> list = GetData().OrderBy(s => s.Name).ToList();
                return View("Index", list);
            }
        }
        public List<Season> GetData()
        {
            return db.Seasons.ToList();


        }
        public ActionResult Delete(int id)
        {
            new SeasonDao().Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var result = new SeasonDao().ChangeStatus(id);
            return Json(new
            {
                status = result

            });
        }



    }
}