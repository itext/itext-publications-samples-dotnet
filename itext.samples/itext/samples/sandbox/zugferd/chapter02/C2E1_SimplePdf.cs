/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
/*
* This code sample was written in the context of the tutorial:
* ZUGFeRD: The future of Invoicing
*/
using System;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Zugferd.Chapter02 {
    /// <summary>Creates a simple PDF with images and text.</summary>
    public class C2E1_SimplePdf : GenericTest {
        public const String FOX = "./src/test/resources/img/fox.bmp";

        public const String DOG = "./src/test/resources/img/dog.bmp";

        public const String DEST = "./target/test/resources/zugferd/chapter02/C2E1_SimplePdf.pdf";

        /// <summary>Creates a simple PDF with images and text</summary>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        protected override void ManipulatePdf(String dest) {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest, new WriterProperties().SetPdfVersion(PdfVersion.PDF_1_7
                )));
            Document doc = new Document(pdfDoc, new PageSize(PageSize.A4).Rotate());
            Paragraph p = new Paragraph();
            p.SetFont(PdfFontFactory.CreateFont(FontConstants.HELVETICA)).SetFontSize(20);
            Text text = new Text("The quick brown ");
            p.Add(text);
            iText.Layout.Element.Image image = new Image(ImageDataFactory.Create(FOX));
            p.Add(image);
            text = new Text(" jumps over the lazy ");
            p.Add(text);
            image = new iText.Layout.Element.Image(ImageDataFactory.Create(DOG));
            p.Add(image);
            doc.Add(p);
            doc.Close();
        }
    }
}
