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
using iText.Pdfa;

namespace iText.Samples.Sandbox.Pdfa
{
    public class HelloPdfA2a
    {
        public static readonly string DEST = "results/sandbox/pdfa/hello_pdf_a_2a.pdf";

        public static readonly String FONT = "../../resources/font/OpenSans-Regular.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HelloPdfA2a().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            FileStream fileStream = new FileStream("../../resources/data/sRGB_CS_profile.icm",
                FileMode.Open, FileAccess.Read);

            PdfADocument pdfDoc = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_2A,
                new PdfOutputIntent("Custom", "",
                    null, "sRGB IEC61966-2.1", fileStream));

            Document document = new Document(pdfDoc);

            // Specifies that document should contain tag structure
            pdfDoc.SetTagged();
            pdfDoc.GetCatalog().SetLang(new PdfString("en-us"));

            Paragraph p = new Paragraph("Hello World!").SetFont(font).SetFontSize(10);
            document.Add(p);
            document.Close();
        }
    }
}