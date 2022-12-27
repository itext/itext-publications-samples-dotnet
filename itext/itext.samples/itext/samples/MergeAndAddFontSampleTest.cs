/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2023 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.Commons.Utils;
using iText.IO.Font;
using iText.Kernel.Utils;
using iText.Licensing.Base;
using iText.Licensing.Base.Reporting;
using iText.Samples.Sandbox.Fonts;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class MergeAndAddFontSampleTest : WrappedSamplesRunner
    {
        public MergeAndAddFontSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Fonts.MergeAndAddFont");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            LicenseKeyReportingConfigurer.UseLocalReporting("./target/test/com/itextpdf/samples/report/");
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            FontCache.ClearSavedFonts();
            FontProgramFactory.ClearRegisteredFonts();
            
            RunSamples();
            LicenseKey.UnloadLicenses();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            CompareTool compareTool = new CompareTool();
            
            foreach (String fileName in MergeAndAddFont.DEST_NAMES.Values) {
                String currentDest = dest + fileName;
                String currentCmp = cmp + "cmp_" + fileName;

                AddError(compareTool.CompareByContent(currentDest, currentCmp, outPath, "diff_"));
                AddError(compareTool.CompareDocumentInfo(currentDest, currentCmp));
            }
        }

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../../cmpfiles/" + dest.Substring(j);
        }
    }
}
