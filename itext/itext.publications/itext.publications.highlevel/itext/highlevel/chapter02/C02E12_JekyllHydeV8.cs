/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using iText.Test.Attributes;

namespace itext.publications.highlevel.itext.highlevel.chapter02 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C02E12_JekyllHydeV8 {
        public const String SRC = "../../resources/txt/jekyll_hyde.txt";

        public const String DEST = "results/chapter02/jekyll_hyde_v8.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E12_JekyllHydeV8().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf, PageSize.A4, false);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLD);
            document.SetTextAlignment(TextAlignment.JUSTIFIED).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3))
                .SetFont(font).SetFontSize(11);
            Text totalPages = new Text("This document has {totalpages} pages.");
            IRenderer renderer = new TextRenderer(totalPages);
            totalPages.SetNextRenderer(renderer);
            document.Add(new Paragraph(totalPages));
            StreamReader sr = File.OpenText(SRC);
            String line;
            Paragraph p;
            bool title = true;
            while ((line = sr.ReadLine()) != null) {
                p = new Paragraph(line);
                p.SetKeepTogether(true);
                if (title) {
                    p.SetFont(bold).SetFontSize(12);
                    title = false;
                }
                else {
                    p.SetFirstLineIndent(36);
                }
                if (String.IsNullOrEmpty(line)) {
                    p.SetMarginBottom(12);
                    title = true;
                }
                else {
                    p.SetMarginBottom(0);
                }
                document.Add(p);
            }
            String total = renderer.ToString().Replace("{totalpages}", pdf.GetNumberOfPages().ToString());
            ((TextRenderer)renderer).SetText(total);
            ((Text)renderer.GetModelElement()).SetNextRenderer(renderer);
            document.Relayout();
            //Close document
            document.Close();
        }
    }
}
