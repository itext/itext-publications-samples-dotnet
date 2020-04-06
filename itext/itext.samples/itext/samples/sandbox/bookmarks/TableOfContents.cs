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
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Bookmarks
{
    public class TableOfContents
    {
        public static readonly String DEST = "results/sandbox/bookmarks/table_of_contents.pdf";

        public static readonly String SRC = "../../../resources/text/tree.txt";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableOfContents().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            document
                .SetTextAlignment(TextAlignment.JUSTIFIED)
                .SetFont(font)
                .SetFontSize(11);
            List<Pair<String, Pair<String, int>>> toc =
                new List<Pair<string, Pair<string, int>>>();

            // Parse text to PDF
            CreatePdfWithOutlines(SRC, document, toc, bold);

            // Remove the main title from the table of contents list
            toc.RemoveAt(0);

            // Create table of contents
            document.Add(new AreaBreak());
            Paragraph p = new Paragraph("Table of Contents")
                .SetFont(bold)
                .SetDestination("toc");
            document.Add(p);
            List<TabStop> tabStops = new List<TabStop>();
            tabStops.Add(new TabStop(580, TabAlignment.RIGHT, new DottedLine()));
            foreach (Pair<String, Pair<String, int>> entry in toc)
            {
                Pair<String, int> text = entry.Value;
                p = new Paragraph()
                    .AddTabStops(tabStops)
                    .Add(text.Key)
                    .Add(new Tab())
                    .Add(text.Value.ToString())
                    .SetAction(PdfAction.CreateGoTo(entry.Key));
                document.Add(p);
            }

            // Move the table of contents to the first page
            int tocPageNumber = pdfDoc.GetNumberOfPages();
            pdfDoc.MovePage(tocPageNumber, 1);

            // Add page labels
            pdfDoc.GetPage(1).SetPageLabel(PageLabelNumberingStyle.UPPERCASE_LETTERS,
                null, 1);
            pdfDoc.GetPage(2).SetPageLabel(PageLabelNumberingStyle.DECIMAL_ARABIC_NUMERALS,
                null, 1);

            document.Close();
        }

        private static void CreatePdfWithOutlines(String path, Document document,
            List<Pair<String, Pair<String, int>>> toc, PdfFont titleFont)
        {
            PdfDocument pdfDocument = document.GetPdfDocument();

            using (StreamReader br = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                String line;
                bool title = true;
                int counter = 0;
                PdfOutline outline = null;
                while ((line = br.ReadLine()) != null)
                {
                    Paragraph p = new Paragraph(line);
                    p.SetKeepTogether(true);
                    if (title)
                    {
                        String name = String.Format("title0{0}", counter++);
                        outline = CreateOutline(outline, pdfDocument, line, name);
                        Pair<String, int> titlePage =
                            new Pair<String, int>(line, pdfDocument.GetNumberOfPages());
                        p
                            .SetFont(titleFont)
                            .SetFontSize(12)
                            .SetKeepWithNext(true)
                            .SetDestination(name)

                            // Add the current page number to the table of contents list
                            .SetNextRenderer(new UpdatePageRenderer(p, titlePage));
                        document.Add(p);
                        toc.Add(new Pair<String, Pair<String, int>>(name, titlePage));
                        title = false;
                    }
                    else
                    {
                        p.SetFirstLineIndent(36);
                        if (line.Equals(""))
                        {
                            p.SetMarginBottom(12);
                            title = true;
                        }
                        else
                        {
                            p.SetMarginBottom(0);
                        }

                        document.Add(p);
                    }
                }
            }
        }

        private static PdfOutline CreateOutline(PdfOutline outline, PdfDocument pdf, String title, String name)
        {
            if (outline == null)
            {
                outline = pdf.GetOutlines(false);
                outline = outline.AddOutline(title);
                outline.AddDestination(PdfDestination.MakeDestination(new PdfString(name)));
            }
            else
            {
                PdfOutline kid = outline.AddOutline(title);
                kid.AddDestination(PdfDestination.MakeDestination(new PdfString(name)));
            }

            return outline;
        }

        private class UpdatePageRenderer : ParagraphRenderer
        {
            protected Pair<String, int> entry;

            public UpdatePageRenderer(Paragraph modelElement, Pair<String, int> entry) :
                base(modelElement)
            {
                this.entry = entry;
            }

            public override LayoutResult Layout(LayoutContext layoutContext)
            {
                LayoutResult result = base.Layout(layoutContext);
                entry.Value = layoutContext.GetArea().GetPageNumber();
                return result;
            }
        }

        private class Pair<T, U>
        {
            public Pair(T first, U second)
            {
                this.Key = first;
                this.Value = second;
            }

            public T Key { get; set; }

            public U Value { get; set; }
        };
    }
}