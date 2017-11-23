/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV

*/
/*
* This code sample was written in the context of the tutorial:
* ZUGFeRD: The future of Invoicing
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
using iText.Pdfa;

namespace iText.Samples.Sandbox.Zugferd.Chapter02 {
    /// <summary>Creates a PDF that conforms with PDF/A-3 Level A.</summary>
    public class C2E4_PdfA3a : GenericTest {
        public static readonly String ICC = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/data/sRGB_CS_profile.icm";

        public static readonly String FONT = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/font/FreeSans.ttf";

        public static readonly String FOX = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/img/fox.bmp";

        public static readonly String DOG = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/img/dog.bmp";

        public static readonly String DEST = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/test/resources/zugferd/chapter02/C2E4_PdfA3a.pdf";

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="iText.Kernel.XMP.XMPException"/>
        /// <exception cref="System.Exception"/>
        protected override void ManipulatePdf(String dest) {
            //PDF/A-3a
            //Set output intents
            //Create PdfAWDocument with the required conformance level
            Stream @is = new FileStream(ICC, FileMode.Open, FileAccess.Read);
            PdfADocument pdfDoc = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_3A, new PdfOutputIntent
                ("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1", @is));
            Document doc = new Document(pdfDoc, new PageSize(PageSize.A4).Rotate());
            //====================
            //TAGGED PDF
            //Make document tagged
            pdfDoc.SetTagged();
            //===============
            //PDF/UA
            //Set document metadata
            pdfDoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            pdfDoc.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDoc.GetDocumentInfo();
            info.SetTitle("Some title");
            //=====================
            Paragraph p = new Paragraph();
            //PDF/A-3a
            //Embed font
            p.SetFont(PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, true)).SetFontSize(20);
            Text text = new Text("The quick brown ");
            p.Add(text);
            iText.Layout.Element.Image image = new Image(ImageDataFactory.Create(FOX));
            //PDF/UA
            //Set alt text
            image.GetAccessibilityProperties().SetAlternateDescription("Fox");
            //==============
            p.Add(image);
            text = new Text(" jumps over the lazy ");
            p.Add(text);
            image = new iText.Layout.Element.Image(ImageDataFactory.Create(DOG));
            //PDF/UA
            //Set alt text
            image.GetAccessibilityProperties().SetAlternateDescription("Dog");
            //==================
            p.Add(image);
            doc.Add(p);
            doc.Close();
        }
    }
}
