using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaTeknopark_MVC5.Models;

namespace SaTeknopark_MVC5.Controllers
{
    public class PersonelController : Controller
    {
        // GET: Personel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Personeller()
        {
            return View();
        }




        public ActionResult GetPersonel()
        {
            string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

            List<Personel> yonetim = new List<Personel>();
            string sorg = @"select ID,Adi,Soyadi,PersonelGrubu,Adres,EkBilgiler,Milleti,TCNo,Cinsiyet,Telefon1 from Personel where PersonelGrubu!='admin'";



            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                using (SqlCommand servisgetir = new SqlCommand(sorg, con))
                {
                    using (SqlDataReader dr = servisgetir.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Personel yt = new Personel();

                            yt.ID =Convert.ToInt32( dr["ID"]);
                            yt.Adi = dr["Adi"].ToString();
                            yt.Soyadi = dr["Soyadi"].ToString();
                            yt.PersonelGrubu = dr["PersonelGrubu"].ToString();
                            yt.Milleti = dr["Milleti"].ToString();
                            yt.TCNo = dr["TCNo"].ToString();
                            yt.Cinsiyet = dr["Cinsiyet"].ToString();
                            yt.Telefon1 = Convert.ToString(dr["Telefon1"]);
                            yt.Adres= dr["Adres"].ToString();
                            yt.EkBilgiler= dr["EkBilgiler"].ToString();

                            yonetim.Add(yt);


                        }
                    }
                }
            }

            return Json(new { data = yonetim.Distinct() }, JsonRequestBehavior.AllowGet);
        }



        sayazilimEntities db = new sayazilimEntities();
        [HttpPost]
        public JsonResult yeniPersonel(Personel pr)
        {
            Personel per = null;
            string Message = "Kayıt Eklendi";
            if (pr.ID == -1)
            {
                per = new Personel();
                per = pr;



                    db.Personel.Add(per);
                    db.SaveChanges();
                
            }
            else
            {
                per = db.Personel.Where(x => x.ID == pr.ID).FirstOrDefault<Personel>();


                per.ID = pr.ID;
                per.Adi = pr.Adi;
                per.Soyadi = pr.Soyadi;
                per.PersonelGrubu = pr.PersonelGrubu;
                per.Milleti = pr.Milleti;
                per.TCNo = pr.TCNo;
                per.Cinsiyet = pr.Cinsiyet;
                per.Telefon1 = pr.Telefon1;
                per.Adres = pr.Adres;
                per.EkBilgiler = pr.EkBilgiler;

                db.SaveChanges();
                Message = "Kayıt Güncellendi";
            }

            var result = new { sonuc = 1, Message = Message };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult PersonelBilgi(int id)
        {
            using (sayazilimEntities db = new sayazilimEntities())
            {
                Personel emp = db.Personel.Where(x => x.ID == id).FirstOrDefault<Personel>();
                return Json(new { success = true, data = emp }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeletePersonel(int id)
        {
            using (sayazilimEntities db = new sayazilimEntities())
            {
                Personel emp = db.Personel.Where(x => x.ID == id).FirstOrDefault<Personel>();
                db.Personel.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Kayıt Silindi" }, JsonRequestBehavior.AllowGet);
            }
        }




    }
}