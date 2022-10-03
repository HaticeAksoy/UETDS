using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaTeknopark_MVC5.Models;
using System.Data.SqlClient;

namespace SaTeknopark_MVC5.Controllers
{
    public class AracController : Controller
    {
        // GET: Arac
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Araclar()
        {
            return View();
        }


        public ActionResult GetAraclar()
        {
            string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

            List<ARAC_OTO> yonetim = new List<ARAC_OTO>();
            string sorg = @"select ID,AracCinsi,Plaka,Marka,AracKodu,Model,Yakit,SoforID from ARAC_OTO";



            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                using (SqlCommand servisgetir = new SqlCommand(sorg, con))
                {
                    using (SqlDataReader dr = servisgetir.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ARAC_OTO yt = new ARAC_OTO();

                            yt.ID = Convert.ToInt32(dr["ID"]);
                            yt.AracCinsi = dr["AracCinsi"].ToString();

                            yt.Plaka = dr["Plaka"].ToString();
                            yt.Marka = dr["Marka"].ToString();
                            yt.AracKodu = dr["AracKodu"].ToString();
                            yt.Model = dr["Model"].ToString();
                            yt.Yakit = dr["Yakit"].ToString();        
                            
                            if(yt.SoforID==0 || yt.SoforID == null)
                            {
                                yt.SoforID = 0;
                            }
                            else
                            {
                                yt.SoforID = Convert.ToInt32(dr["SoforID"]);
                            }
                          

                            yonetim.Add(yt);


                        }
                    }
                }
            }

            return Json(new { data = yonetim.Distinct() }, JsonRequestBehavior.AllowGet);
        }



        sayazilimEntities db = new sayazilimEntities();
        [HttpPost]
        public JsonResult yeniArac(ARAC_OTO cr)
        {
            ARAC_OTO arac = null;
            string Message = "Kayıt Eklendi";
            var result = new { sonuc = 0, Message = "" };
            if (cr.ID == -1)
            {
                arac = new ARAC_OTO();
                arac = cr;
               
                if(arac.SoforID==0 || arac.SoforID == null)
                {
                    arac.SoforID = 0;
                }

                
                if (cr.Plaka == "-1" || cr.Plaka == "0" || cr.Plaka == null)
                {
                    result = new { sonuc = 0, Message = "Plaka Giriniz" };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    db.ARAC_OTO.Add(arac);
                    db.SaveChanges();
                }
               
               
                

            }
            else
            {
                arac = db.ARAC_OTO.Where(x => x.ID == cr.ID).FirstOrDefault<ARAC_OTO>();

                arac.ID = cr.ID;
                arac.AracCinsi = cr.AracCinsi;
                if(cr.Plaka==null || cr.Plaka==" ")
                {
                    result = new { sonuc = 0, Message = "Plaka boş olamaz" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                arac.Plaka = cr.Plaka;
                arac.Marka = cr.Marka;
                arac.AracKodu = cr.AracKodu;
                arac.Model = cr.Model;
                arac.Yakit = cr.Yakit;
                arac.SoforID = cr.SoforID;
               
                db.SaveChanges();
                Message = "Kayıt Güncellendi";
            }

            result = new { sonuc = 1, Message = Message };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult AracBilgi(int id)
        {
            using (sayazilimEntities db = new sayazilimEntities())
            {
                ARAC_OTO emp = db.ARAC_OTO.Where(x => x.ID == id).FirstOrDefault<ARAC_OTO>();
                return Json(new { success = true, data = emp }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public ActionResult DeleteArac(int id)
        {
            using (sayazilimEntities db = new sayazilimEntities())
            {
                ARAC_OTO emp = db.ARAC_OTO.Where(x => x.ID == id).FirstOrDefault<ARAC_OTO>();
                db.ARAC_OTO.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Kayıt Silindi" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}