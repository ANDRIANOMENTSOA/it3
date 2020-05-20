using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class ObservationModel
    {
        public string osbValue { get; set; }
        public string subject { get; set; }
        public string dateObs { get; set; }
        public string hdrguid { get; set; }
        public string recId { get; set; }
        public string docNumber { get; set; }
    }
}