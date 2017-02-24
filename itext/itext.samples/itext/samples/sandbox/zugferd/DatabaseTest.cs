/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV

*/
using System;
using System.Collections.Generic;
using iText.Samples.Sandbox.Zugferd.Pojo;

namespace iText.Samples.Sandbox.Zugferd {
    /// <summary>A simple example to test the database</summary>
    public class DatabaseTest {
        /// <exception cref="Java.Sql.SQLException"/>
        public static void Main(String[] args) {
            PojoFactory factory = PojoFactory.GetInstance();
            IList<Invoice> invoices = factory.GetInvoices();
            foreach (Invoice invoice in invoices) {
                System.Console.Out.WriteLine(invoice.ToString());
            }
            factory.Close();
        }
    }
}
