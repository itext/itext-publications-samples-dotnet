using System;
using System.IO;
using System.Text;
using System.Xml;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillWithUnderline
    {
        public static readonly String DEST = "results/sandbox/acroforms/fill_with_underline.pdf";

        public static readonly String SRC = "../../../resources/pdfs/form.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillWithUnderline().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            // If no fields have been explicitly included, then all fields are flattened.
            // Otherwise only the included fields are flattened.
            form.FlattenFields();

            Rectangle pos = form.GetField("Name").GetWidgets()[0].GetRectangle().ToRectangle();

            // Custom parser gets position of the form field
            // to fill in the document with the parsed content.
            CustomXmlParser parser = new CustomXmlParser(doc, pos);
            parser.Parse("<root><div>Bruno <u>Lowagie</u></div></root>");

            pdfDoc.Close();
        }

        private class CustomXmlParser
        {
            protected Document document;
            protected Rectangle position;
            protected Paragraph paragraph;

            // If isUnderlined flag is true, then parsed text should be underlined.
            protected bool isUnderlined;

            public CustomXmlParser(Document document, Rectangle position)
            {
                this.document = document;
                this.position = position;
                paragraph = new Paragraph();
                isUnderlined = false;
            }

            public void Parse(String line)
            {
                byte[] content = Encoding.UTF8.GetBytes(line);
                MemoryStream stream = new MemoryStream(content);
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            
                            // If the node type is opening tag &lt;u&gt;, then set isUnderlined flag to true.
                            HandleStartElement(reader);
                        }
                        else if (reader.NodeType == XmlNodeType.Text)
                        {
                            
                            // Creates a iText.Layout.Element.Text instance from the text,
                            // got from the passed reader, and wraps it if not empty with a paragraph.
                            HandleText(reader);
                        }
                        else if (reader.NodeType == XmlNodeType.EndElement)
                        {
                            
                            // This method handles closing tags &lt;/div&gt; and &lt;/u&gt;:
                            // if the node type is closing tag &lt;/div&gt;, then add the parsed text to the document
                            // if the node type is closing tag &lt;/u&gt;, then set isUnderlined flag to false.
                            HandleEndElement(reader);
                        }
                    }
                }
            }

            private void HandleStartElement(XmlReader reader)
            {
                if (reader.Name == "u")
                {
                    isUnderlined = true;
                }
            }

            private void HandleEndElement(XmlReader reader)
            {
                if (reader.Name == "div")
                {
                    document.Add(paragraph.SetFixedPosition(position.GetLeft(),
                        position.GetBottom(), position.GetWidth()));

                    // Set the position of the next form field.
                    position.MoveDown(22);
                    paragraph = new Paragraph();
                }
                else if (reader.Name == "u")
                {
                    isUnderlined = false;
                }
            }

            private void HandleText(XmlReader reader)
            {
                Text text = new Text(reader.Value);
                if (isUnderlined)
                {
                    text.SetUnderline();
                }

                if (0 != text.GetText().Length)
                {
                    paragraph.Add(text);
                }
            }
        }
    }
}