/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.License;
using iText.Samples.Sandbox.Zugferd.Data;
using iText.Samples.Sandbox.Zugferd.Pojo;
using iText.Zugferd;
using iText.Zugferd.Profiles;
using java.io;
using javax.xml.transform;
using javax.xml.transform.stream;

namespace iText.Samples.Sandbox.Zugferd
{
    /// <author>Bruno Lowagie</author>
    public class HtmlInvoicesComfort {
        public const String DEST = "./target/main/resources/zugferd/html/comfort{0:00000}.html";

        public const String XSL = "./src/main/resources/xml/invoice.xsl";

        public const String CSS = "./src/main/resources/data/invoice.css";

        public const String LOGO = "./src/main/resources/img/logo.png";

        /// <exception cref="SQLException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="ParserConfigurationException"/>
        /// <exception cref="SAXException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        /// <exception cref="TransformerException"/>
        public static void Main(String[] args) {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-multiple-products.xml");
            Directory.CreateDirectory(Directory.GetParent(DEST).FullName);
            CopyFile(CSS, Directory.GetParent(DEST).FullName + Path.DirectorySeparatorChar + Path.GetFileName(CSS));
            CopyFile(LOGO, Directory.GetParent(DEST).FullName + Path.DirectorySeparatorChar + Path.GetFileName(LOGO));
            HtmlInvoicesComfort app = new HtmlInvoicesComfort();
            PojoFactory factory = PojoFactory.GetInstance();
            IList<Invoice> invoices = factory.GetInvoices();
            foreach (Invoice invoice in invoices) {
                app.CreateHtml(invoice, new FileWriter(String.Format(DEST, invoice.GetId())));
            }
            factory.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="ParserConfigurationException"/>
        /// <exception cref="SAXException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        /// <exception cref="TransformerException"/>
        public virtual void CreateHtml(Invoice invoice, FileWriter writer) {
            IComfortProfile comfort = new InvoiceData().CreateComfortProfileData(invoice);
            InvoiceDOM dom = new InvoiceDOM(comfort);
            StreamSource xml = new StreamSource(new ByteArrayInputStream(dom.ToXML()));
            StreamSource xsl = new StreamSource(new FileInputStream(XSL));
            TransformerFactory factory = TransformerFactory.newInstance();
            Transformer transformer = factory.newTransformer(xsl);
            transformer.transform(xml, new StreamResult(writer));
            writer.flush();
            writer.close();
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
