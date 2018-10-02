using Fakulteti.Helpers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Fakulteti.Helpers
{
    public class GjenerimiRaporteve
    {
        private string[] Parametrat;
        string reportHeight, reportWidth;
        private List<DataTable> objDataTable;

        public GjenerimiRaporteve()
        {
            reportWidth = "8.5in";
            reportHeight = "11in";
        }

        public enum LlojiRaportit
        {
            rptTranskriptaNotave = 0,
        }

        public byte[] GjeneroRaportin(LlojiRaportit lloji, string reportPathAndName, List<DataTable> objDataTables = null, params string[] reportParameters)
        {
            this.objDataTable = objDataTables;
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = reportPathAndName;
            reportViewer.LocalReport.EnableExternalImages = true;
            var parametrat = new ReportParameterCollection();
            Parametrat = reportParameters;

            switch (lloji)
            {

                case LlojiRaportit.rptTranskriptaNotave:
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsTranskriptaNotave", objDataTable[0]));
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsStatike", objDataTable[1]));
                    reportViewer.LocalReport.EnableHyperlinks = true;
                    break;

                

                default:
                    break;
            }

            // general settings
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            var deviceInfo = "<DeviceInfo>" + "  <OutputFormat>PDF</OutputFormat>" + "<PageWidth>" + reportWidth + "</PageWidth>" +
                         "  <PageHeight>" + reportHeight + "</PageHeight>" + "  <MarginTop>0in</MarginTop>" +
                         "  <MarginLeft>0in</MarginLeft>" + "  <MarginRight>0in</MarginRight>" +
                         "  <MarginBottom>0in</MarginBottom>" + "</DeviceInfo>";

            byte[] bytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            return bytes;
        }
    }
}