/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2022 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using iText.IO.Font;
using iText.Commons.Utils;
using iText.Kernel.Geom;
using iText.Kernel.Utils;
using iText.Licensing.Base;
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
            
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            RunSamples();
            LicenseKey.UnloadLicenses();
        }

        protected override void ComparePdf(string outPath, string dest, string cmp)
        {
            CompareTool compareTool = new CompareTool();
            for (int i = 1; i <= 3; i++)
            {
                String currentDest = String.Format(dest, i);
                String currentCmp = String.Format(cmp, i);

                Rectangle ignoredArea = new Rectangle(30, 700, 120, 18);
                IList<Rectangle> rectangles = JavaUtil.ArraysAsList(ignoredArea);
                Dictionary<int, IList<Rectangle>> ignoredAreasMap = new Dictionary<int, IList<Rectangle>>();
                ignoredAreasMap.Add(1, rectangles);
                AddError(compareTool.CompareVisually(currentDest, currentCmp, outPath, "diff_",
                    ignoredAreasMap));
                AddError(compareTool.CompareDocumentInfo(currentDest, currentCmp));
            }
        }
    }
}
