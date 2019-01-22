using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IConGenerator.Models;
using IConGenerator.Utils;

namespace IConGenerator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
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
                        var icon = new IconGenerator();
                        icon.CreateIcon( imageResult.FullPath, 128, 128,".png");
                        icon.CreateIcon( imageResult.FullPath, 16, 16,".ico");

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
            return File(CurrentFileName, contentType, CurrentFileName);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}