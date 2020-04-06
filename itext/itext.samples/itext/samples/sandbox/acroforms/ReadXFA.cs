/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using iText.Forms;
using iText.Forms.Xfa;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class ReadXFA
    {
        public static readonly String DEST = "results/xml/xfa_example.xml";

        public static readonly String SRC = "../../../resources/pdfs/xfa_invoice_example.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ReadXFA().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            XfaForm xfa = form.GetXfaForm();

            // Get XFA data under datasets/data.
            XElement node = xfa.GetDatasetsNode();
            IEnumerable<XNode> list = node.Nodes();
            foreach (XNode item in list)
            {
                if (item is XElement && "data".Equals(((XElement) item).Name.LocalName))
                {
                    node = (XElement) item;
                    break;
                }
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(dest, settings);
            node.WriteTo(writer);
            writer.Close();

            pdfDoc.Close();
        }
    }
}