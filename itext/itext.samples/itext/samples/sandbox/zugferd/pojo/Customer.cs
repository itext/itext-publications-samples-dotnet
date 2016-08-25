/*

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV

*/
/*
* Part of a set of classes based on a sample database.
*/
using System;
using System.Text;

namespace iText.Samples.Sandbox.Zugferd.Pojo {
    /// <summary>Plain Old Java Object containing info about a Customer.</summary>
    /// <author>Bruno Lowagie (iText Software)</author>
    public class Customer {
        protected internal int id;

        protected internal String firstName;

        protected internal String lastName;

        protected internal String street;

        protected internal String postalcode;

        protected internal String city;

        protected internal String countryId;

        public virtual int GetId() {
            return id;
        }

        public virtual void SetId(int id) {
            this.id = id;
        }

        public virtual String GetFirstName() {
            return firstName;
        }

        public virtual void SetFirstName(String firstName) {
            this.firstName = firstName;
        }

        public virtual String GetLastName() {
            return lastName;
        }

        public virtual void SetLastName(String lastName) {
            this.lastName = lastName;
        }

        public virtual String GetStreet() {
            return street;
        }

        public virtual void SetStreet(String street) {
            this.street = street;
        }

        public virtual String GetCity() {
            return city;
        }

        public virtual void SetCity(String city) {
            this.city = city;
        }

        public virtual String GetPostalcode() {
            return postalcode;
        }

        public virtual void SetPostalcode(String postalcode) {
            this.postalcode = postalcode;
        }

        public virtual String GetCountryId() {
            return countryId;
        }

        public virtual void SetCountryId(String countryId) {
            this.countryId = countryId;
        }

        public override String ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(id).Append("\n");
            sb.Append("    First Name: ").Append(firstName).Append("\n");
            sb.Append("    Last Name: ").Append(lastName).Append("\n");
            sb.Append("    Street: ").Append(street).Append("\n");
            sb.Append("    City: ").Append(countryId).Append(" ").Append(postalcode).Append(" ").Append(city);
            return sb.ToString();
        }
    }
}
