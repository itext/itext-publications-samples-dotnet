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

        private static readonly String DB = NUnit.Framework.TestContext.CurrentContext.TestDirectory +
                                            " /../../resources/zugferd/db/invoices";

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
            while (rs.next()) {
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
            foreach (Item item in items) {
                total += item.GetCost();
            }
            invoice.SetTotal(total);
            java.util.Date date = rs.getDate("invoicedate");
            invoice.SetInvoiceDate(new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(date.getTime() / 1000).ToLocalTime());
            return invoice;
        }

        /// <exception cref="SQLException"/>
        public virtual Item GetItem(ResultSet rs) {
            Item item = new Item();
            item.SetItem(rs.getInt("Item"));
            Product product = GetProduct(rs.getInt("ProductId"));
            item.SetProduct(product);
            item.SetQuantity(rs.getInt("Quantity"));
            item.SetCost(item.GetQuantity()*product.GetPrice());
            return item;
        }

        /// <exception cref="SQLException"/>
        public virtual Customer GetCustomer(int id) {
            if (customerCache.ContainsKey(id)) {
                return customerCache[id];
            }
            getCustomer.setInt(1, id);
            ResultSet rs = getCustomer.executeQuery();
            if (rs.next()) {
                Customer customer = new Customer();
                customer.SetId(id);
                customer.SetFirstName(rs.getString("FirstName"));
                customer.SetLastName(rs.getString("LastName"));
                customer.SetStreet(rs.getString("Street"));
                customer.SetPostalcode(rs.getString("Postalcode"));
                customer.SetCity(rs.getString("City"));
                customer.SetCountryId(rs.getString("CountryID"));
                customerCache[id] = customer;
                return customer;
            }
            return null;
        }

        /// <exception cref="SQLException"/>
        public virtual Product GetProduct(int id) {
            if (productCache.ContainsKey(id)) {
                return productCache[id];
            }
            getProduct.setInt(1, id);
            ResultSet rs = getProduct.executeQuery();
            if (rs.next()) {
                Product product = new Product();
                product.SetId(id);
                product.SetName(rs.getString("Name"));
                product.SetPrice(rs.getDouble("Price"));
                product.SetVat(rs.getDouble("Vat"));
                productCache[id] = product;
                return product;
            }
            return null;
        }

        /// <exception cref="SQLException"/>
        public virtual IList<Item> GetItems(int invoiceid) {
            IList<Item> items = new List<Item>();
            getItems.setInt(1, invoiceid);
            ResultSet rs = getItems.executeQuery();
            while (rs.next()) {
                items.Add(GetItem(rs));
            }
            return items;
        }
    }
}
