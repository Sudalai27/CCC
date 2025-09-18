using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsolOnline.Models
{
    public class AttachmentModel
    {
        public string _id { get; set; }

        public string Itemcode { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string ContentType { get; set; }

        public string FileId { get; set; }
    }
}