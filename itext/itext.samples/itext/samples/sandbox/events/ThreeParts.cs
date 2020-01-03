/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using System.Text;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Events
{
    public class ThreeParts
    {
        public static readonly String DEST = "results/sandbox/events/three_parts.pdf";

        public static readonly String SRC_LA = "../../resources/txt/liber1_{0}_la.txt";
        public static readonly String SRC_EN = "../../resources/txt/liber1_{0}_en.txt";
        public static readonly String SRC_FR = "../../resources/txt/liber1_{0}_fr.txt";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ThreeParts().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            int firstPageNumber = 1;

            for (int i = 0; i < 3; i++)
            {
                // latin
                AddSection(pdfDoc, ReadAndCreateParagraph(String.Format(SRC_LA, i + 1)),
                    firstPageNumber, 0);

                // english
                AddSection(pdfDoc, ReadAndCreateParagraph(String.Format(SRC_EN, i + 1)),
                    firstPageNumber, 1);

                // french
                AddSection(pdfDoc, ReadAndCreateParagraph(String.Format(SRC_FR, i + 1)),
                    firstPageNumber, 2);

                firstPageNumber = pdfDoc.GetNumberOfPages() + 1;
            }

            pdfDoc.Close();
        }

        private static void AddSection(PdfDocument pdfDoc, Paragraph paragraph, int pageNumber, int sectionNumber)
        {
            Document doc = new Document(pdfDoc);
            ParagraphRenderer renderer = (ParagraphRenderer) paragraph.CreateRendererSubTree();
            renderer.SetParent(new DocumentRenderer(doc));

            float pageHeight = pdfDoc.GetDefaultPageSize().GetHeight();
            float pageWidth = pdfDoc.GetDefaultPageSize().GetWidth();
            Rectangle textSectionRectangle = new Rectangle(
                doc.GetLeftMargin(),
                doc.GetBottomMargin() + ((pageHeight - doc.GetTopMargin() - doc.GetBottomMargin()) / 3) * sectionNumber,
                pageWidth - doc.GetLeftMargin() - doc.GetRightMargin(),
                (pageHeight - doc.GetTopMargin() - doc.GetBottomMargin()) / 3);

            // Simulate the positioning of the renderer to find out how much space the text section will occupy.
            LayoutResult layoutResult = renderer
                .Layout(new LayoutContext(new LayoutArea(pageNumber, textSectionRectangle)));

            /* Fill the current page section with the content.
             * If the content isn't fully placed in the current page section,
             * it will be split and drawn in the next page section.
             */
            while (layoutResult.GetStatus() != LayoutResult.FULL)
            {
                if (pdfDoc.GetNumberOfPages() < pageNumber)
                {
                    pdfDoc.AddNewPage();
                }

                pageNumber++;

                layoutResult.GetSplitRenderer().Draw(new DrawContext(pdfDoc,
                    new PdfCanvas(pdfDoc.GetPage(pageNumber - 1)), false));

                renderer = (ParagraphRenderer) layoutResult.GetOverflowRenderer();

                layoutResult = renderer
                    .Layout(new LayoutContext(new LayoutArea(pageNumber, textSectionRectangle)));
            }

            if (pdfDoc.GetNumberOfPages() < pageNumber)
            {
                pdfDoc.AddNewPage();
            }

            renderer.Draw(new DrawContext(pdfDoc, new PdfCanvas(pdfDoc.GetPage(pageNumber)), false));
        }

        private static Paragraph ReadAndCreateParagraph(String path)
        {
            Paragraph p = new Paragraph();
            StringBuilder buffer = new StringBuilder();
            using (StreamReader reader =
                new StreamReader(new FileStream(path, FileMode.Open)))
            {
                String line = reader.ReadLine();
                while (null != line)
                {
                    buffer.Append(line);
                    line = reader.ReadLine();
                }
            }

            p.SetBorder(new SolidBorder(ColorConstants.RED, 1));
            p.Add(buffer.ToString());
            return p;
        }
    }
}