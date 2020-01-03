/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class LinkInPositionedTable
    {
        public static readonly string DEST = "results/sandbox/tables/link_in_positioned_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LinkInPositionedTable().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1));
            table.SetWidth(500);

            Cell cell = new Cell();
            Paragraph p = new Paragraph();

            Link link = new Link("link to top of next page", PdfAction.CreateGoTo("top"));
            p.Add(link);
            cell.Add(p);
            table.AddCell(cell);

            doc.Add(table);
            doc.Add(new AreaBreak());

            // Creates a target that the link leads to
            Paragraph target = new Paragraph("top");
            target.SetDestination("top");
            doc.Add(target);

            doc.Close();
        }
    }
}