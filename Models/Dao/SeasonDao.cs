using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class SeasonDao
    {
        MovieProjectDbcontext db = null;
        public SeasonDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Season> ListPg(int page, int pageSize)
        {
            return db.Seasons.OrderByDescending(x => x.Season_id).ToPagedList(page, pageSize);
        }
        public List<Season> ListAll()
        {
            return db.Seasons.Where(x => x.Status == true).ToList();
        }
        public Season ViewDetail(long id)
        {
            return db.Seasons.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Seasons.Find(id);
                db.Seasons.Remove(user);
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool ChangeStatus(int id)
        {
            var ad = db.Seasons.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}