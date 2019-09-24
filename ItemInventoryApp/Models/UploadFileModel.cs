using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Models
{
    class UploadFileModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string MimeType { get; set; }
        public string contentLength { get; set; }
    }
}
