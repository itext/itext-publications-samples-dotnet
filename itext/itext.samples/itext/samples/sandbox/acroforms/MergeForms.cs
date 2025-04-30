using System;
using System.IO;
using iText.Forms;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // MergeForms.cs
    // 
    // This example demonstrates how to merge multiple PDF files containing AcroForm fields
    // while preserving the form fields' functionality in the resulting document.
 
    public class MergeForms
    {
        public static readonly String DEST = "results/sandbox/acroforms/merge_forms.pdf";

        public static readonly String SRC1 = "../../../resources/pdfs/subscribe.pdf";
        public static readonly String SRC2 = "../../../resources/pdfs/state.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MergeForms().ManipulatePdf(DEST);
        }

        public virtual String GetFile1()
        {
            return SRC1;
        }

        public virtual String GetFile2()
        {
            return SRC2;
        }

        protected void ManipulatePdf(String dest)
        {
            PdfReader[] readers =
            {
                new PdfReader(GetFile1()),
                new PdfReader(GetFile2())
            };

            // Method copies the content of all read files to the created resultant pdf
            mergePdfForms(dest, readers);
        }

        private void mergePdfForms(String dest, PdfReader[] readers)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            // This method initializes an outline tree of the document and sets outline mode to true.
            pdfDoc.InitializeOutlines();

            // Copier contains the logic to copy only acroform fields to a new page.
            // PdfPageFormCopier uses some caching logic which can potentially improve performance
            // in case of the reusing of the same instance.
            PdfPageFormCopier formCopier = new PdfPageFormCopier();

            foreach (PdfReader reader in readers)
            {
                PdfDocument readerDoc = new PdfDocument(reader);
                readerDoc.CopyPagesTo(1, readerDoc.GetNumberOfPages(), pdfDoc, formCopier);
                readerDoc.Close();
            }

            pdfDoc.Close();
        }
    }
}