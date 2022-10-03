using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SaTeknopark_MVC5.Models;
using System.Web.Mvc;
using System.IO;
using SaTeknopark_MVC5.UETDS;
using System.Net;
using System.Data;

namespace SaTeknopark_MVC5.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            //UdhbUetdsEsyaWsService ds = new UdhbUetdsEsyaWsService();
            //ds.Credentials = new NetworkCredential("999999", "999999testtest");
            //ds.PreAuthenticate = true;

            //uetdsYtsUser user = new uetdsYtsUser(); 
            //user.kullaniciAdi = "999999"; 
            //user.sifre = "999999testtest"; 

            //string kimlikNo = "11122233344";

            //uetdsMesSorguSonuc sonuc=   ds.meslekiYeterlilikSorgula(user, kimlikNo);

            //List<string> list = sonuc.belgeListesi.ToList();

            //ViewBag.Liste = list;
            return View();
        }

        public ActionResult YeniYuk(int id=0)
        {
            if (id == 0)
            {
                return View(new YUK());
            }
            else
            {
                using (sayazilimEntities db = new sayazilimEntities())
                {
                    var servisdetaylari = db.YUK_DETAY.Where(x => x.SeferID == id).ToList<YUK_DETAY>();
                    ViewBag.Detay = servisdetaylari.ToList();

                    return View(db.YUK.Where(x => x.ID == id).FirstOrDefault<YUK>());
                }

            }
        
        }

        public ActionResult YukListele()
        {
            return View();
        }


        public ActionResult GetSefer(int id)
        {
            string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

            List<YUK_DETAY> yonetim = new List<YUK_DETAY>();
            YUK yt = new YUK();
            string sorg = @"select * from YUK WHERE ID='" + id + "'";
            string sorgdetay = @"select * from YUK_DETAY WHERE SeferID='"+id+"'";
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();
                    using (SqlCommand servisgetir = new SqlCommand(sorg, con))
                    {
                        using (SqlDataReader dr = servisgetir.ExecuteReader())
                        {
                            while (dr.Read())
                            {


                                //System.IO.File.WriteAllText(Path.Combine(@"C:\", "sonuç.xml"), yt.AracCinsi.ToString());

                                yt.ID = Convert.ToInt32(dr["ID"]);
                                yt.Plaka1 = dr["Plaka1"].ToString();
                                yt.Sofor1TcNo = dr["Sofor1TcNo"].ToString();
                                yt.FirmaSeferNo = dr["FirmaSeferNo"].ToString();
                                yt.BaslangicUlkeKodu = dr["BaslangicUlkeKodu"].ToString();
                                yt.BaslangicIlMernisKod = Convert.ToInt32(dr["BaslangicIlMernisKod"]);
                                yt.BaslangicIlceMernisK = Convert.ToInt32(dr["BaslangicIlceMernisK"]);

                                yt.BitisUlkeKodu = dr["BitisUlkeKodu"].ToString();
                                yt.BitisIlMernisKodu = Convert.ToInt32(dr["BitisIlMernisKodu"]);
                                yt.BitisIlceMernisKodu = Convert.ToInt32(dr["BitisIlceMernisKodu"]);

                                yt.SeferBaslangicTarihi = dr["SeferBaslangicTarihi"].ToString();
                                yt.SeferBaslangicSaati = dr["SeferBaslangicSaati"].ToString();

                                yt.SeferBitisTarihi = dr["SeferBitisTarihi"].ToString();
                                yt.SeferBitisSaati = dr["SeferBitisSaati"].ToString();

                                yt.SeferTasimaBedeli = dr["SeferTasimaBedeli"].ToString();

                                yt.SeferTasimaBedeliParaBirimi = dr["SeferTasimaBedeliParaBirimi"].ToString();



                            }
                        }
                    }
                }


                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();
                    using (SqlCommand servisgetir = new SqlCommand(sorg, con))
                    {
                        using (SqlDataReader dr = servisgetir.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                YUK_DETAY ytd = new YUK_DETAY(); 

                                ytd.ID = Convert.ToInt32(dr["ID"]);
                               

                                yonetim.Add(ytd);

                            }
                        }
                    }
                }



            }
            catch (Exception e1)
            {
                System.IO.File.WriteAllText(Path.Combine(@"C:\Users\ilhan\AppData\Local\Sayazilim", "sonuç.xml"), e1.ToString() + Environment.NewLine);
            }



            return Json(new { data = yt, detay = yonetim.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetYuk()
        {
            string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

            List<YUK> yonetim = new List<YUK>();
            string sorg = @"select * from YUK";

            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    con.Open();
                    using (SqlCommand servisgetir = new SqlCommand(sorg, con))
                    {
                        using (SqlDataReader dr = servisgetir.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                YUK yt = new YUK();

                                //System.IO.File.WriteAllText(Path.Combine(@"C:\", "sonuç.xml"), yt.AracCinsi.ToString());

                                yt.ID = Convert.ToInt32(dr["ID"]);
                                yt.Plaka1 = dr["Plaka1"].ToString();
                                yt.Plaka2 = dr["Plaka2"].ToString();
                                yt.Sofor1TcNo = dr["Sofor1TcNo"].ToString();
                                yt.Sofor2TcNo = dr["Sofor2TcNo"].ToString();
                                yt.FirmaSeferNo = dr["FirmaSeferNo"].ToString();
                                yt.SeferEklemeTarihi= dr["SeferEklemeTarihi"].ToString();
                                yt.BaslangicUlkeKodu = dr["BaslangicUlkeKodu"].ToString();
                                yt.BaslangicIlMernisKod = Convert.ToInt32(dr["BaslangicIlMernisKod"]);
                                yt.BaslangicIlceMernisK = Convert.ToInt32(dr["BaslangicIlceMernisK"]);
                                yt.BitisUlkeKodu = dr["BitisUlkeKodu"].ToString();
                                yt.BitisIlMernisKodu = Convert.ToInt32(dr["BitisIlMernisKodu"]);
                                yt.BitisIlceMernisKodu = Convert.ToInt32(dr["BitisIlceMernisKodu"]);
                                yt.SeferBaslangicTarihi =Convert.ToDateTime( dr["SeferBaslangicTarihi"]).ToString("dd.MM.yyyy");
                                yt.SeferBaslangicSaati = dr["SeferBaslangicSaati"].ToString();
                                yt.SeferBitisTarihi = Convert.ToDateTime(dr["SeferBitisTarihi"]).ToString("dd.MM.yyyy");
                                yt.SeferBitisSaati = dr["SeferBitisSaati"].ToString();
                                yt.SeferTasimaBedeli = dr["SeferTasimaBedeli"].ToString();
                                yt.SeferTasimaBedeliParaBirimi = dr["SeferTasimaBedeliParaBirimi"].ToString();
                                yt.Durum = dr["Durum"].ToString();

                                ViewBag.Sofor= dr["Plaka1"].ToString();

                                yonetim.Add(yt);

                            }
                        }
                    }
                }
            }
            catch (Exception e1)
            {
                System.IO.File.WriteAllText(Path.Combine(@"C:\Users\Hatice\AppData\Local\Sayazilim", "sonuç.xml"), e1.ToString() + Environment.NewLine);
             }

          

            return Json(new { data = yonetim.Distinct() }, JsonRequestBehavior.AllowGet);
        }

        public partial class YukEkAlan
        {
          
            public string BasUlkeAdi { get; set; }
            public string BitisUlkeAdi { get; set; }
            public string BasIlAdi { get; set; }
            public string BitisIlAdi { get; set; }
            public string BasIlceAdi { get; set; }
            public string BitisIlceAdi { get; set; }
            public string SoforAdi { get; set; }

            public string GonderenUlkeAdi { get; set; }
            public string AliciUlkeAdi { get; set; }
            public string GonderenIlAdi { get; set; }
            public string AliciIlAdi { get; set; }
         
        }

        public partial class YukEkAlanDetay
        {

            public string BasUlkeAdi { get; set; }
            public string BitisUlkeAdi { get; set; }
            public string BasIlAdi { get; set; }
            public string BitisIlAdi { get; set; }
            public string BasIlceAdi { get; set; }
            public string BitisIlceAdi { get; set; }
            public string SoforAdi { get; set; }

            public string YuklemeUlkeAdi { get; set; }
            public string AliciUlkeAdi { get; set; }
            public string GonderenIlAdi { get; set; }
            public string AliciIlAdi { get; set; }

        }


        public ActionResult YukBilgi(int id )
        {

            using (sayazilimEntities db = new sayazilimEntities())
            {
              
                YUK emp = db.YUK.Where(x => x.ID == id).FirstOrDefault<YUK>();
                //ViewBag.Sofor = emp.Sofor1TcNo;
                //ViewBag.Plaka = emp.Plaka1;

                YukEkAlan ek = new YukEkAlan();

                ek.BasUlkeAdi = AyarMetot.UlkelerList().Where(x => x.Value == emp.BaslangicUlkeKodu).FirstOrDefault<SelectListItem>().Text;
                ek.BitisUlkeAdi = AyarMetot.UlkelerList().Where(x => x.Value == emp.BitisUlkeKodu).FirstOrDefault<SelectListItem>().Text;
                if (emp.BaslangicIlMernisKod ==0) {
                    ek.BasIlAdi = " ";
                }
                else
                {
                    ek.BasIlAdi = AyarMetot.IlcelerList().Where(x => x.Value == emp.BaslangicIlMernisKod.ToString()).FirstOrDefault<SelectListItem>().Text;
                }

                if (emp.BitisIlMernisKodu==0)
                {
                    ek.BitisIlAdi = " ";
                }
                else {
                    ek.BitisIlAdi = AyarMetot.IlcelerList().Where(x => x.Value == emp.BitisIlMernisKodu.ToString()).FirstOrDefault<SelectListItem>().Text;
                }


                var servisdetaylari = db.YUK_DETAY.Where(x => x.SeferID == id).ToList<YUK_DETAY>();

                List<YukEkAlanDetay> dr = new List<YukEkAlanDetay>();

                for (int i = 0; i < servisdetaylari.Count; i++)
                {
                    YukEkAlanDetay st = new YukEkAlanDetay();
                    YUK_DETAY str = servisdetaylari[i];
                    st.YuklemeUlkeAdi = AyarMetot.UlkelerList().Where(x => x.Value == str.YuklemeUlkeKodu.ToString()).FirstOrDefault<SelectListItem>().Text;
                    dr.Add(st);
                }

                return Json(new { success = true, data = emp , yt=ek,st=servisdetaylari.ToList()  , yd = dr.ToList() }, JsonRequestBehavior.AllowGet);
            }
         
      

    }


        [HttpPost]
        public ActionResult GuncelleSefer(int id, string durum)
        {
            using (sayazilimEntities db = new sayazilimEntities())
            {
                YUK emp = db.YUK.Where(x => x.ID == id).FirstOrDefault<YUK>();
                if (durum == "Iptal") durum = "İptal Edildi";
                emp.Durum = durum;
                db.SaveChanges();
                return Json(new { success = true, message = "Durum Güncellendi" }, JsonRequestBehavior.AllowGet);
            }
        }

        sayazilimEntities db = new sayazilimEntities();
        [HttpPost]
        public JsonResult yeniYuk(YUK cr, Array [] data/*, bool Gonder*/)
        {
            //Gonder = false;
            //UdhbUetdsEsyaWsService ds = new UdhbUetdsEsyaWsService();
            //ds.Credentials = new NetworkCredential("999999", "999999testtest");
            //ds.PreAuthenticate = true;

            //uetdsYtsUser YT = new uetdsYtsUser();  //E-devletten alınan kısım
            //YT.kullaniciAdi = "999999";
            //YT.sifre = "999999testtest";

            
            //List<wsUetdsEsyaIPListesi> st = new List<wsUetdsEsyaIPListesi>();
            //List<wsUetdsEsyaYukBilgileriInputV2> list = new List<wsUetdsEsyaYukBilgileriInputV2>();
            //wsUetdsEsyaSeferBilgileriInput sb = new wsUetdsEsyaSeferBilgileriInput();



            YUK arac = null;
            string Message = "Kayıt Eklendi";
            var result = new { sonuc = 0, Message = "" };
            if (cr.ID == -1)
            {
                arac = new YUK();
                arac = cr;

                if (arac.BaslangicIlMernisKod == null)
                {
                    arac.BaslangicIlMernisKod = 0;
                    arac.BaslangicIlceMernisK = 0;
                }
                if (arac.BitisIlMernisKodu == null)
                {
                    arac.BitisIlMernisKodu = 0;
                    arac.BitisIlceMernisKodu = 0;
                }
                
                arac.SeferBaslangicSaati = Convert.ToDateTime(cr.SeferBitisTarihi).ToString("HH:mm");
                string YukTurID=" ";
                string YukTurAdi = " ";
                string YukMiktari = " ";
                string TasimaBedeli = " ";
                string YukBirimi = "YukBirimi";

                string GonderenVergiNo = " ";
                string GonderenUnvan = " ";
                string YuklemeUlkeKodu = " ";

                string YuklemeIlMernisKodu = " ";
                int YuklemeILceMernisKodu = 0;
                string YuklemeTarihi = " ";
                string AliciVergiNo = " ";

                string AliciUnvan = " ";
                string BosaltmaUlkeKodu =" ";
                string BosaltmaIlMernisKodu = " ";
                int YukCinsID = 0;
                string BosaltmaTarihi = " ";

                db.YUK.Add(arac);
                db.SaveChanges();


                int EmpID = -1;
                try
                {
                    using (SqlConnection conp = new SqlConnection(AyarMetot.strcon))
                    {
                        if (conp.State == ConnectionState.Closed) conp.Open();
                        using (SqlCommand command = new SqlCommand("SELECT ID From yuk_DETAY order by ID desc", conp))
                        {

                            EmpID = Convert.ToInt32(command.ExecuteScalar());
                        }
                    }
                }
                catch { }

                //if (Gonder)
                //{

                //   sb = new wsUetdsEsyaSeferBilgileriInput()
                //    {
                //        baslangicIlMernisKodu = "6",
                //        baslangicIlceMernisKodu = "2034",
                //        bitisIlMernisKodu = "6",
                //        bitisIlceMernisKodu = "2034",
                //        baslangicUlkeKodu = "TR",
                //        bitisUlkeKodu = "TR",
                //        firmaSeferNo = "",
                //        plaka1 = "06TEST123",
                //        plaka2 = "",
                //        seferBaslangicSaati = "10:00",
                //        seferBaslangicTarihi = "13/10/2020",
                //        seferBitisSaati = "15:30",
                //        seferBitisTarihi = "14/10/2020",
                //        seferTasimaBedeli = "2000",
                //        seferTasimaBedeliParaBirimi = "TL",
                //        sofor1TcNo = "11111111111",
                //        sofor2TcNo = "",

                //    };
                //}


                int kolon = 0;
                for (int i = 0; i < data.Length ; i++)
                {
                  
                    foreach (var veri in data[i])
                    {
                        if (kolon == 0)
                        {

                            YukTurID = (veri).ToString();
                        }
                        else if (kolon == 1)
                        {

                            YukTurAdi = veri.ToString();
                        }
                        else if (kolon == 2)
                        {

                            YukMiktari = veri.ToString();
                        }
                        else if (kolon == 3)
                        {
                            TasimaBedeli = (veri).ToString();
                        }
                        else if (kolon == 4)
                        {
                            YukBirimi = (veri).ToString();
                        }


                       else if (kolon == 5)
                        {

                            GonderenVergiNo = (veri).ToString();
                        }
                        else if (kolon == 6)
                        {

                            GonderenUnvan = veri.ToString();
                        }
                        else if (kolon == 7)
                        {
                            YuklemeUlkeKodu = (veri).ToString();
                        }
                        else if (kolon == 8)
                        {
                            if (veri.ToString() != "")
                            {
                                YuklemeIlMernisKodu = (veri).ToString();
                            }
                            else
                            {
                                YuklemeIlMernisKodu = "0";
                            }
                        }
                        else if (kolon == 9)
                        {

                            YuklemeTarihi = (veri).ToString();
                        }
                        else if (kolon == 10)
                        {

                            AliciVergiNo = veri.ToString();
                        }
                        else if (kolon == 11)
                        {
                            AliciUnvan = (veri).ToString();
                        }
                        else if (kolon == 12)
                        {
                            BosaltmaUlkeKodu = (veri).ToString();
                        }
                        else if (kolon == 13)
                        {
                            if (veri.ToString().Replace(" ",String.Empty) != "")
                            {
                                BosaltmaIlMernisKodu = veri.ToString();
                            }
                            else
                            {
                                BosaltmaIlMernisKodu = "0";
                            }
                       
                        }
                        else if (kolon == 14)
                        {
                            BosaltmaTarihi = (veri).ToString();
                        }
                        kolon++;
                    }
                    kolon = 0;



                    //if (Gonder)
                    //{
                    //    wsUetdsEsyaYukBilgileriInputV2 yuk1 = new wsUetdsEsyaYukBilgileriInputV2()
                    //    {
                    //        aliciUnvan = "ALICI ÜNVAN",
                    //        bosaltmaSaati = "10:00",
                    //        aliciVergiNo = "1",
                    //        bosaltmaIlceMernisKodu = "2034",
                    //        bosaltmaIlMernisKodu = "6",
                    //        bosaltmaTarihi = "14/10/2020",
                    //        bosaltmaUlkeKodu = "TR",
                    //        firmaYukNo = "",
                    //        gonderenVergiNo = "8460056835",
                    //        gonderenUnvan = "GÖNDEREN ÜNVAN",
                    //        tasimaBedeli = "2000",
                    //        tasimaBedeliParaBirimi = "TL",
                    //        yukBirimi = "KG",
                    //        yukCinsId = "205",
                    //        yukDigerAciklama = "Hammadde / Maden ürünleri / İnşaat malzemesi",
                    //        yuklemeIlceMernisKodu = "2034",
                    //        yuklemeIlMernisKodu = "6",
                    //        yuklemeSaati = "15:30",
                    //        yuklemeTarihi = "13/10/2020",
                    //        yuklemeUlkeKodu = "TR",
                    //        yukMiktari = "250",


                    //    };

                    //    list.Add(yuk1);

                    //}


                    try
                    {
                        string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(strcon))
                        {
                            if (con.State == ConnectionState.Closed) con.Open();
                            using (SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("select * from YUK_DETAY", con))
                            {
                                using (SqlCommandBuilder cb = new SqlCommandBuilder(da))
                                {
                                    DataSet ds1 = new DataSet();
                                    da.Fill(ds1, "YUK_DETAY");
                                    DataRow df = ds1.Tables["YUK_DETAY"].NewRow();

                                    df["SeferID"] = EmpID.ToString();
                                    df["YukTurID"] = YukTurID.ToString();
                                    df["YukMiktari"] = YukMiktari;
                                    df["TasimaBedeli"] = TasimaBedeli;
                                    df["YukBirimi"] = YukBirimi;
                                    df["GonderenVergiNo"] = GonderenVergiNo;

                                    df["GonderenUnvan"] = GonderenUnvan;
                                    df["YuklemeUlkeKodu"] = YuklemeUlkeKodu;
                                    df["YuklemeIlMernisKodu"] = YuklemeIlMernisKodu;
                                    df["YuklemeIlceMernisKodu"] = YuklemeIlMernisKodu;
                                    df["YuklemeTarihi"] =Convert.ToDateTime( YuklemeTarihi);
                                    df["AliciVergiNo"] = AliciVergiNo;

                                    df["AliciUnvan"] = AliciUnvan;
                                    df["BosaltmaUlkeKodu"] = BosaltmaUlkeKodu;
                                    df["BosaltmaIlMernisKodu"] = BosaltmaIlMernisKodu;
                                    df["BosaltmaIlceMernisKodu"] = BosaltmaIlMernisKodu;
                                    df["BosaltmaTarihi"] = BosaltmaTarihi;
                                    df["YukTurAdi"] = AyarMetot.YukTuru().Where(x => x.Value == YukTurID).FirstOrDefault<SelectListItem>().Text; ;


                                    ds1.Tables["YUK_DETAY"].Rows.Add(df);
                                    da.Update(ds1, "YUK_DETAY");
                                }
                            }
                        }

                    }
                    catch (Exception E1)
                    {
                        System.IO.File.WriteAllText(Path.Combine(@"C:\Users\Hatice\AppData\Local\Sayazilim", "sonuç.xml"), E1.ToString());
                    }
                    
                    
                   

                       //
                    
                }
                
         



                //if (Gonder)
                //{
                //    List<uetdsEsyaSonucV2> SONUC = new List<uetdsEsyaSonucV2>();


                //    uetdsEsyaYeniYukKaydiBildirSonucV2 sn1 = ds.yeniYukKaydiBildirV2(YT, sb, list.ToArray());
                //    SONUC = sn1.uetdsEsyaSonuc.ToList();



                //    //textBox1.Text = "sonucKodu:" + sn1.sonucKodu.ToString() +
                //    //    Environment.NewLine +
                //    //    "ReferansNo:" + sn1.uetdsSeferReferansNo +
                //    //    Environment.NewLine +
                //    //    "uetdsSeferReferansNo:" + SONUC[0].sonucMesaji.ToString();
                //}

            }
            else
            {
                arac = db.YUK.Where(x => x.ID == cr.ID).FirstOrDefault<YUK>();

                arac.ID = cr.ID;
                arac.BaslangicIlceMernisK = cr.BaslangicIlMernisKod;
                arac.BaslangicIlMernisKod = cr.BaslangicIlMernisKod;
                arac.BitisIlMernisKodu = cr.BitisIlMernisKodu;
                arac.BitisIlceMernisKodu = cr.BitisIlMernisKodu;
                arac.BaslangicUlkeKodu = cr.BaslangicUlkeKodu;
                arac.BitisUlkeKodu = cr.BitisUlkeKodu;
                arac.FirmaSeferNo = cr.FirmaSeferNo;
                arac.Plaka1 = cr.Plaka1;
                arac.Plaka2 = cr.Plaka2;
                arac.SeferBaslangicSaati = cr.SeferBaslangicSaati;
                arac.SeferBaslangicTarihi = cr.SeferBaslangicTarihi;
                arac.SeferBitisSaati = cr.SeferBitisSaati;
                arac.SeferBitisTarihi = cr.SeferBitisTarihi;
                arac.SeferEklemeTarihi = cr.SeferEklemeTarihi;
                arac.SeferTasimaBedeli = cr.SeferTasimaBedeli;
                arac.SeferTasimaBedeliParaBirimi = cr.SeferTasimaBedeliParaBirimi;
                arac.Sofor1TcNo = cr.Sofor1TcNo;
                arac.Sofor2TcNo = cr.Sofor2TcNo;


                db.SaveChanges();
                Message = "Kayıt Güncellendi";
            }

            result = new { sonuc = 1, Message = Message };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult DeleteYuk(int id)
        {
            using (sayazilimEntities db = new sayazilimEntities())
            {
                YUK emp = db.YUK.Where(x => x.ID == id).FirstOrDefault<YUK>();
                db.YUK.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Kayıt Silindi" }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}