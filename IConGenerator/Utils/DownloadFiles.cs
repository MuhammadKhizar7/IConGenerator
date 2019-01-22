using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using IConGenerator.Models;

namespace IConGenerator.Utils
{
    public class DownloadFiles
    {
        public List<DownLoadFileInformation> GetFiles()
        {
            List<DownLoadFileInformation> lstFiles = new List<DownLoadFileInformation>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Upload"));

            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                lstFiles.Add(new DownLoadFileInformation()
                {

                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
            }
            return lstFiles;
        }
    }
}