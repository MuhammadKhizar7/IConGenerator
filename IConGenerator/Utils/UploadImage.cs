using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using IConGenerator.Models;

namespace IConGenerator.Utils
{
    public class UploadImage
    {
        // folder for the upload, you can put this in the web.config
        private readonly string UploadPath = "~/Upload/";
        public RenameFileResult RenameUploadFile(HttpPostedFileBase file, Int32 counter = 0)
        {
            var fileName = Path.GetFileName(file.FileName);
//            string ext = Path.GetExtension(fileName);
//            var filename = Path.ChangeExtension(file.FileName, "");
            string prepend = "IconGenerator_";
            string finalFileName = prepend + ((counter).ToString()) + "_" + fileName;
            if (System.IO.File.Exists
                (HttpContext.Current.Request.MapPath(UploadPath+ finalFileName)))
            {
                //file exists => add country try again
                return RenameUploadFile(file, ++counter);
            }
            //file doesn't exist, upload item but validate first
            return new RenameFileResult{File = file, FileName = finalFileName};
        }
        public ImageResult UploadFile(HttpPostedFileBase file, string fileName)
        {
            ImageResult imageResult = new ImageResult { Success = true, ErrorMessage = null };
            var path =
                Path.Combine(HttpContext.Current.Request.MapPath(UploadPath), fileName);
            string extension = Path.GetExtension(file.FileName);

            //make sure the file is valid
            if (!ValidateExtension(extension))
            {
                imageResult.Success = false;
                imageResult.ErrorMessage = "Invalid Extension";
                return imageResult;
            }

            try
            {
                file.SaveAs(path);
                imageResult.ImageName = Path.ChangeExtension(file.FileName, "");
                imageResult.FullPath = path;
                return imageResult;
            }
            catch (Exception ex)
            {
                // you might NOT want to show the exception error for the user
                // this is generaly logging or testing

                imageResult.Success = false;
                imageResult.ErrorMessage = ex.Message;
                return imageResult;
            }
        }
        private bool ValidateExtension(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return true;
                case ".png":
                    return true;
                case ".gif":
                    return true;
                case ".jpeg":
                    return true;
                default:
                    return false;
            }
        }
    }
}