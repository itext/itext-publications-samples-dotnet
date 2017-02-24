/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV

*/
/*
* Part of a set of classes based on a sample database.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace iText.Samples.Sandbox.Zugferd.Pojo {
    /// <summary>Plain Old Java Object containing info about an Invoice.</summary>
    /// <author>Bruno Lowagie (iText Software)</author>
    public class Invoice {
        protected internal int id;

        protected internal Customer customer;

        protected internal double total;

        protected internal IList<Item> items;

        protected internal DateTime invoiceDate;

        public virtual int GetId() {
            return id;
        }

        public virtual void SetId(int id) {
            this.id = id;
        }

        public virtual Customer GetCustomer() {
            return customer;
        }

        public virtual void SetCustomer(Customer customer) {
            this.customer = customer;
        }

        public virtual double GetTotal() {
            return total;
        }

        public virtual void SetTotal(double total) {
            this.total = total;
        }

        public virtual IList<Item> GetItems() {
            return items;
        }

        public virtual void SetItems(IList<Item> items) {
            this.items = items;
        }

        public virtual DateTime GetInvoiceDate() {
            return invoiceDate;
        }

        public virtual void SetInvoiceDate(DateTime invoiceDate) {
            this.invoiceDate = invoiceDate;
        }

        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("Invoice id: ").Append(id).Append(" Date: ").Append(invoiceDate).Append(" Total cost: ").Append(
                total).Append("\u20ac\n");
            sb.Append("Customer: ").Append(customer.ToString()).Append("\n");
            foreach (Item item in items) {
                sb.Append(item.ToString()).Append("\n");
            }
            return sb.ToString();
        }
    }
}
