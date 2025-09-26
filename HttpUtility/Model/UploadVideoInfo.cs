using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility.Model
{
    internal class UploadVideoInfo
    {

        public Snippet snippet { get; set; } = new Snippet();   
        public Status status { get; set; } = new Status();
        public UploadVideoInfo(string title, string description, string[] tags, string categoryId, string privacyStatus)
        {
            snippet.title = title;
            snippet.description = description;
            snippet.tags = tags;
            snippet.categoryId = categoryId;
            status.privacyStatus = privacyStatus;
        }
        public class Snippet
        {
            public string title { get; set; }
            public string description { get; set; }
            public string[] tags { get; set; }
            public string categoryId { get; set; }
        }

        public class Status
        {
            public string privacyStatus { get; set; }
        }

    }
}
