using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RCT.Web.Utility
{
    public class ReportWrapper
    {
        // Constructors
        public ReportWrapper()
        {
            // Constructors
            ReportDataSources = new List<ReportDataSource>();
            ReportParameters = new List<ReportParameter>();
        }

        // Properties
        public string ReportPath { get; set; }

        public List<ReportDataSource> ReportDataSources { get; set; }

        public List<ReportParameter> ReportParameters { get; set; }

        public bool IsDownloadDirectly { get; set; }
    }

    public class reportParm
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}