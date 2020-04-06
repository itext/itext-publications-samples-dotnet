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
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Pdfua
{
    public class PdfUA
    {
        public static readonly string DEST = "results/sandbox/pdfua/pdf_ua.pdf";

        public static readonly String DOG = "../../../resources/img/dog.bmp";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";

        public static readonly String FOX = "../../../resources/img/fox.bmp";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfUA().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest,
                new WriterProperties().AddUAXmpMetadata().SetPdfVersion
                    (PdfVersion.PDF_1_7)));
            Document document = new Document(pdfDoc, PageSize.A4.Rotate());

            //TAGGED PDF
            //Make document tagged
            pdfDoc.SetTagged();

            //PDF/UA
            //Set document metadata
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("English pangram");

            Paragraph p = new Paragraph();

            //PDF/UA
            //Embed font
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, true);
            p.SetFont(font);

            p.Add("The quick brown ");

            Image img = new Image(ImageDataFactory.Create(FOX));

            //PDF/UA
            //Set alt text
            img.GetAccessibilityProperties().SetAlternateDescription("Fox");
            p.Add(img);
            p.Add(" jumps over the lazy ");

            img = new Image(ImageDataFactory.Create(DOG));

            //PDF/UA
            //Set alt text
            img.GetAccessibilityProperties().SetAlternateDescription("Dog");
            p.Add(img);

            document.Add(p);

            p = new Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n").SetFont(font).SetFontSize(20);
            document.Add(p);

            List list = new List().SetFont(font).SetFontSize(20);
            list.Add(new ListItem("quick"));
            list.Add(new ListItem("brown"));
            list.Add(new ListItem("fox"));
            list.Add(new ListItem("jumps"));
            list.Add(new ListItem("over"));
            list.Add(new ListItem("the"));
            list.Add(new ListItem("lazy"));
            list.Add(new ListItem("dog"));
            document.Add(list);

            document.Close();
        }
    }
}