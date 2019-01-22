using System.Collections.Generic;

namespace IConGenerator.Models
{
    public class ImageResult
    {
 
        public bool Success { get; set; }
        public string ImageName { get; set; }
        public string ErrorMessage { get; set; }
        public string FullPath { get; set; }
        
    }
}