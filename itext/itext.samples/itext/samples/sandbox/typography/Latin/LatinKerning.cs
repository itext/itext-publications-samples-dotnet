/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.License;
using iText.Typography.Config;

namespace iText.Samples.Sandbox.Typography.Latin
{
    public class LatinKerning
    {
        public const String DEST = "../../results/sandbox/typography/LatinKerning.pdf";
        public const String FONTS_FOLDER = "../../itext/samples/sandbox/typography/latin/resources/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LatinKerning().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "FoglihtenNo07.otf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document.SetFont(font);

            // Add a new paragraph with the default kerning feature
            Paragraph p = new Paragraph("FoglihtenNo07.otf: Paragraph with the default (off) kerning feature in" +
                                        " layout: \nAtAVAWAwAYAvLTLWPA" + "\nWAWeYaWaWAWAWA7447// // 4");
            document.Add(p);

            // Add a new paragraph with switched off kerning feature
            Paragraph pKernOff = new Paragraph("FoglihtenNo07.otf: Paragraph with switched OFF kerning feature in" +
                                               " layout: \nAtAVAWAwAYAvLTLWPA" + "\nWAWeYaWaWAWAWA7447// // 4");
            pKernOff.SetProperty(Property.TYPOGRAPHY_CONFIG, new TypographyConfigurator()
                    .AddFeatureConfig(new LatinScriptConfig()
                            .SetKerningFeature(false)));
            document.Add(pKernOff);

            // Add a new paragraph with switched on kerning feature
            Paragraph pKernOn = new Paragraph("FoglihtenNo07.otf: Paragraph with switched ON kerning feature in" +
                                              " layout: \nAtAVAWAwAYAvLTLWPA" + "\nWAWeYaWaWAWAWA7447// // 4");
            pKernOn.SetProperty(Property.TYPOGRAPHY_CONFIG, new TypographyConfigurator()
                    .AddFeatureConfig(new LatinScriptConfig()
                            .SetKerningFeature(true)));
            document.Add(pKernOn);

            // Add a new paragraph with switched on kerning and ligature features
            Paragraph pKernOnLigaOn = new Paragraph("FoglihtenNo07.otf: Paragraph with switched ON kerning and ON" +
                                                    " ligature feature in layout: \nAtAVAWAwAYAvLTLWPA" 
                                                    + "\nWAWeYaWaWAWAWA7447// // 4");
            pKernOnLigaOn.SetProperty(Property.TYPOGRAPHY_CONFIG, new TypographyConfigurator()
                    .AddFeatureConfig(new LatinScriptConfig()
                            .SetLigaturesApplying(true)
                            .SetKerningFeature(true)));
            document.Add(pKernOnLigaOn);

            document.Close();
        }
    }
}