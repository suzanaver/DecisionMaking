using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class OptionAdvantage
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int OptionID { get; set; }

        public string Status { get; set; }
    }
}
