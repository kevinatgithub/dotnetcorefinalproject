using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
