using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables 
{
    public class CustomBorder3 
    {
        public static readonly String DEST = "results/sandbox/tables/custom_border3.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new CustomBorder3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            
            // By default column width is calculated automatically for the best fit.
            // useAllAvailableWidth() method makes table use the whole page's width while placing the content.
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            Cell cell = new Cell().Add(new Paragraph("dotted left border"));
            
            // By default all the cell's borders are black and have 0.5pt width.
            // To write only the left border one should set the value of the others to null.
            cell.SetBorder(Border.NO_BORDER);
            cell.SetBorderLeft(new DottedBorder(0.5f));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("solid right border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetBorderRight(new SolidBorder(0.5f));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("dashed top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetBorderTop(new DashedBorder(1f));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetBorderBottom(new SolidBorder(1f));
            table.AddCell(cell);
            
            document.Add(table);
            
            table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.SetMarginBottom(30);
            
            cell = new Cell().Add(new Paragraph("dotted left and solid top border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetBorderTop(new SolidBorder(1f));
            cell.SetBorderLeft(new DottedBorder(0.5f));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("dashed right and dashed bottom border"));
            cell.SetBorder(Border.NO_BORDER);
            cell.SetBorderBottom(new DashedBorder(1f));
            cell.SetBorderRight(new DashedBorder(0.5f));
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("no border"));
            cell.SetBorder(Border.NO_BORDER);
            table.AddCell(cell);
            
            cell = new Cell().Add(new Paragraph("full solid border"));
            table.AddCell(cell);
            
            document.Add(table);
            
            document.Close();
        }
    }
}
