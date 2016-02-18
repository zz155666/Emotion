using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotion.Core.ViewModels
{
   public class ImageContent
    {
        public string credentialName { get; set; }
        public string Url { get; set; }
        public int eId { get; set; }
        public string status { get; set; }
        public string imageFile1 { get; set; }
        public int pId { get; set; }
        public string statusId { get; set; }
        public string imageFile { get; set; }
        public int size { get; set; }
        public Uri uri { get; set; }
    }
}
