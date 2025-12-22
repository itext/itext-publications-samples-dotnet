using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Pdfua;

namespace iText.Samples.Sandbox.Pdfua {
   
    // PdfUA2TableCaption.cs
    //
    // Example showing how to add table captions in PDF/UA-2 documents.
    // Demonstrates positioning captions at top and bottom of tables.
 
    public class PdfUA2TableCaption {
        public const String DEST = "results/sandbox/pdfua2/pdf_ua_table_caption.pdf";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfUA2TableCaption().ManipulatePdf(DEST);
        }

        public virtual void ManipulatePdf(String dest) {
            try {
                using (PdfDocument pdfDocument = new PdfUADocument(
                           new PdfWriter(dest, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)),
                           new PdfUAConfig(PdfUAConformance.PDF_UA_2, "PdfUA2 Title", "en-US"))) {
                    Document document = new Document(pdfDocument);
                    PdfFont font = PdfFontFactory.CreateFont(FONT, "WinAnsi",
                        PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                    document.SetFont(font);
                    Table tableCaptionBottom = new Table(new float[] { 1, 2, 2 });
                    Paragraph caption = new Paragraph("This is Caption to the bottom").SetBackgroundColor(
                        ColorConstants.GREEN
                    );
                    tableCaptionBottom.SetCaption(new Div().Add(caption), CaptionSide.BOTTOM);
                    tableCaptionBottom.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    tableCaptionBottom.SetWidth(200);
                    tableCaptionBottom.AddHeaderCell("ID");
                    tableCaptionBottom.AddHeaderCell("Name");
                    tableCaptionBottom.AddHeaderCell("Age");
                    for (int i = 1; i <= 5; i++) {
                        tableCaptionBottom.AddCell("ID: " + i);
                        tableCaptionBottom.AddCell("Name " + i);
                        tableCaptionBottom.AddCell("Age: " + (20 + i));
                    }

                    document.Add(tableCaptionBottom);
                    Table captionTopTable = new Table(new float[] { 1, 2, 3 });
                    captionTopTable.SetCaption(new Div().Add(new Paragraph("Caption on top")));
                    captionTopTable.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    captionTopTable.SetWidth(200);
                    captionTopTable.AddHeaderCell("ID");
                    captionTopTable.AddHeaderCell("Name");
                    captionTopTable.AddHeaderCell("Age");
                    for (int i = 1; i <= 5; i++) {
                        captionTopTable.AddCell("ID: " + i);
                        captionTopTable.AddCell("Name " + i);
                        captionTopTable.AddCell("Age: " + (20 + i));
                    }

                    document.Add(captionTopTable);
                }
            }
            catch (System.IO.IOException e) {
                //process io exception
            }
        }
    }
}