using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace IConGenerator.Utils
{
    public class FileCopy
    {
        /// <summary>
        /// CopyFile
        /// </summary>
        /// <param name="sourceFileName">source file path</param>
        /// <param name="suffix">suffix</param>
        private void CopyFile(string sourceFileName, string suffix)
        {

            //get the directory name the file is in
            string sourceDirectory = Path.GetDirectoryName(sourceFileName);

            //get the file name without extension
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFileName);

            //get the file extension
            string fileExtension = Path.GetExtension(sourceFileName);

            //get the new file name you want,the Combine method is strongly recommended
            string destFileName = Path.Combine(sourceDirectory, filenameWithoutExtension + suffix + fileExtension);

            //You can use string operation below instead of all above,it also works but I think the Path class is more clear
            //string destFileName = sourceFileName.Remove(sourceFileName.LastIndexOf('.')) + suffix + ".tif";

            File.Copy(sourceFileName, destFileName);
        }
    }
}