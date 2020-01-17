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
    public class TableTab
    {
        public static readonly string DEST = "results/sandbox/objects/table_tab.pdf";
        public static readonly string[][] DATA =
        {
            new string[] {"John Edward Jr.", "AAA"},
            new string[] {"Pascal Einstein W. Alfi", "BBB"},
            new string[] {"St. John", "CCC"}
        };

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new TableTab().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(CreateParagraphWithTab("Name: ", DATA[0][0], DATA[0][1]));
            doc.Add(CreateParagraphWithTab("Surname: ", DATA[1][0], DATA[1][1]));
            doc.Add(CreateParagraphWithTab("School: ", DATA[2][0], DATA[2][1]));

            doc.Close();
        }

        private static Paragraph CreateParagraphWithTab(string key, string value1, string value2)
        {
            Paragraph p = new Paragraph();
            p.AddTabStops(new TabStop(200f, TabAlignment.LEFT));
            p.Add(key);
            p.Add(value1);
            p.Add(new Tab());
            p.Add(value2);
            return p;
        }
    }
}