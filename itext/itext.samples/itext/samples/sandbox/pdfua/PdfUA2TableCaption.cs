using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Pdfua 
{
    public class PdfUA2TableCaption {
        public const String DEST = "results/sandbox/pdfua2/pdf_ua_table_caption.pdf";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public const String UA_XMP = "../../../resources/xml/pdf_ua_xmp.xmp";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfUA2TableCaption().ManipulatePdf(DEST);
        }

        public virtual void ManipulatePdf(String dest) {
            try {
                using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest, new WriterProperties().SetPdfVersion(
                    PdfVersion.PDF_2_0)))) {
                    Document document = new Document(pdfDocument);
                    PdfFont font = PdfFontFactory.CreateFont(FONT, "WinAnsi", PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                    document.SetFont(font);
                    byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(UA_XMP));
                    XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
                    pdfDocument.SetXmpMetadata(xmpMeta);
                    pdfDocument.SetTagged();
                    pdfDocument.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
                    pdfDocument.GetCatalog().SetLang(new PdfString("en-US"));
                    PdfDocumentInfo info = pdfDocument.GetDocumentInfo();
                    info.SetTitle("PdfUA2 Title");
                    Table tableCaptionBottom = new Table(new float[] { 1, 2, 2 });
                    Paragraph caption = new Paragraph("This is Caption to the bottom").SetBackgroundColor(ColorConstants.GREEN
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
            catch (XMPException e) {
                //process xmp exception
            }
        }
    }
}
