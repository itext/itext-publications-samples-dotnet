/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class ListInCell
    {
        public static readonly string DEST = "results/sandbox/tables/list_in_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ListInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());

            // This is how not to do it (but it works anyway):

            // Create a list:
            List list = new List();
            list.Add(new ListItem("Item 1"));
            list.Add(new ListItem("Item 2"));
            list.Add(new ListItem("Item 3"));

            // Wrap this list in a paragraph 
            Paragraph paragraph = new Paragraph();
            paragraph.Add(list);

            // Add this paragraph to a cell
            Cell paragraphCell = new Cell();
            paragraphCell.Add(paragraph);

            // Add the cell to a table
            Table paragraphTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            paragraphTable.SetMarginTop(5);
            paragraphTable.AddCell("List wrapped in a paragraph:");
            paragraphTable.AddCell(paragraphCell);

            // Add this nested table to the document
            doc.Add(new Paragraph("A list, wrapped in a paragraph, wrapped in a cell, "
                                  + "wrapped in a table, wrapped in a phrase:"));
            paragraphTable.SetMarginTop(5);
            doc.Add(paragraphTable);

            // This is how to do it:

            // Add the list directly to a cell
            Cell cell = new Cell();
            cell.Add(list);

            // Add the cell to the table
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetMarginTop(5);
            table.AddCell("List placed directly into cell");
            table.AddCell(cell);

            // Add the table to the document
            doc.Add(new Paragraph("A list, wrapped in a cell, wrapped in a table:"));
            doc.Add(table);

            // Avoid adding tables to paragraph (but it works anyway):
            Paragraph tableWrapper = new Paragraph();
            tableWrapper.SetMarginTop(0);
            tableWrapper.Add(table);
            doc.Add(new Paragraph("A list, wrapped in a cell, wrapped in a table, wrapped in a paragraph:"));
            
            doc.Add(tableWrapper);

            doc.Close();
        }
    }
}