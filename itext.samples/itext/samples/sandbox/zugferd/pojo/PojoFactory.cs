/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
/*
* Part of a set of classes based on a sample database.
*/

using System.Collections.Generic;
using SharpHsql;

namespace iText.Samples.Sandbox.Zugferd.Pojo {
    /// <summary>Factory that creates Invoice, Customer, Product, and Item classes.</summary>
    /// <author>Bruno Lowagie (iText Software)</author>
    public class PojoFactory {
        protected internal static PojoFactory factory = null;

        protected internal Dictionary<int, Customer> customerCache = new Dictionary<int, Customer>();

        protected internal Dictionary<int, Product> productCache = new Dictionary<int, Product>();

        private Database db;
        private Channel channel;

        /// <exception cref="System.TypeLoadException"/>
        /// <exception cref="Java.Sql.SQLException"/>
        private PojoFactory() {
            // Create an in memory database by creating with the name "."
            // This has no logging or other disk access
            db = new Database(".");
            // The "sa" user is created by default with no password, so we can connect
            // using this user
            channel = db.Connect("sa", "");
            System.Type.GetType("org.hsqldb.jdbcDriver");
            //connection = DriverManager.GetConnection("jdbc:hsqldb:src/test/resources/zugferd/db/invoices", "SA", "");
            //getCustomer = connection.PrepareStatement("SELECT * FROM Customer WHERE id = ?");
            //getProduct = connection.PrepareStatement("SELECT * FROM Product WHERE id = ?");
            //getItems = connection.PrepareStatement("");
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public static PojoFactory GetInstance() {
            if (factory == null || factory.db.IsShutdown) {
                factory = new PojoFactory();
            }
            return factory;
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public virtual void Close() {
            db.Dispose();
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public virtual IList<Invoice> GetInvoices() {
            IList<Invoice> invoices = new List<Invoice>();
            //Statement stm = connection.CreateStatement();
            //ResultSet rs = stm.ExecuteQuery("SELECT * FROM Invoice");
            //while (rs.Next()) {
            //    invoices.Add(GetInvoice(rs));
            //}
            //stm.Close();
            return invoices;
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public virtual Invoice GetInvoice(Record rs) {
            Invoice invoice = new Invoice();
            //invoice.SetId(rs.GetInt("id"));
            //invoice.SetCustomer(GetCustomer(rs.GetInt("customerid")));
            //IList<Item> items = GetItems(rs.GetInt("id"));
            //invoice.SetItems(items);
            //double total = 0;
            //foreach (Item item in items) {
            //    total += item.GetCost();
            //}
            //invoice.SetTotal(total);
            //invoice.SetInvoiceDate(rs.GetDate("invoicedate"));
            return invoice;
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public virtual Item GetItem(Record rs) {
            Item item = new Item();
            //item.SetItem(rs.GetInt("Item"));
            //Product product = GetProduct(rs.GetInt("ProductId"));
            //item.SetProduct(product);
            //item.SetQuantity(rs.GetInt("Quantity"));
            //item.SetCost(item.GetQuantity() * product.GetPrice());
            return item;
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public virtual Customer GetCustomer(int id) {
            //if (customerCache.Contains(id)) {
            //    return customerCache.Get(id);
            //}
            //getCustomer.SetInt(1, id);
            //ResultSet rs = getCustomer.ExecuteQuery();
            //if (rs.Next()) {
            //    Customer customer = new Customer();
            //    customer.SetId(id);
            //    customer.SetFirstName(rs.GetString("FirstName"));
            //    customer.SetLastName(rs.GetString("LastName"));
            //    customer.SetStreet(rs.GetString("Street"));
            //    customer.SetPostalcode(rs.GetString("Postalcode"));
            //    customer.SetCity(rs.GetString("City"));
            //    customer.SetCountryId(rs.GetString("CountryID"));
            //    customerCache[id] = customer;
            //    return customer;
            //}
            return null;
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public virtual Product GetProduct(int id) {
            //if (productCache.Contains(id)) {
            //    return productCache.Get(id);
            //}
            //getProduct.SetInt(1, id);
            //ResultSet rs = getProduct.ExecuteQuery();
            //if (rs.Next()) {
            //    Product product = new Product();
            //    product.SetId(id);
            //    product.SetName(rs.GetString("Name"));
            //    product.SetPrice(rs.GetDouble("Price"));
            //    product.SetVat(rs.GetDouble("Vat"));
            //    productCache[id] = product;
            //    return product;
            //}
            return null;
        }

        /// <exception cref="Java.Sql.SQLException"/>
        public virtual IList<Item> GetItems(int invoiceid) {
            IList<Item> items = new List<Item>();
            //Result res = db.Execute(String.Format("SELECT * FROM Item WHERE invoiceid = {0}", invoiceid), channel);
            //Record r = res.Root;
            //while (r.res.Next()) {
            //    items.Add(GetItem(rs));
            //}
            return items;
        }
    }
}
