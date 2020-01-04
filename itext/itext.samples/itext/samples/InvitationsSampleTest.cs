/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Utils;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class InvitationsSampleTest : WrappedSamplesRunner
    {
        public InvitationsSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddPackageToRunnerSearchPath("iText.Samples.Htmlsamples.Chapter05.C05E03_Invitations");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            FontCache.ClearSavedFonts();
            FontProgramFactory.ClearRegisteredFonts();
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.xml");
            RunSamples();
            ResetLicense();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            CompareTool compareTool = new CompareTool();
            for (int i = 1; i <= 3; i++)
            {
                String currentDest = String.Format(dest, i);
                String currentCmp = String.Format(cmp, i);

                Rectangle ignoredArea = new Rectangle(30, 689, 120, 18);
                IList<Rectangle> rectangles = JavaUtil.ArraysAsList(ignoredArea);
                Dictionary<int, IList<Rectangle>> ignoredAreasMap = new Dictionary<int, IList<Rectangle>>();
                ignoredAreasMap.Add(1, rectangles);
                AddError(compareTool.CompareVisually(currentDest, currentCmp, outPath, "diff_",
                    ignoredAreasMap));
                AddError(compareTool.CompareDocumentInfo(currentDest, currentCmp));
            }
        }

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            int i = dest.LastIndexOf("/", StringComparison.Ordinal);
            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../resources/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        private void ResetLicense()
        {
            try
            {
                FieldInfo validatorsField = typeof(LicenseKey).GetField("validators",
                    BindingFlags.NonPublic | BindingFlags.Static);
                validatorsField.SetValue(null, null);
                FieldInfo versionField = typeof(Kernel.Version).GetField("version",
                    BindingFlags.NonPublic | BindingFlags.Static);
                versionField.SetValue(null, null);
            }
            catch
            {
                
                // No exception handling required, because there can be no license loaded
            }
        }
    }
}