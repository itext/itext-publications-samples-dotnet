using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class TableSplitTest
    {
        public static readonly string DEST = "results/sandbox/tables/tables_split_test.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableSplitTest().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(595, 842));

            doc.SetMargins(55, 15, 35, 15);

            ILineDrawer line = new SolidLine(2);
            line.SetColor(ColorConstants.LIGHT_GRAY);

            LineSeparator tableEndSeparator = new LineSeparator(line);
            tableEndSeparator.SetMarginTop(10);

            String[] header = {"Header1", "Header2", "Header3", "Header4", "Header5"};
            String[] content = {"column 1", "column 2", "some Text in column 3", "Test data ", "column 5"};

            Table table = new Table(
                    UnitValue.CreatePercentArray(new float[] {3, 2, 4, 3, 2})).UseAllAvailableWidth();

            foreach (String columnHeader in header)
            {
                Paragraph headerParagraph = new Paragraph(columnHeader)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                    .SetFontSize(10);

                Cell headerCell = new Cell()
                    .Add(headerParagraph)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                    .SetPadding(8);

                table.AddHeaderCell(headerCell);
            }

            for (int i = 0; i < 15; i++)
            {
                int j = 0;

                foreach (String text in content)
                {
                    Paragraph paragraph = new Paragraph((i == 13 && j == 3) ? "Test data \n Test data \n Test data" : text)
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                        .SetFontSize(10);

                    Cell cell = new Cell()
                        .Add(paragraph)
                        .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                        .SetPaddingLeft(5)
                        .SetPaddingTop(5)
                        .SetPaddingRight(5)
                        .SetPaddingBottom(5);

                    table.AddCell(cell);

                    j++;
                }
            }

            doc.Add(table);
            doc.Add(tableEndSeparator);

            for (int k = 0; k < 5; k++)
            {
                Paragraph info = new Paragraph("Some title")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(10)
                    .SetMarginTop(12);
                
                doc.Add(info);

                table = new Table(
                    UnitValue.CreatePercentArray(new float[] {3, 2, 4, 3, 2})).UseAllAvailableWidth();
                table.SetMarginTop(15);

                foreach (String columnHeader in header)
                {
                    Paragraph paragraph = new Paragraph(columnHeader)
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                        .SetFontSize(10);
                    
                    Cell headerCell = new Cell()
                        .Add(paragraph)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                        .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                        .SetPaddingLeft(8)
                        .SetPaddingTop(8)
                        .SetPaddingRight(8)
                        .SetPaddingBottom(8);

                    table.AddHeaderCell(headerCell);
                }

                foreach (String text in content)
                {
                    Paragraph paragraph = new Paragraph(text)
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                        .SetFontSize(10);
                    
                    Cell cell = new Cell()
                        .Add(paragraph)
                        .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                        .SetPaddingLeft(5)
                        .SetPaddingTop(5)
                        .SetPaddingRight(5)
                        .SetPaddingBottom(5);
                    
                    table.AddCell(cell);
                }

                doc.Add(table);
                doc.Add(tableEndSeparator);
            }

            doc.Close();
        }
    }
}