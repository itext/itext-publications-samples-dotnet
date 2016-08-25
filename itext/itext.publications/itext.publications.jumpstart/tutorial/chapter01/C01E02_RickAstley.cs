/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Tutorial.Chapter01 {
    /// <summary>Simple List example.</summary>
    public class C01E02_RickAstley {
        public const String DEST = "results/chapter01/rick_astley.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            if (!file.Directory.Exists) file.Directory.Create();
            new C01E02_RickAstley().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            Document document = new Document(pdf);
            // Create a PdfFont
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            // Add a Paragraph
            document.Add(new Paragraph("iText is:").SetFont(font));
            // Create a List
            List list = new List().SetSymbolIndent(12).SetListSymbol("\u2022").SetFont(font);
            // Add ListItem objects
            list.Add(new ListItem("Never gonna give you up")).Add(new ListItem("Never gonna let you down")).Add(new ListItem
                ("Never gonna run around and desert you")).Add(new ListItem("Never gonna make you cry")).Add(new ListItem
                ("Never gonna say goodbye")).Add(new ListItem("Never gonna tell a lie and hurt you"));
            // Add the list
            document.Add(list);
            //Close document
            document.Close();
        }
    }
}
