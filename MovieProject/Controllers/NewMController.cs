using Models.Dao;
using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieProject.Controllers
{
    public class NewMController : Controller
    {
        MovieProjectDbcontext db = new MovieProjectDbcontext();
        // GET: New
        public ActionResult Index()
        {
            var dao = new NewDao();
            ViewBag.news = dao.ListNews(9);
            ViewBag.NewsNew = dao.ListNewsNew(5);
            ViewBag.NewsTop = dao.ListNewsTop(6);
            return View();
        }
        [ChildActionOnly]

        public PartialViewResult Country()
        {
            var model = new CountryDao().ListAll();
            return PartialView(model);

        }

        [ChildActionOnly]

        public PartialViewResult Season()
        {
            var model = new SeasonDao().ListAll();
            return PartialView(model);

        }

        public ActionResult CountryPage(long idc, int page = 1)
        {
            var movieDao = new MovieDao();
            var country = new CountryDao().ViewDetail(idc);
            ViewBag.country = country;
            ViewBag.ListMovieNew = movieDao.ListMovieNew(12);
            var model = movieDao.ListByCountryID(idc);
            return View(model.ToPagedList(page, 12));

        }

        public ActionResult SeasonPage(long idc, int page = 1)
        {
            var newDao = new NewDao();
            var season = new SeasonDao().ViewDetail(idc);
            ViewBag.season = season;
            ViewBag.listnewsnew = newDao.ListNewsNew(12);
            var model = newDao.ListBySeasonID(idc);
            return View(model.ToPagedList(page, 12));

        }

        public ActionResult NewDetail(int id)
        {
            var dao = new NewDao();
            ViewBag.news = dao.ViewDetail(id);
            ViewBag.NewsNew = dao.ListNewsNew(5);
            ViewBag.NewsTop = dao.ListNewsTop(6);
            ViewBag.ListMovieNew1 = new MovieDao().ListMovieNew1(12);
            News upview = db.News.Find(id);
            if (upview.Viewed == null)
            {
                upview.Viewed = 1;
                upview.Name = upview.Name;
                upview.Image = upview.Image;
                upview.Description = upview.Description;
                upview.CreatedDate = upview.CreatedDate;
                upview.Status = upview.Status;
                db.Entry(upview).State = EntityState.Modified;
                db.SaveChanges();
                return View(upview);
            }
            else
            {
                upview.Viewed = upview.Viewed + 1;
                upview.Name = upview.Name;
                upview.Image = upview.Image;
                upview.Description = upview.Description;
                upview.CreatedDate = upview.CreatedDate;
                upview.Status = upview.Status;
                db.Entry(upview).State = EntityState.Modified;
                db.SaveChanges();
                return View(upview);
            }
           
        }
    }
}