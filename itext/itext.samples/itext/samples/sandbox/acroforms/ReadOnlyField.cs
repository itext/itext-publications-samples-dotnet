using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Source;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class ReadOnlyField
    {
        public static readonly String DEST = "results/sandbox/acroforms/read_only_field.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ReadOnlyField().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            
            // CreateForm() method creates a temporary document in the memory,
            // which then will be used as a source while writing to a real document
            byte[] content = CreateForm();
            IRandomAccessSource source = new RandomAccessSourceFactory().CreateSource(content);
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(source, new ReaderProperties()), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            form.GetField("text")

                // Method sets the flag, specifying whether or not the field can be changed.
                .SetReadOnly(true)
                .SetValue("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z");

            pdfDoc.Close();
        }

        public byte[] CreateForm()
        {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(baos));
            PdfFont font = PdfFontFactory.CreateFont();
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            Rectangle rect = new Rectangle(36, 770, 104, 36);
            PdfTextFormField textField = new TextFormFieldBuilder(pdfDoc, "text").SetWidgetRectangle(rect).CreateText();
            textField.SetValue("text").SetFont(font).SetFontSize(20f);

            // Being set as true, the field can contain multiple lines of text;
            // if false, the field's text is restricted to a single line.
            textField.SetMultiline(true);
            form.AddField(textField);

            pdfDoc.Close();

            return baos.ToArray();
        }
    }
}