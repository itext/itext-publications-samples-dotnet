/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using iText.License;
using iText.Samples.Sandbox.Zugferd.Data;
using iText.Samples.Sandbox.Zugferd.Pojo;
using iText.Zugferd;
using iText.Zugferd.Profiles;

namespace iText.Samples.Sandbox.Zugferd {
    /// <author>Bruno Lowagie</author>
    public class XmlInvoicesComfort {
        public static readonly String DEST = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/test/itext/zugferd/pdfa/comfort{0:00000}.xml";

        /// <exception cref="Java.Sql.SQLException"/>
        /// <exception cref="Javax.Xml.Parsers.ParserConfigurationException"/>
        /// <exception cref="Org.Xml.Sax.SAXException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="Javax.Xml.Transform.TransformerException"/>
        /// <exception cref="iText.Zugferd.Exceptions.DataIncompleteException"/>
        /// <exception cref="iText.Zugferd.Exceptions.InvalidCodeException"/>
        public static void Main(String[] args) {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-multiple-products.xml");
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Directory.CreateDirectory(Directory.GetParent(DEST).FullName);
            PojoFactory factory = PojoFactory.GetInstance();
            IList<Invoice> invoices = factory.GetInvoices();
            InvoiceData invoiceData = new InvoiceData();
            IBasicProfile comfort;
            InvoiceDOM dom;
            foreach (Invoice invoice in invoices) {
                comfort = invoiceData.CreateComfortProfileData(invoice, true);
                dom = new InvoiceDOM(comfort);
                byte[] xml = dom.ToXML();
                FileStream fos = new FileStream(String.Format(DEST, invoice.GetId()), FileMode.Create);
                fos.Write(xml, 0, xml.Length);
                fos.Flush();
                fos.Close();
            }
            factory.Close();
        }
    }
}
