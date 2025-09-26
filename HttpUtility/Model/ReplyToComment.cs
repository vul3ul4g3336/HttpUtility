using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtility.Model
{
    internal class ReplyToComment
    {


        public Snippet snippet { get; set; }


        public class Snippet
        {
            public string parentId { get; set; }
            public string textOriginal { get; set; }
        }

    }
}
