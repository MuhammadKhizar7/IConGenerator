using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IConGenerator.Models
{
    public class RenameFileResult
    {
        public string FileName { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}