/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
using iText.License;

namespace iText.Samples.Sandbox.Typography.Malayalam
{
    public class MalayalamWordSpacing
    {
        public const String DEST = "results/sandbox/typography/MalayalamWordSpacing.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MalayalamWordSpacing().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // സാേങ്കതിക പദസൂചികഒരു സ്വതന്ത്ര
            String text = "\u0D38\u0D3E\u0D47\u0D19\u0D4D\u0D15\u0D24\u0D3F\u0D15\u0020\u0D2A\u0D26\u0D38\u0D42\u0D1A"
                          + "\u0D3F\u0D15\u0D12\u0D30\u0D41\u0020\u0D38\u0D4D\u0D35\u0D24\u0D28\u0D4D\u0D24\u0D4D\u0D30";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "NotoSansMalayalam-Regular.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);

            // Create paragraph, add string to it with the default word spacing and add all to the document
            document.Add(new Paragraph(text));

            // Add text with a word spacing of 10 points
            document.Add(new Paragraph(text).SetWordSpacing(10));

            // Set word spacing on the document. It will be applied to the next paragraph
            document.SetWordSpacing(20);
            document.Add(new Paragraph(text));

            document.Close();
        }
    }
}