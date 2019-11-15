/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Tables
{
    public class LeadingInCell
    {
        public static readonly string DEST = "../../results/sandbox/tables/leading_in_cell.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LeadingInCell().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(UnitValue.CreatePercentArray(1));
            table.SetWidth(UnitValue.CreatePercentValue(60));

            Cell cell = new Cell();
            
            Paragraph p = new Paragraph("paragraph 1: leading 16. Text to force a wrap and check the leading. Ha-ha")
                .SetFixedLeading(16);
            cell.Add(p);

            p = new Paragraph("paragraph 2: leading 32. Text to force a wrap and check the leading. Ha-ha")
                .SetFixedLeading(32);
            cell.Add(p);

            p = new Paragraph("paragraph 3: leading 10. Text to force a wrap and check the leading. Ha-ha")
                .SetFixedLeading(10);
            cell.Add(p);

            p = new Paragraph("paragraph 4: leading 18. Text to force a wrap and check the leading. Ha-ha")
                .SetFixedLeading(18);
            cell.Add(p);

            p = new Paragraph("paragraph 5: leading 40. Text to force a wrap and check the leading. Ha-ha")
                .SetFixedLeading(40);
            cell.Add(p);

            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}