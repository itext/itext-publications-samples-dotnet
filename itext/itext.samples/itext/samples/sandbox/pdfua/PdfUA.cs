using System;
using System.IO;
using iText.Forms.Form.Element;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfua;

namespace iText.Samples.Sandbox.Pdfua
{
   
    // PdfUA.cs
    //
    // Example showing how to create PDF/UA-1 accessible documents.
    // Demonstrates images, lists, tables, headings, and form fields with tags.
 
    public class PdfUA
    {
        public static readonly string DEST = "results/sandbox/pdfua/pdf_ua.pdf";

        public static readonly String DOG = "../../../resources/img/dog.bmp";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";

        public static readonly String FOX = "../../../resources/img/fox.bmp";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfUA().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfUADocument pdfDoc = new PdfUADocument(new PdfWriter(dest),
                new PdfUAConfig(PdfUAConformance.PDF_UA_1, "Some title", "en-US"));
            Document document = new Document(pdfDoc, PageSize.A4.Rotate());


            // PDF UA requires font to be embedded, this is the way we want to do it
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI,
                PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);
            document.SetFont(font);

            Paragraph p = new Paragraph();
            // You can also set it on individual elements if you want to
            p.SetFont(font);
            p.Add("The quick brown ");
            document.Add(p);


            // Images require to have an alternative description
            Image img = new Image(ImageDataFactory.Create(FOX));
            img.GetAccessibilityProperties().SetAlternateDescription("Fox");
            document.Add(img);

            Paragraph p2 = new Paragraph(" jumps over the lazy ");
            document.Add(p2);

            Image img2 = new Image(ImageDataFactory.Create(DOG));
            img2.GetAccessibilityProperties().SetAlternateDescription("Dog");
            document.Add(img2);


            Paragraph p3 = new Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n").SetFontSize(20);
            document.Add(p3);

            // Let's add a list
            List list = new List().SetFontSize(20);
            list.Add(new ListItem("quick"));
            list.Add(new ListItem("brown"));
            list.Add(new ListItem("fox"));
            list.Add(new ListItem("jumps"));
            list.Add(new ListItem("over"));
            list.Add(new ListItem("the"));
            list.Add(new ListItem("lazy"));
            list.Add(new ListItem("dog"));
            document.Add(list);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            AddTables(document);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

            AddFormfields(document);

            document.Close();
        }


        private void AddTables(Document document)
        {
            // Add a table with the automatic column scope
            Table table = new Table(2);
            Cell headerCell1 = new Cell().Add(new Paragraph("Header 1"));
            headerCell1.GetAccessibilityProperties().SetRole(StandardRoles.TH);
            Cell headerCell2 = new Cell().Add(new Paragraph("Header 2"));
            headerCell2.GetAccessibilityProperties().SetRole(StandardRoles.TH);

            table.AddHeaderCell(headerCell1);
            table.AddHeaderCell(headerCell2);

            table.AddCell(new Cell().Add(new Paragraph("data 1")));
            table.AddCell(new Cell().Add(new Paragraph("data 2")));

            document.Add(table);
            document.Add(new Paragraph("\n\n"));
            // Add a table with row scope
            Table table2 = new Table(2);
            Cell headerCell3 = new Cell().Add(new Paragraph("Header 1"));
            PdfStructureAttributes attributes = new PdfStructureAttributes("Table");
            attributes.AddEnumAttribute("Scope", "Row");
            headerCell3.GetAccessibilityProperties().AddAttributes(attributes);
            headerCell3.GetAccessibilityProperties().SetRole(StandardRoles.TH);

            Cell headerCell4 = new Cell().Add(new Paragraph("Header 2"));
            headerCell4.GetAccessibilityProperties().SetRole(StandardRoles.TH);
            PdfStructureAttributes attributes2 = new PdfStructureAttributes("Table");
            attributes2.AddEnumAttribute("Scope", "Row");
            headerCell4.GetAccessibilityProperties().AddAttributes(attributes2);


            table2.AddCell(headerCell3);
            table2.AddCell(new Cell().Add(new Paragraph("data 1")));
            table2.AddCell(headerCell4);
            table2.AddCell(new Cell().Add(new Paragraph("data 2")));
            document.Add(table2);

            // For complex tables you can also make use of Id's
            Table table3 = new Table(2);
            for (int i = 0; i < 4; i++)
            {
                Cell cell = new Cell().Add(new Paragraph("data " + i));
                PdfStructureAttributes cellAttributes = new PdfStructureAttributes("Table");
                PdfArray headers = new PdfArray();
                headers.Add(new PdfString("header_id_1"));
                cellAttributes.GetPdfObject().Put(PdfName.Headers, headers);
                cell.GetAccessibilityProperties().AddAttributes(cellAttributes);
                table3.AddCell(cell);
            }

            Cell headerCell5 = new Cell(1, 2).Add(new Paragraph("Header 1"));
            headerCell5.GetAccessibilityProperties().SetRole(StandardRoles.TH);
            headerCell5.GetAccessibilityProperties().SetStructureElementIdString("header_id_1");
            PdfStructureAttributes headerAttributes = new PdfStructureAttributes("Table");
            headerAttributes.AddEnumAttribute("Scope", "None");
            headerCell5.GetAccessibilityProperties().AddAttributes(headerAttributes);
            table3.AddCell(headerCell5);
            document.Add(table3);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            // Let's add some headings
            Paragraph h1 = new Paragraph("Heading 1").SetFontSize(20);
            h1.GetAccessibilityProperties().SetRole(StandardRoles.H1);
            document.Add(h1);
            Paragraph h2 = new Paragraph("Heading 2").SetFontSize(18);
            h2.GetAccessibilityProperties().SetRole(StandardRoles.H2);
            document.Add(h2);
            Paragraph h3 = new Paragraph("Heading 3").SetFontSize(16);
            h3.GetAccessibilityProperties().SetRole(StandardRoles.H3);
            document.Add(h3);
        }

        private void AddFormfields(Document document)
        {
            // Formfields are also possible
            InputField field = new InputField("name");
            field.GetAccessibilityProperties().SetAlternateDescription("Name");
            field.SetValue("John Doe");
            field.SetBackgroundColor(ColorConstants.CYAN);
            document.Add(field);

            InputField field2 = new InputField("email");
            field2.GetAccessibilityProperties().SetAlternateDescription("Email");
            field2.SetValue("sales@apryse.com");
            field2.SetInteractive(true);
            field2.SetBackgroundColor(ColorConstants.YELLOW);
            document.Add(field2);
        }
    }
}