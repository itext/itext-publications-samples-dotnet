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
using iText.IO.Font;
using iText.IO.Util;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.License;
using iText.Test;
using NUnit.Framework;

namespace iText.Samples
{
    [TestFixtureSource("Data")]
    public class CreateFromURLSampleTest : WrappedSamplesRunner
    {
        private static readonly Dictionary<String, int> expectedNumbersOfPages;

        static CreateFromURLSampleTest()
        {
            expectedNumbersOfPages = new Dictionary<string, int>();

            expectedNumbersOfPages.Add("iText.Samples.Htmlsamples.Chapter07.C07E04_CreateFromURL", 5);
            expectedNumbersOfPages.Add("iText.Samples.Htmlsamples.Chapter07.C07E05_CreateFromURL2", 2);
            expectedNumbersOfPages.Add("iText.Samples.Htmlsamples.Chapter07.C07E06_CreateFromURL3", 2);
        }

        public CreateFromURLSampleTest(RunnerParams runnerParams) : base(runnerParams)
        {
        }

        public static ICollection<TestFixtureData> Data()
        {
            RunnerSearchConfig searchConfig = new RunnerSearchConfig();
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Htmlsamples.Chapter07.C07E04_CreateFromURL");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Htmlsamples.Chapter07.C07E05_CreateFromURL2");
            searchConfig.AddClassToRunnerSearchPath("iText.Samples.Htmlsamples.Chapter07.C07E06_CreateFromURL3");

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
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(dest));

            int currentNumberOfPages = pdfDoc.GetNumberOfPages();
            int expectedNumberOfPages = expectedNumbersOfPages[sampleClass.FullName];
            if (currentNumberOfPages != expectedNumberOfPages)
            {
                AddError("Numbers of pages are not equal.\nExpected: " + expectedNumberOfPages
                                                                       + "\nActual: " + currentNumberOfPages);
            }

            String compareFilePath = "../../../resources/htmlsamples/txt/cmp_" + sampleClass.Name + "_keywords.txt";
            String compareContent = ReadFile(compareFilePath);
            String[] comparePagesContent = compareContent.Split(';');

            // Get the content words of the first page and compare it with expected content words
            String firstPageContentString = PdfTextExtractor.GetTextFromPage(pdfDoc.GetFirstPage(),
                new LocationTextExtractionStrategy());
            IList<String> firstPageWords = JavaUtil.ArraysAsList(firstPageContentString.Split('\n'));
            IList<String> firstPageWordsToCompare = JavaUtil.ArraysAsList(comparePagesContent[0].Split('|'));
            foreach (String word in firstPageWordsToCompare)
            {
                if (!firstPageWords.Contains(word))
                {
                    AddError("Some of the expected words do not present on the first page");
                }
            }

            // Get the content words of the last page and compare it with expected content words
            String lastPageContentString = PdfTextExtractor.GetTextFromPage(pdfDoc.GetLastPage(),
                new LocationTextExtractionStrategy());
            IList<String> lastPageWords = JavaUtil.ArraysAsList(lastPageContentString.Split('\n'));
            IList<String> lastPageWordsToCompare = JavaUtil.ArraysAsList(comparePagesContent[1].Split('|'));
            foreach (String word in lastPageWordsToCompare)
            {
                if (!lastPageWords.Contains(word))
                {
                    AddError("Some of the expected words do not present on the last page");
                }
            }
        }

        private String ReadFile(String filePath)
        {
            StringBuilder contentString = new StringBuilder();
            using (StreamReader reader = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
            {
                String line = reader.ReadLine();
                while (line != null)
                {
                    contentString.Append(line);
                    line = reader.ReadLine();
                }
            }

            return contentString.ToString();
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