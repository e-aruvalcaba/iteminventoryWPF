using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Models
{
    class DriveFileModel
    {
        public string TableId { get; set; }
        public string Fileid { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string DownloadLink { get; set; }
        public string CreatedTime { get; set; }
        public string Modified { get; set; }
        public string Description { get; set; }
    }
}
