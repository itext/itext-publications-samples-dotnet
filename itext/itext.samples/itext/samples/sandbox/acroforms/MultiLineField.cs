using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // MultiLineField.cs
    // 
    // This example demonstrates how to create a PDF form with a multiline text field
    // and populate it with content that spans multiple lines before flattening the form.
 
    public class MultiLineField
    {
        public static readonly String DEST = "results/sandbox/acroforms/multi_line_field.pdf";

        public static readonly String FIELD_NAME = "text";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MultiLineField().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            
            // createForm() method creates a temporary document in the memory,
            // which then will be used as a source while writing to a real document
            byte[] content = CreateForm();
            IRandomAccessSource source = new RandomAccessSourceFactory().CreateSource(content);
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(source, new ReaderProperties()), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            form.GetField(FIELD_NAME)
                .SetValue("A B C D E F\nG H I J K L M N\nO P Q R S T U\r\nV W X Y Z\n\nAlphabet street");

            // If no fields have been explicitly included, then all fields are flattened.
            // Otherwise only the included fields are flattened.
            form.FlattenFields();

            pdfDoc.Close();
        }

        public virtual byte[] CreateForm()
        {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(baos));
            Rectangle rect = new Rectangle(36, 720, 108, 86);

            PdfTextFormField textFormField =
                new TextFormFieldBuilder(pdfDoc, FIELD_NAME).SetWidgetRectangle(rect).CreateText();
            textFormField.SetValue("text");

            // Being set as true, the field can contain multiple lines of text;
            // if false, the field's text is restricted to a single line.
            textFormField.SetMultiline(true);
            PdfFormCreator.GetAcroForm(pdfDoc, true).AddField(textFormField);

            pdfDoc.Close();

            return baos.ToArray();
        }
    }
}