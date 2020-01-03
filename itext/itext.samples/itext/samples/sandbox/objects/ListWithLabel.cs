/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class ListWithLabel
    {
        public static readonly string DEST = "results/sandbox/objects/list_with_label.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ListWithLabel().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(new float[] {1, 10});
            table.SetWidth(200);
            table.SetHorizontalAlignment(HorizontalAlignment.LEFT);
            
            Cell cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            cell.Add(new Paragraph("Label"));
            table.AddCell(cell);

            cell = new Cell();
            cell.SetBorder(Border.NO_BORDER);
            List list = new List();
            list.Add(new ListItem("Value 1"));
            list.Add(new ListItem("Value 2"));
            list.Add(new ListItem("Value 3"));
            cell.Add(list);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}