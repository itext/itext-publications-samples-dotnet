using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Tagging
{
    public class AddAltTags
    {
        public static readonly string DEST = "results/sandbox/tagging/add_alt_tags.pdf";

        public static readonly String SRC = "../../../resources/pdfs/no_alt_attribute.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddAltTags().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfDictionary catalog = pdfDoc.GetCatalog().GetPdfObject();

            // Gets the root dictionary
            PdfDictionary structTreeRoot = catalog.GetAsDictionary(PdfName.StructTreeRoot);
            Manipulate(structTreeRoot);

            pdfDoc.Close();
        }

        private void Manipulate(PdfDictionary element)
        {
            if (element == null)
            {
                return;
            }

            // If an element is a figure, adds an /Alt entry.
            if (PdfName.Figure.Equals(element.Get(PdfName.S)))
            {
                element.Put(PdfName.Alt, new PdfString("Figure without an Alt description"));
            }

            PdfArray kids = element.GetAsArray(PdfName.K);

            if (kids == null)
            {
                return;
            }

            // Loops over all the kids
            for (int i = 0; i < kids.Size(); i++)
            {
                Manipulate(kids.GetAsDictionary(i));
            }
        }
    }
}