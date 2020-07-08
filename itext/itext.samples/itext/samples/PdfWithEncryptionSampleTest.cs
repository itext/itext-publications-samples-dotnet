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
using System.Text;
using iText.Kernel.Utils;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class PdfWithEncryptionSampleTest : WrappedSamplesRunner
    {

        public PdfWithEncryptionSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Security.EncryptPdf");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Security.DecryptPdf2");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            LicenseKey.UnloadLicenses();
            RunSamples();
        }

        protected override void ComparePdf(String outPath, String dest, String cmp)
        {
            CompareTool compareTool = new CompareTool();
            compareTool.EnableEncryptionCompare();
            
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_", 
                Encoding.UTF8.GetBytes("World"), Encoding.UTF8.GetBytes("World")));
            AddError(compareTool.CompareDocumentInfo(dest, cmp, 
                Encoding.UTF8.GetBytes("World"), Encoding.UTF8.GetBytes("World")));
        }
    }
}