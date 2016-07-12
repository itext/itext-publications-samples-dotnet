/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
/*
* Part of a set of classes based on a sample database.
*/

using System;
using System.Collections.Generic;
using java.sql;

namespace iText.Samples.Sandbox.Zugferd.Pojo {
    /// <summary>Factory that creates Invoice, Customer, Product, and Item classes.</summary>
    /// <author>Bruno Lowagie (iText Software)</author>
    public class PojoFactory {
        protected internal static PojoFactory factory = null;

        protected Connection connection;

        protected internal Dictionary<int, Customer> customerCache = new Dictionary<int, Customer>();

        protected internal Dictionary<int, Product> productCache = new Dictionary<int, Product>();

        protected PreparedStatement getCustomer;
        protected PreparedStatement getProduct;
        protected PreparedStatement getItems;

        private static readonly String DB = NUnit.Framework.TestContext.CurrentContext.TestDirectory + " /../../resources/zugferd/db/invoices";

        /// <exception cref="System.TypeLoadException"/>
        /// <exception cref="SQLException"/>
        private PojoFactory() {
            DriverManager.registerDriver(new org.hsqldb.jdbcDriver());
            connection = DriverManager.getConnection("jdbc:hsqldb:" + DB, "SA", "");
            getCustomer = connection.prepareStatement("SELECT * FROM Customer WHERE id = ?");
            getProduct = connection.prepareStatement("SELECT * FROM Product WHERE id = ?");
            getItems = connection.prepareStatement("SELECT * FROM Item WHERE invoiceid = ?");
        }

        /// <exception cref="SQLException"/>
        public static PojoFactory GetInstance() {
            if (factory == null || factory.connection.isClosed()) {
                factory = new PojoFactory();
            }
            return factory;
        }

        /// <exception cref="SQLException"/>
        public virtual void Close() {
            connection.close();
        }

        /// <exception cref="SQLException"/>
        public virtual IList<Invoice> GetInvoices() {
            List<Invoice> invoices = new List<Invoice>();
            Statement stm = connection.createStatement();
            ResultSet rs = stm.executeQuery("SELECT * FROM Invoice");
            while (rs.next())
            {
                invoices.Add(GetInvoice(rs));
            }
            stm.close();
            return invoices;
        }

        /// <exception cref="SQLException"/>
        public virtual Invoice GetInvoice(ResultSet rs) {
            Invoice invoice = new Invoice();
            invoice.SetId(rs.getInt("id"));
            invoice.SetCustomer(GetCustomer(rs.getInt("customerid")));
            IList<Item> items = GetItems(rs.getInt("id"));
            invoice.SetItems(items);
            double total = 0;
            foreach (Item item in items)
            {
                total += item.GetCost();
            }
            invoice.SetTotal(total);
            // TODO
            //invoice.SetInvoiceDate(rs.getDate("invoicedate"));
            return invoice;
        }

        /// <exception cref="SQLException"/>
        public virtual Item GetItem(ResultSet rs) {
            Item item = new Item();
            //item.SetItem(rs.GetInt("Item"));
            //Product product = GetProduct(rs.GetInt("ProductId"));
            //item.SetProduct(product);
            //item.SetQuantity(rs.GetInt("Quantity"));
            //item.SetCost(item.GetQuantity() * product.GetPrice());
            return item;
        }

        /// <exception cref="SQLException"/>
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

        /// <exception cref="SQLException"/>
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

        /// <exception cref="SQLException"/>
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
