﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
namespace DecisionMaking.Models
{
    public class DisplayDecisionDocumentsViewModel
    {
        public IEnumerable<DecisionDocument> DecisionDocuments { get; set; }
    }
}