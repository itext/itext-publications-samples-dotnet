/*

This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV

*/
/*
* Part of a set of classes based on a sample database.
*/
using System;
using System.Text;

namespace iText.Samples.Sandbox.Zugferd.Pojo {
    /// <summary>Plain Old Java Object containing info about a Product.</summary>
    /// <author>Bruno Lowagie (iText Software)</author>
    public class Product {
        protected internal int id;

        protected internal String name;

        protected internal double price;

        protected internal double vat;

        public virtual int GetId() {
            return id;
        }

        public virtual void SetId(int id) {
            this.id = id;
        }

        public virtual String GetName() {
            return name;
        }

        public virtual void SetName(String name) {
            this.name = name;
        }

        public virtual double GetPrice() {
            return price;
        }

        public virtual void SetPrice(double price) {
            this.price = price;
        }

        public virtual double GetVat() {
            return vat;
        }

        public virtual void SetVat(double vat) {
            this.vat = vat;
        }

        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("\t(").Append(id).Append(")\t").Append(name).Append("\t").Append(price).Append("\u20ac\tvat ").Append
                (vat).Append("%");
            return sb.ToString();
        }
    }
}
