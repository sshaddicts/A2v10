﻿using System;
using System.Configuration;
using System.Collections.Generic;
using Stimulsoft.Report.Export;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Mvc;

namespace A2v10.Reports
{
    public static class StiReportExtensions
    {
        public static StiPdfExportSettings GetDefaultPdfSettings()
        {
            return new Stimulsoft.Report.Export.StiPdfExportSettings()
            {
                EmbeddedFonts = true,
                UseUnicode = true,
                ImageResolution = 300,
                ImageCompressionMethod = StiPdfImageCompressionMethod.Flate
            };
        }

        public static StiExcelExportSettings GetDefaultXlSettings()
        {
            return new Stimulsoft.Report.Export.StiExcelExportSettings()
            {
                ExcelType = StiExcelType.Excel2007
            };
        }
        public static void AddReferencedAssemblies(this StiReport report)
        {
            int asCount = 1;
            int len = report.ReferencedAssemblies.Length;
            var ra = new String[len + asCount];
            Array.Copy(report.ReferencedAssemblies, ra, len);
            ra[len] = "A2v10.Infrastructure";
            report.ReferencedAssemblies = ra;
        }

        public static void SubstDataSources(this StiReport report)
        {
            var cns = ConfigurationManager.ConnectionStrings;
            foreach (var db in report.Dictionary.Databases)
            {
                var dbs = db as StiSqlDatabase;
                if (dbs != null)
                {
                    var st = cns[dbs.Name];
                    if (st != null)
                        dbs.ConnectionString = st.ConnectionString;
                }
            }
        }

        public static StiReport CreateReport(String path)
        {
            var r = new Stimulsoft.Report.StiReport();
            r.Load(path);
            r.AddReferencedAssemblies();
            r.SubstDataSources();
            return r;
        }

        public static void AddVariables(this StiReport report, IDictionary<String, Object> vars)
        {
            if (report?.Dictionary?.Variables?.Items == null)
                return;
            if (vars == null)
                return;
            foreach (var vp in vars)
            {
                if (vp.Value != null)
                {
                    foreach (var xs in report?.Dictionary?.Variables?.Items)
                    {
                        if (xs.Name == vp.Key)
                        {
                            xs.ValueObject = vp.Value;
                            xs.Type = vp.Value.GetType();
                        }
                    }
                }
            }
        }
    }
}
