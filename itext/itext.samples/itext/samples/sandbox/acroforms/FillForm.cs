using System;
using System.IO;
using iText.Forms;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillForm
    {
        public static readonly String DEST = "results/sandbox/acroforms/fill_form.pdf";

        public static readonly String SRC = "../../../resources/pdfs/CertificateOfExcellence.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillForm().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            form.GetField("course").SetValue("Copying and Pasting from StackOverflow");
            form.GetField("name").SetValue("Some dude on StackOverflow");
            form.GetField("date").SetValue("April 10, 2016");
            form.GetField("description").SetValue(
                "In this course, people consistently ignore the existing documentation completely. "
                + "They are encouraged to do no effort whatsoever, but instead post their questions " 
                + "on StackOverflow. It would be a mistake to refer to people completing this course "
                + "as developers. A better designation for them would be copy/paste artist. " 
                + "Only in very rare cases do these people know what they are actually doing. "
                + "Not a single student has ever learned anything substantial during this course.");

            // If no fields have been explicitly included, then all fields are flattened.
            // Otherwise only the included fields are flattened.
            form.FlattenFields();

            pdfDoc.Close();
        }
    }
}