using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Model
{
    public class Prosol_Attachment
    {

        public ObjectId _id { get; set; }

        public string Itemcode { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string ContentType { get; set; }        
        public string FileId { get; set; }
        public DateTime Date_ts { get; set; }
    }
}

