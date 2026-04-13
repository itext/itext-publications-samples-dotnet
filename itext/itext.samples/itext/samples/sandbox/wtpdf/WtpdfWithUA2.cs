using iText.Commons.Utils;
using iText.Forms.Form;
using iText.Forms.Form.Element;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Tagging;
using iText.Kernel.XMP;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfua.Wtpdf;
using NUnit.Framework;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace iText.Samples.Sandbox.Wtpdf {

    //WtpdfWithUA2.cs
    //
    //Example showing how to create document compliant with Pdf/UA2 and well tagged pdf for reuse and accessibility.

    internal class WtpdfWithUA2 {
        public static readonly String DEST = "results/sandbox/wtpdf/pdf_wtpdfPdfUA2.pdf";
        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";
        public static readonly String SOURCE_FOLDER = "../../../resources/wtpdf/";
        public static readonly String IMAGE_PATH = "../../../resources/img/itext.png";

        public static void Main(String[] args) {
            var file = new FileInfo(DEST);
            file.Directory.Create();

            new WtpdfWithUA2().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest) {
            PdfWriter writer = new PdfWriter(FileUtil.GetFileOutputStream(dest), new WriterProperties().
                SetPdfVersion(PdfVersion.PDF_2_0));

            List<WellTaggedPdfConformance> conformances = new List<WellTaggedPdfConformance>();
            conformances.Add(WellTaggedPdfConformance.FOR_ACCESSIBILITY);
            conformances.Add(WellTaggedPdfConformance.FOR_REUSE);
            PdfDocument pdf = new WellTaggedPdfDocument(writer, new WellTaggedPdfConfig(conformances,
                    "well tagged pdf and pdf UA2 compliant document", "en-US"));

            // Setup the metadata for a PDF/UA-2 document
            var bytes = System.IO.File.ReadAllBytes(Path.Combine(SOURCE_FOLDER + "simplePdfUA2Wtpdf.xmp"));
            var xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
            pdf.SetXmpMetadata(xmpMeta);

            //add content
            AddContent(pdf);
        }

        private void AddContent(PdfDocument pdf) {
            using (Document document = new Document(pdf)) {
                PdfFont font = PdfFontFactory.CreateFont(FONT, "WinAnsi", PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                document.SetFont(font);

                //Create a paragraph
                Paragraph paragraph = new Paragraph().Add("This document is compliant with well tagged pdf and pdf UA2! " +
                        "Here, look at this in a list:");
                document.Add(paragraph);

                //Create a list
                iText.Layout.Element.List list = new iText.Layout.Element.List();
                list.Add("compliant with well tagged pdf for reuse");
                list.Add("compliant with well tagged pdf for accessibility");
                list.Add("compliant with PdfUA2");
                document.Add(list);

                //Create a paragraph
                Paragraph paragraph2 = new Paragraph().Add("here is also a table");
                document.Add(paragraph2);

                //Create a table
                Table table = new Table(new float[] { 1, 1, 1 });
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                table.SetWidth(300);
                for (int i = 0; i < 3; i++) {
                    table.AddHeaderCell(new Paragraph("Column " + (i + 1)));
                }

                for (int i = 0; i < 3; i++) {
                    table.AddCell(new Paragraph("element " + (i + 1) + " in column 1"));
                    table.AddCell(new Paragraph("element " + (i + 1) + " in column 2"));
                    table.AddCell(new Paragraph("element " + (i + 1) + " in column 3"));
                }
                document.Add(table);

                //Create a paragraph
                Paragraph paragraph3 = new Paragraph().Add("and an image");
                document.Add(paragraph3);

                //Create an image
                Image img = new Image(ImageDataFactory.Create(IMAGE_PATH));
                img.GetAccessibilityProperties()
                        .SetAlternateDescription("Company logo");
                document.Add(img);

                // Creating an InputField
                InputField formInputField = new InputField("form input field");
                formInputField.SetProperty(FormProperty.FORM_FIELD_VALUE, "an input field also here");
                formInputField.GetAccessibilityProperties().SetRole(StandardRoles.ANNOT);
                formInputField.GetAccessibilityProperties().SetAlternateDescription("input field");
                formInputField.SetInteractive(true);
                document.Add(formInputField);
            }
        }
    }
}
