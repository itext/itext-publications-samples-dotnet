/*

This file is part of the iText (R) project.
Copyright (c) 1998-2017 iText Group NV

*/
/*
* Part of a set of classes based on a sample database.
*/
using System;
using System.Text;

namespace iText.Samples.Sandbox.Zugferd.Pojo {
    /// <summary>Plain Old Java Object containing info about an Item.</summary>
    /// <author>Bruno Lowagie (iText Software)</author>
    public class Item {
        protected internal int item;

        protected internal Product product;

        protected internal int quantity;

        protected internal double cost;

        public virtual int GetItem() {
            return item;
        }

        public virtual void SetItem(int item) {
            this.item = item;
        }

        public virtual Product GetProduct() {
            return product;
        }

        public virtual void SetProduct(Product product) {
            this.product = product;
        }

        public virtual int GetQuantity() {
            return quantity;
        }

        public virtual void SetQuantity(int quantity) {
            this.quantity = quantity;
        }

        public virtual double GetCost() {
            return cost;
        }

        public virtual void SetCost(double cost) {
            this.cost = cost;
        }

        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("  #").Append(item);
            sb.Append(product.ToString());
            sb.Append("\tQuantity: ").Append(quantity);
            sb.Append("\tCost: ").Append(cost).Append("\u20ac");
            return sb.ToString();
        }
    }
}
