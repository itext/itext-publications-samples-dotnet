using System;
using System.IO;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using iText.Layout.Element;
using iText.Layout;
using iText.Kernel.Exceptions;

namespace iText.Samples.Sandbox.Pdfua
{
    public class PdfUA2 {
        public const String DEST = "results/sandbox/pdfua2/pdf_ua.pdf";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public const String UA_XMP = "../../../resources/xml/pdf_ua_xmp.xmp";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfUA2().ManipulatePdf(DEST);
        }

        public virtual void ManipulatePdf(String dest) {
            try {
                using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest, new WriterProperties().SetPdfVersion(
                    PdfVersion.PDF_2_0)))) {
                    Document document = new Document(pdfDocument);
                    byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(UA_XMP));
                    XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
                    pdfDocument.SetXmpMetadata(xmpMeta);
                    //TAGGED PDF
                    //Make document tagged
                    pdfDocument.SetTagged();
                    pdfDocument.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
                    pdfDocument.GetCatalog().SetLang(new PdfString("en-US"));
                    PdfDocumentInfo info = pdfDocument.GetDocumentInfo();
                    info.SetTitle("PdfUA2 Title");
                    //PDF/UA
                    //Embed font
                    PdfFont font = PdfFontFactory.CreateFont(FONT, "WinAnsi", PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                    Paragraph paragraph = new Paragraph("Hello PdfUA2").SetFont(font);
                    byte[] byteMetaData = pdfDocument.GetXmpMetadataBytes();
                    //PDF/UA
                    //Get string xmp metadata representation
                    String documentMetaData = iText.Commons.Utils.JavaUtil.GetStringForBytes(byteMetaData);
                    document.Add(paragraph);
                }
            }
            catch (XMPException e) {
                //process xmp exception
            }
            catch (System.IO.IOException e) {
                //process io exception
            }
        }
    }
}