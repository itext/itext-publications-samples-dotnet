/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using iText.Kernel.Utils;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class SystemOutWithPdfSampleTest : WrappedSamplesRunner
    {
        private FileStream stream;

        public SystemOutWithPdfSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Sandbox.Security.DecryptPdf");

            return GenerateTestsList(Assembly.GetExecutingAssembly(), searchConfig);
        }

        [Timeout(60000)]
        [Test, Description("{0}")]
        public virtual void Test()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);

            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/all-products.xml");

            // By default target dir is created during runSamples method. In this particular case we need this
            // action before due to overriding system out stream
            new FileInfo(GetDest()).Directory.Create();

            TextWriter storedSystemOut = Console.Out;
            try
            {
                using (stream = File.Open((GetDest().Replace(".pdf", "_sout.txt")),
                    FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    Console.SetOut(writer);
                    RunSamples();
                }
            }
            finally
            {
                Console.SetOut(storedSystemOut);
                LicenseKey.UnloadLicenses();
            }
        }

        protected override void ComparePdf(String outPath, String dest, String cmp)
        {
            AddError(CompareSystemOut(dest, cmp));

            CompareTool compareTool = new CompareTool();
            AddError(compareTool.CompareByContent(dest, cmp, outPath, "diff_"));
            AddError(compareTool.CompareDocumentInfo(dest, cmp));
        }

        protected override String GetCmpPdf(String dest)
        {
            if (dest == null)
            {
                return null;
            }

            int i = dest.LastIndexOf("/");
            int j = dest.LastIndexOf("/results", StringComparison.Ordinal) + 9;
            return "../../resources/" + dest.Substring(j, (i + 1) - j) + "cmp_" + dest.Substring(i + 1);
        }

        private String CompareSystemOut(String dest, String cmp)
        {
            Console.Out.Flush();
            
            String destSystemOut = dest.Replace(".pdf", "_sout.txt");
            String cmpSystemOut = cmp.Replace(".pdf", "_sout.txt");
            using (StreamReader destReader = new StreamReader(new FileStream(destSystemOut,
                    FileMode.Open, FileAccess.Read, FileShare.ReadWrite), Encoding.UTF8),
                cmpReader = new StreamReader(cmpSystemOut, Encoding.UTF8))
            {
                for (int lineNumber = 1; true; ++lineNumber)
                {
                    String destLine = destReader.ReadLine();
                    String cmpLine = cmpReader.ReadLine();
                    if (destLine == null && cmpLine == null)
                    {
                        return null;
                    }
                    if (destLine == null || cmpLine == null)
                    {
                        return "The number of lines is different\n";
                    }
                    if (!cmpLine.Equals(destLine))
                    {
                        return String.Format("Result differs at line {0}\nExpected: \"{1}\"\nActual: \"{2}\"",
                            lineNumber, cmpLine, destLine);
                    }
                }
            }
        }
    }
}