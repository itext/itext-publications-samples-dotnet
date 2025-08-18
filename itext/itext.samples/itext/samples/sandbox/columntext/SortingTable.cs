using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Layer;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Columntext
{
   
    // SortingTable.cs
    // 
    // This class demonstrates how to create an interactive table with column sorting using PDF layers.
    // It generates a PDF with three overlapping tables where only one is visible at a time, creating
    // the effect of sorting by different columns when the user clicks the column headers.
 
    public class SortingTable
    {
        public static readonly String DEST = "results/sandbox/columntext/sorting_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SortingTable().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            List<PdfLayer> options = new List<PdfLayer>();
            PdfLayer radiogroup = PdfLayer.CreateTitle("Table", pdfDoc);

            PdfLayer radio1 = new PdfLayer("column1", pdfDoc);

            // To follow a "radio button" paradigm,
            // make sure, that no more that one column is selected.
            radio1.SetOn(true);
            options.Add(radio1);
            radiogroup.AddChild(radio1);

            PdfLayer radio2 = new PdfLayer("column2", pdfDoc);
            radio2.SetOn(false);
            options.Add(radio2);
            radiogroup.AddChild(radio2);

            PdfLayer radio3 = new PdfLayer("column3", pdfDoc);
            radio3.SetOn(false);
            options.Add(radio3);
            radiogroup.AddChild(radio3);

            PdfLayer.AddOCGRadioGroup(pdfDoc, options);

            PdfCanvas pdfCanvas = new PdfCanvas(pdfDoc.AddNewPage());
            for (int i = 0; i < 3; i++)
            {
                pdfCanvas.BeginLayer(options[i]);
                Table table = CreateTable(i + 1, options);
                table.SetFixedPosition(1, 36, 700, 523);
                doc.Add(table);
                pdfCanvas.EndLayer();
            }

            doc.Close();
        }

        private static Table CreateTable(int c, List<PdfLayer> options)
        {
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            for (int j = 0; j < 3; j++)
            {
                table.AddCell(CreateHeaderCell(j, options));
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    table.AddCell(CreateCell(i + 1, j + 1, c));
                }
            }

            return table;
        }

        private static Cell CreateHeaderCell(int c, List<PdfLayer> options)
        {
            IList<PdfDictionary> dictList = new List<PdfDictionary>();
            dictList.Add(options[c].GetPdfObject());
            IList<PdfActionOcgState> list = new List<PdfActionOcgState>();
            list.Add(new PdfActionOcgState(PdfName.ON, dictList));

            // Create an action to set option content group state.
            PdfAction action = PdfAction.CreateSetOcgState(list, true);
            Link link = new Link("Column " + (c + 1), action);
            Cell cell = new Cell().Add(new Paragraph(link));
            return cell;
        }

        private static Cell CreateCell(int i, int j, int c)
        {
            Cell cell = new Cell();
            cell.Add(new Paragraph(String.Format("row {0}; column {1}", i, j)));

            // If the current column is selected, then set different background color
            if (j == c)
            {
                cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            }

            return cell;
        }
    }
}