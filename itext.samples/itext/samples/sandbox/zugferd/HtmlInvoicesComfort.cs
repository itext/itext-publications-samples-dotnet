/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Samples.Sandbox.Zugferd.Data;
using iText.Samples.Sandbox.Zugferd.Pojo;
using iText.Zugferd;
using iText.Zugferd.Profiles;

// TODO string format
namespace iText.Samples.Sandbox.Zugferd
{
    /// <author>Bruno Lowagie</author>
    public class HtmlInvoicesComfort {
        public const String DEST = "./target/main/resources/zugferd/html/comfort%05d.html";

        public const String XSL = "./src/main/resources/xml/invoice.xsl";

        public const String CSS = "./src/main/resources/data/invoice.css";

        public const String LOGO = "./src/main/resources/img/logo.png";

        /// <exception cref="Java.Sql.SQLException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        /// <exception cref="Javax.Xml.Transform.TransformerException"/>
        public static void Main(String[] args) {
            Directory.CreateDirectory(Directory.GetParent(DEST).FullName);
            CopyFile(CSS, Directory.GetParent(DEST).FullName + Path.DirectorySeparatorChar + Path.GetFileName(CSS));
            CopyFile(LOGO, Directory.GetParent(DEST).FullName + Path.DirectorySeparatorChar + Path.GetFileName(LOGO));
            HtmlInvoicesComfort app = new HtmlInvoicesComfort();
            PojoFactory factory = PojoFactory.GetInstance();
            IList<Invoice> invoices = factory.GetInvoices();
            foreach (Invoice invoice in invoices) {
                // TODO
                //app.CreateHtml(invoice, new FileWriter(String.Format(DEST, invoice.GetId())));
            }
            factory.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        /// <exception cref="Javax.Xml.Transform.TransformerException"/>
        public virtual void CreateHtml(Invoice invoice, TextWriter writer) {
            IComfortProfile comfort = new InvoiceData().CreateComfortProfileData(invoice);
            InvoiceDOM dom = new InvoiceDOM(comfort);
            // TODO
            //StreamSource xml = new StreamSource(new MemoryStream(dom.ToXML()));
            //StreamSource xsl = new StreamSource(new FileInfo(XSL));
            //TransformerFactory factory = TransformerFactory.NewInstance();
            //Transformer transformer = factory.NewTransformer(xsl);
            //transformer.Transform(xml, new StreamResult(writer));
            writer.Flush();
            writer.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        private static void CopyFile(String source, String dest) {
            System.IO.Stream input = new FileStream(source, FileMode.Open, FileAccess.Read);
            System.IO.Stream output = new FileStream(dest, FileMode.Create);
            byte[] buf = new byte[1024];
            int bytesRead;
            while ((bytesRead = input.Read(buf, 0, buf.Length)) > 0) {
                output.Write(buf, 0, bytesRead);
            }
        }
    }
}
