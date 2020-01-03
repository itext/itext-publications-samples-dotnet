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

namespace iText.Samples.Sandbox.Typography.Latin
{
    public class LatinBreveDiacritic
    {
        public const String DEST = "results/sandbox/typography/LatinBreveDiacritic.pdf";
        public const String FONTS_FOLDER = "../../resources/font/";

        public static void Main(String[] args)
        {
            // Load the license file to use typography features
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-typography.xml");

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LatinBreveDiacritic().CreatePDF(DEST);
        }

        public virtual void CreatePDF(String dest)
        {
            Document document = new Document(new PdfDocument(new PdfWriter(dest)));

            // ĂăĔĕĞğĬĭŎŏŬŭ˘ḜḝẶặᾰᾸῐῘῠῨ
            String line1 =
                    "\u0102\u0103\u0114\u0115\u011E\u011F\u012C\u012D\u014E\u014F\u016C\u016D\u02D8\u1E1C\u1E1D\u1EB6"
                    + "\u1EB7\u1FB0\u1FB8\u1FD0\u1FD8\u1FE0\u1FE8";

            PdfFont font = PdfFontFactory.CreateFont(FONTS_FOLDER + "FreeSans.ttf", PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            document.SetFont(font);
            document.SetFontSize(10);

            document.Add(new Paragraph(line1));

            document.Close();
        }
    }
}