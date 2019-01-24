using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IConGenerator.Models;
using IConGenerator.Utils;
using Ionic.Zip;
using Microsoft.Ajax.Utilities;


namespace IConGenerator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection, string Option)
        {
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;

                if (file.ContentLength > 0)
                {

                    var upload = new UploadImage();
                    var rename = upload.RenameUploadFile(file);
                    var imageResult = upload.UploadFile(rename.File, rename.FileName);
                    if (imageResult.Success)
                    {
                        var iconGenerator = new IconGenerator();
                        if (!Option.IsNullOrWhiteSpace() && Option == "multiple")
                        {
                            var sizes = new List<int>{57,60,72,76,114,120,114,150,152,180,192,32,96,115};
                            foreach (var size in sizes)
                            {
                                iconGenerator.CreateIcon(imageResult.FullPath, size, size, "_android.png");
                            }
                            foreach (var size in sizes)
                            {
                                iconGenerator.CreateIcon(imageResult.FullPath, size, size, "_apple.png");
                            }
                        }
                        else
                        {
                        iconGenerator.CreateIcon( imageResult.FullPath, 16, 16,".ico");

                        }



                    }
                    
 

                    if (imageResult.Success)
                    {
                        //TODO: write the filename to the db
                        Console.WriteLine(imageResult.ImageName);
                        ViewBag.Success = "Image Successfully uploaded";
                        ViewBag.FileName = imageResult.ImageName;
                    }
                    else
                    {
                        //TODO: show view error
                        // use imageResult.ErrorMessage to show the error
                        ViewBag.Error = imageResult.ErrorMessage;
                    }

                }
            }

            return View();
        }


        public ActionResult About(string fileName)
        {
            ViewBag.Message = "Click The Download Button to Download";
            var  obj = new DownloadFiles();
            var filesCollection = obj.GetFiles().Where(f=>f.FileName.Contains(fileName));
            return View(filesCollection);

        }
        public FileResult Download(string FileID)
        {
            int CurrentFileID = Convert.ToInt32(FileID);
            var obj = new DownloadFiles();
            var filesCol = obj.GetFiles();
            string CurrentFileName = (filesCol.Where(fls => fls.FileId == CurrentFileID).Select(fls => fls.FilePath)).First();

            string contentType = string.Empty;

            if (CurrentFileName.Contains(".png"))
            {
                contentType = "image/png";
            }

            else if (CurrentFileName.Contains(".ico"))
            {
                contentType = "image/png";
            }
            else
            {
                contentType = "image/*";
            }
            return File(CurrentFileName, contentType, CurrentFileName);
        }

        public FileResult GetAllZip(string fileName)
        {
            var obj = new DownloadFiles();
            var filesCollection = obj.GetFiles().Where(f => f.FileName.Contains(fileName));
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");
                foreach (DownLoadFileInformation file in filesCollection)
                {

                        zip.AddFile(file.FilePath, "Files");

                }

                string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.Save(memoryStream);
                    return File(memoryStream.ToArray(), "application/zip", zipName);
                }
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}