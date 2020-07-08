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
    public class ReadOnlyField2
    {
        public static readonly String DEST = "results/sandbox/acroforms/read_only_field2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ReadOnlyField2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            
            // CreateForm() method creates a temporary document in the memory,
            // which then will be used as a source while writing to a real document
            byte[] content = CreateForm();
            IRandomAccessSource source = new RandomAccessSourceFactory().CreateSource(content);
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(source, new ReaderProperties()), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            form.GetField("text1")

                // Method sets the flag, specifying whether or not the field can be changed.
                .SetReadOnly(true)
                .SetValue("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z");

            form.GetField("text2")
                .SetReadOnly(true)
                .SetValue("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z");

            form.GetField("text3")
                .SetReadOnly(true)
                .SetValue("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z");

            form.GetField("text4")
                .SetReadOnly(true)
                .SetValue("A B C D E F G H I J K L M N O P Q R S T U V W X Y Z");

            pdfDoc.Close();
        }

        public byte[] CreateForm()
        {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(baos));
            PdfFont font = PdfFontFactory.CreateFont();
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            Rectangle rect = new Rectangle(36, 770, 108, 36);
            PdfTextFormField textField1 = PdfFormField.CreateText(pdfDoc, rect, "text1",
                "text1", font, 18f);

            // Being set as true, the field can contain multiple lines of text;
            // if false, the field's text is restricted to a single line.
            textField1.SetMultiline(true);
            form.AddField(textField1);

            rect = new Rectangle(148, 770, 108, 36);
            PdfTextFormField textField2 = PdfFormField.CreateText(pdfDoc, rect, "text2",
                "text2", font, 18f);
            textField2.SetMultiline(true);
            form.AddField(textField2);

            rect = new Rectangle(36, 724, 108, 36);
            PdfTextFormField textField3 = PdfFormField.CreateText(pdfDoc, rect, "text3",
                "text3", font, 18f);
            textField3.SetMultiline(true);
            form.AddField(textField3);

            rect = new Rectangle(148, 727, 108, 33);
            PdfTextFormField textField4 = PdfFormField.CreateText(pdfDoc, rect, "text4",
                "text4", font, 18f);
            textField4.SetMultiline(true);
            form.AddField(textField4);

            pdfDoc.Close();

            return baos.ToArray();
        }
    }
}