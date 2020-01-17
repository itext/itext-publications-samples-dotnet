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
using iText.Layout.Properties;
using iText.License;

namespace iText.Samples.Sandbox.Typography.Khmer
{
    public class KhmerTable
    {
        public const String DEST = "results/sandbox/typography/KhmerTable.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new KhmerTable().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            // Create a pdf document along with a Document (default root layout element) instance
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDocument);

            // ភាសាខ្មែរ
            String text = "\u1797\u17B6\u179F\u17B6\u1781\u17D2\u1798\u17C2\u179A";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "KhmerOS.ttf",
                    PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document
                    .SetFont(font)
                    .SetFontSize(10);
            
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table
                    .AddCell(text)
                    .AddCell(text)
                    .AddCell(text);

            document.Add(table);

            document.Close();
        }
    }
}