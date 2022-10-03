using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FastReport.Export.Pdf;
using System.Data;
using System.Data.SqlClient;
using FastReport;
using System.IO;

namespace SaTeknopark_MVC5.Controllers
{
    public class AdvController : Controller
    {
        public FileResult TahsilatYazdir(string ID = "")
        {
            FastReport.Utils.Config.WebMode = true;
            FastReport.Report report1 = new FastReport.Report();

            report1.Prepare(true);

            try
            {
                report1.Load(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Cash.frx"));
            }
            catch { }

            using (SqlConnection conp = new SqlConnection(AyarMetot.strcon))
            {
                if (conp.State == ConnectionState.Closed) conp.Open();
                using (SqlCommand getir = new SqlCommand("Select CariID  From CASH_PAY where ID='" + ID + "'", conp))
                {
                    using (SqlDataReader dr = getir.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                           

                                using (DataTable Teklifler = GetDataTable("set dateformat dmy;SELECT * ,(Select Adi+' '+Soyadi from Personel where ID=PersonelID) as PersonelAdi From CASH_PAY where ID = '" + ID + "'"))
                                {

                                    report1.RegisterData(Teklifler.DefaultView.ToTable(), "NakitHavale");
                                    report1.GetDataSource("NakitHavale").Enabled = true;


                                   

                                }

                                report1.RegisterData(AyarMetot.CariBil(Convert.ToInt32(dr["CariID"]), "Tümü", "", "Tümü", "Tümü"), "CariBilgileri");
                                report1.RegisterData(AyarMetot.FirmaBil(), "FirmaBilgileri");


                                report1.GetDataSource("CariBilgileri").Enabled = true;
                                report1.GetDataSource("FirmaBilgileri").Enabled = true;


                            
                        }
                    }
                }
            }


            using (FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport())
            {
               

                report1.Prepare(true);
             

                report1.Export(pdf, Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Cash.pdf"));


                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //report1.Save(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"),"b.frx"));
                report1.Dispose();
                byte[] dosya = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"),"Cash.pdf"));
                return File(dosya, "application/pdf");
            }

        }
        
        public FileResult IrsaliyeYazdir(string ID = "")
        {
            FastReport.Utils.Config.WebMode = true;
            FastReport.Report report1 = new FastReport.Report();

            report1.Prepare(true);
            DataTable dtDetay = new DataTable();

            report1.Load(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Irsaliye.frx"));
            using (SqlConnection bag = new SqlConnection(AyarMetot.strcon))
            {
                if (bag.State == ConnectionState.Closed) bag.Open();
                using (SqlCommand servisgetir = new SqlCommand("Select * From INVOICE where ID='" + ID + "'", bag))
                {
                    using (SqlDataReader dt = servisgetir.ExecuteReader())
                    {
                        while (dt.Read())
                        {
                            string KDVdh = "Dahil";

                            dtDetay = AyarMetot.FaturaDetayBil(ID, KDVdh, Convert.ToDecimal(dt["FKuru"]));

                            if (dt["KdvDH"].ToString() != "D") KDVdh = "Hariç";
                            report1.RegisterData(AyarMetot.FaturaBil(ID), "FaturaBilgileri");
                            report1.RegisterData(dtDetay, "UrunBilgileri");
                            report1.RegisterData(AyarMetot.CariBil(Convert.ToInt32(dt["CariID"]), "Tümü", "Tümü", "Tümü", "Tümü"), "CariBilgileri");
                            report1.RegisterData(AyarMetot.FirmaBil(), "FirmaBilgileri");

                            report1.GetDataSource("FaturaBilgileri").Enabled = true;
                            report1.GetDataSource("UrunBilgileri").Enabled = true;
                            report1.GetDataSource("CariBilgileri").Enabled = true;
                            report1.GetDataSource("FirmaBilgileri").Enabled = true;

                           

                        }
                    }
                }
            }






            using (FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport())
            {

                ReportPage page1 = report1.Pages[0] as ReportPage;
                page1.PaperHeight = page1.PaperHeight + (dtDetay.Rows.Count * ((float)5));

                report1.Prepare(true);
                
                report1.Export(pdf, Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Irsaliye.pdf"));

                dtDetay.Dispose();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //report1.Save(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"),"Irs.frx"));
               // page1.Dispose();
                report1.Dispose();
                byte[] dosya = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Irsaliye.pdf"));
                return File(dosya, "application/pdf");
            }

        }
        
        public FileResult IadeYazdir(string ID = "")
        {try
            {
                FastReport.Utils.Config.WebMode = true;
                FastReport.Report report1 = new FastReport.Report();

                report1.Prepare(true);
                DataTable dtDetay = new DataTable();

                report1.Load(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Iade.frx"));
                using (SqlConnection bag = new SqlConnection(AyarMetot.strcon))
                {
                    if (bag.State == ConnectionState.Closed) bag.Open();
                    using (SqlCommand servisgetir = new SqlCommand("Select * From INVOICE where ID='" + ID + "'", bag))
                    {
                        using (SqlDataReader dt = servisgetir.ExecuteReader())
                        {
                            while (dt.Read())
                            {
                                string KDVdh = "Dahil";

                                dtDetay = AyarMetot.FaturaDetayBil(ID, KDVdh, Convert.ToDecimal(dt["FKuru"]));

                                if (dt["KdvDH"].ToString() != "D") KDVdh = "Hariç";
                                report1.RegisterData(AyarMetot.FaturaBil(ID), "FaturaBilgileri");
                                report1.RegisterData(dtDetay, "UrunBilgileri");
                                report1.RegisterData(AyarMetot.CariBil(Convert.ToInt32(dt["CariID"]), "Tümü", "Tümü", "Tümü", "Tümü"), "CariBilgileri");
                                report1.RegisterData(AyarMetot.FirmaBil(), "FirmaBilgileri");

                                report1.GetDataSource("FaturaBilgileri").Enabled = true;
                                report1.GetDataSource("UrunBilgileri").Enabled = true;
                                report1.GetDataSource("CariBilgileri").Enabled = true;
                                report1.GetDataSource("FirmaBilgileri").Enabled = true;



                            }
                        }
                    }
                }






                using (FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport())
                {

                    ReportPage page1 = report1.Pages[0] as ReportPage;
                    page1.PaperHeight = page1.PaperHeight + (dtDetay.Rows.Count * ((float)5));

                    report1.Prepare(true);

                    report1.Export(pdf, Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Iade.pdf"));

                    dtDetay.Dispose();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    //report1.Save(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"),"Irs.frx"));
                    // page1.Dispose();
                    report1.Dispose();
                    byte[] dosya = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "Iade.pdf"));
                    return File(dosya, "application/pdf");
                }
            }
            catch (Exception e1)
            {
                System.IO.File.WriteAllText(Path.Combine(@"C:\Users\ilhan\AppData\Local\Sayazilim", "sonuç.xml"), e1.ToString());
                byte[] dosya = null;
                return File(dosya, "application/pdf");
            }
        }



        public FileResult DepoIslemYazdir(string ID = "")
        {
            FastReport.Utils.Config.WebMode = true;
            FastReport.Report report1 = new FastReport.Report();

            report1.Prepare(true);
            DataTable dtDetay = new DataTable();

            try
            {
                report1.Load(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "depo.frx"));
            }
            catch { }

            using (SqlConnection bag = new SqlConnection(AyarMetot.strcon))
            {
                if (bag.State == ConnectionState.Closed) bag.Open();
                using (SqlCommand servisgetir = new SqlCommand("Select * From STORE_PROCESS where ID='" + ID + "'", bag))
                {
                    using (SqlDataReader dt = servisgetir.ExecuteReader())
                    {
                        while (dt.Read())
                        {
                        

                            dtDetay = AyarMetot.DepoIslemDetayBil(ID);

                         
                            report1.RegisterData(AyarMetot.DepoIslemBil(ID), "Depo_İşlem");
                            report1.RegisterData(dtDetay, "Depo_İşlem_Detay");
                            report1.RegisterData(AyarMetot.FirmaBil(), "FirmaBilgileri");

                            report1.GetDataSource("Depo_İşlem").Enabled = true;
                            report1.GetDataSource("Depo_İşlem_Detay").Enabled = true;
                            report1.GetDataSource("FirmaBilgileri").Enabled = true;
                            
                        }
                    }
                }
            }
            
            using (FastReport.Export.Pdf.PDFExport pdf = new FastReport.Export.Pdf.PDFExport())
            {
                try
                {
                    ReportPage page1 = report1.Pages[0] as ReportPage;
                    page1.PaperHeight = page1.PaperHeight + (dtDetay.Rows.Count * ((float)6.5));
                }
                catch { }
           
                report1.Prepare(true);

                report1.Export(pdf, Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "depo.pdf"));

                dtDetay.Dispose();

                //System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //report1.Save(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "depoyeni.frx"));


                report1.Dispose();
                byte[] dosya = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/SUTHANEMBELGELER/ATTACHMENT"), "depo.pdf"));
                return File(dosya, "application/pdf");
            }

        }





        private DataTable GetDataTable(string cmd)
        {
            DataTable returnTable = new DataTable();
            try
            {
                SqlConnection ODBCConn = new SqlConnection(AyarMetot.strcon);
                SqlDataAdapter oda = new SqlDataAdapter(cmd, ODBCConn);
                oda.Fill(returnTable);

            }
            catch
            {
                
            }
            return returnTable;

        }


    }


}