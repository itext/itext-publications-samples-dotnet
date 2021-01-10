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
using System.Linq;
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
        private static readonly Dictionary<String, int[]> expectedNumbersOfPages;

        static CreateFromURLSampleTest()
        {
            expectedNumbersOfPages = new Dictionary<string, int[]>();

            expectedNumbersOfPages.Add("iText.Samples.Htmlsamples.Chapter07.C07E04_CreateFromURL", new int[] {4, 5});
            expectedNumbersOfPages.Add("iText.Samples.Htmlsamples.Chapter07.C07E05_CreateFromURL2", new int[] {2, 3});
            expectedNumbersOfPages.Add("iText.Samples.Htmlsamples.Chapter07.C07E06_CreateFromURL3", new int[] {2, 3});
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

		/// This test is expected to be flaky, because its output depends on the content of the site page, 
		/// which could be changed at any time
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
            List<int> expectedNumberOfPages = expectedNumbersOfPages[sampleClass.FullName].ToList();
            if (!expectedNumberOfPages.Contains(currentNumberOfPages))
            {
                AddError("Number of pages is out of expected range.\nActual: " + currentNumberOfPages);
            }

            // Get words to check the resultant pdf file
            String compareFilePath = "../../../cmpfiles/htmlsamples/txt/cmp_" + sampleClass.Name + "_keywords.txt";
            String compareContent = ReadFile(compareFilePath);
            List<String> cmpPdfWords = new List<String>(compareContent.Split('|'));

            // Get all words from all pages of the resultant pdf file
            List<String> destPdfWords = new List<String>();
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                String pageContentString = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i),
                    new LocationTextExtractionStrategy());
                List<String> pageWords = new List<String>(pageContentString.Replace("\n", " ")
                    .Split(' '));
                destPdfWords.AddRange(pageWords);
            }
            
            StringBuilder errorMessage = new StringBuilder();
            foreach (String word in cmpPdfWords) {
                if (!destPdfWords.Contains(word)) {
                    errorMessage.Append(word).Append(",");
                }
            }
            String errorText = errorMessage.ToString();
            if (!errorText.Equals("")) {
                AddError("Some words are missing in the result pdf: " + errorText);
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