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
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class RemoveListSymbol
    {
        public static readonly string DEST = "results/sandbox/objects/remove_list_symbol.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new RemoveListSymbol().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph paragraph = new Paragraph("A list without list symbol");
            doc.Add(paragraph);

            List list = new List();
            
            // List symbol replaced, not deleted
            list.SetListSymbol("");
            list.Add(new ListItem("Item 1"));
            list.Add(new ListItem("Item 2"));
            list.Add(new ListItem("Item 3"));

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.SetMarginTop(5);
            table.AddCell(new Cell().Add(new Paragraph("List:")));
            table.AddCell(list);
            doc.Add(table);

            doc.Close();
        }
    }
}