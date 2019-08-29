using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureProject.Models;
using System.IO;
using System.Data;
using LinqToExcel;
using ClosedXML.Excel;
using Rotativa;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AzureProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.EmployeeList = "";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public int PostExcelData(int id, string d)
        {
            System.Diagnostics.Debug.WriteLine("ID: " + id + " , Expectation: " + d);
            return 0 ;
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase FileUpload)
        {
            string data = "";
            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;

                    if (filename.EndsWith(".xlsx"))
                    {
                        string targetpath = Server.MapPath("~/Content/");
                        FileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;

                        string sheetName = "Sheet1";

                        var excelFile = new ExcelQueryFactory(pathToExcelFile);
                        var empDetails = from a in excelFile.Worksheet<Employee>(sheetName) select a;
                        foreach (var a in empDetails)
                        {
                            if (a.Expectation != null)
                            {
                                // int resullt = PostExcelData(a.IdEmp, a.Expectation);
                                System.Diagnostics.Debug.WriteLine("ID: " + a.IdEmp + " , Expectation: " + a.Expectation);                       
                            }

                            else
                            {
                                data = a.IdEmp + "Some fields are null, Please check your excel sheet";
                                ViewBag.Message = data;
                                return RedirectToAction(data);
                            }

                        }
                    }

                    else
                    {
                        data = "This file is not valid format";
                        ViewBag.Message = data;
                    }
                   return Content(data +" 2nd");
                }
                else
                {

                    data = "Only Excel file format is allowed";

                    ViewBag.Message = data;
                    return Content("Only Excel file format is allowed");

                }

            }
            else
            {

                if (FileUpload == null)
                {
                    data = "Please choose Excel file";
                }

                ViewBag.Message = data;
               return Content("File Is not found");
            }
        }

        public FileResult DownloadExcel()
        {
            string path = "/File/SampleExcelFormat.xlsx";
            return File(path, "application/vnd.ms-excel", "ExcelFormat.xlsx");
        }

        public ActionResult Certificates()
        {
            lear dbconext = new lear();
            CertificatesViewModel model = new CertificatesViewModel();
            var certificates = dbconext.Certifications.ToList<Certification>();
            foreach(var cert in certificates)
            {
                System.Diagnostics.Debug.WriteLine(cert.Description);
                model.Certifications.Add(new Certification() { Id = cert.Id, Code = cert.Code , Description = cert.Description, Points= cert.Points });
               
             }
         
            return View(model);
        }

        [HttpPost]
        public FileResult ExportToExcel()
        {
            lear entities = new lear();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Id"),
                                            new DataColumn("Code"),
                                            new DataColumn("Description"),
                                            new DataColumn("Points") });

            var certificates = from Certification in entities.Certifications
                            select Certification;

            foreach (var certificate in certificates)
            {
                dt.Rows.Add(certificate.Id, certificate.Code, certificate.Description, certificate.Points);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Certificates.xlsx");
                }
            }
        }
        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("Certificates");
            return report;
        }

        public ActionResult PrintPartialViewToPdf(int id)
        {
            using (lear db = new lear())
            {
                Certification customer = db.Certifications.FirstOrDefault(c => c.Id == id);

                var report = new PartialViewAsPdf("~/Views/Shared/DetailCustomer.cshtml", customer);
                return report;
            }

        }
        public ActionResult ExportToWord()
        {
            lear db = new lear();
            // get the data from database
             var data = db.Certifications.ToList();
            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", "attachment;filename = Certificate.doc");
            Response.ContentType = "application/ms-word";
            Response.Charset = "";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
    }   
}