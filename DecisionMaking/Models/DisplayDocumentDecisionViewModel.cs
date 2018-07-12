using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace DecisionMaking.Models
{
    public class DisplayDocumentDecisionViewModel
    {
        public int DecisionID { get; set; }
        public DecisionDocument DecisionDocument { get; set; }
    }
}