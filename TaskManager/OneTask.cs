using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskManager
{
    public class OneTask
    {
        public string subject { get; set; }
        public string shortDescription { get; set; }
        public DateTime deadLine { get; set; }
        public string priority { get; set; }
        public DateTime dateDoc { get; set; }
        public string numberDoc { get; set; }

        public OneTask()
        {
            subject = string.Empty;
            shortDescription = string.Empty;
            deadLine = DateTime.Now;
            priority = string.Empty;
            dateDoc = DateTime.Now;
            numberDoc = string.Empty;
        }
    }
}
