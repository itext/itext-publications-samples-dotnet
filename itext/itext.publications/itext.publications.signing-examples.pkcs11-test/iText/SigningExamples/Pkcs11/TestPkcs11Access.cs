using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.HighLevelAPI.Factories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace iText.SigningExamples.Pkcs11
{
    public class TestPkcs11Access
    {
        /// <summary>
        /// A first PKCS#11 access test to the SoftHSM2 installation via Pkcs11Interop.
        /// (Copied from https://github.com/Pkcs11Interop/Pkcs11Interop/blob/5.1.1/doc/GETTING_STARTED.md as a starter.)
        /// </summary>
        [Test]
        public void TestAccess()
        {
            // Specify the path to unmanaged PKCS#11 library provided by the cryptographic device vendor
            string pkcs11LibraryPath = @"d:\Program Files\SoftHSM2\lib\softhsm2-x64.dll";

            // Create factories used by Pkcs11Interop library
            Pkcs11InteropFactories factories = new Pkcs11InteropFactories();

            // Load unmanaged PKCS#11 library
            using (IPkcs11Library pkcs11Library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, pkcs11LibraryPath, AppType.MultiThreaded))
            {
                // Show general information about loaded library
                ILibraryInfo libraryInfo = pkcs11Library.GetInfo();

                Console.WriteLine("Library");
                Console.WriteLine("  Manufacturer:       " + libraryInfo.ManufacturerId);
                Console.WriteLine("  Description:        " + libraryInfo.LibraryDescription);
                Console.WriteLine("  Version:            " + libraryInfo.LibraryVersion);

                // Get list of all available slots
                foreach (ISlot slot in pkcs11Library.GetSlotList(SlotsType.WithOrWithoutTokenPresent))
                {
                    // Show basic information about slot
                    ISlotInfo slotInfo = slot.GetSlotInfo();

                    Console.WriteLine();
                    Console.WriteLine("Slot");
                    Console.WriteLine("  Manufacturer:       " + slotInfo.ManufacturerId);
                    Console.WriteLine("  Description:        " + slotInfo.SlotDescription);
                    Console.WriteLine("  Token present:      " + slotInfo.SlotFlags.TokenPresent);

                    if (slotInfo.SlotFlags.TokenPresent)
                    {
                        // Show basic information about token present in the slot
                        ITokenInfo tokenInfo = slot.GetTokenInfo();

                        Console.WriteLine("Token");
                        Console.WriteLine("  Manufacturer:       " + tokenInfo.ManufacturerId);
                        Console.WriteLine("  Model:              " + tokenInfo.Model);
                        Console.WriteLine("  Serial number:      " + tokenInfo.SerialNumber);
                        Console.WriteLine("  Label:              " + tokenInfo.Label);

                        // Show list of mechanisms (algorithms) supported by the token
                        Console.WriteLine("Supported mechanisms: ");
                        foreach (CKM mechanism in slot.GetMechanismList())
                            Console.WriteLine("  " + mechanism);
                    }
                }
            }
        }

        [Test]
        public void TestAccessKeyAndCertificate()
        {
            // Specify the path to unmanaged PKCS#11 library provided by the cryptographic device vendor
            string pkcs11LibraryPath = @"d:\Program Files\SoftHSM2\lib\softhsm2-x64.dll";

            Pkcs11InteropFactories factories = new Pkcs11InteropFactories();
            using (IPkcs11Library pkcs11Library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, pkcs11LibraryPath, AppType.MultiThreaded))
            {
                ISlot slot = pkcs11Library.GetSlotList(SlotsType.WithTokenPresent).Find(slot => slot.SlotId == 171137967);
                Assert.IsNotNull(slot, "Slot with ID 171137967 does not exist or does not have token.");

                using (ISession session = slot.OpenSession(SessionType.ReadWrite))
                {
                    // Login as normal user
                    session.Login(CKU.CKU_USER, "5678");

                    List<IObjectAttribute> attributes = new List<IObjectAttribute>();
                    ObjectAttributeFactory objectAttributeFactory = new ObjectAttributeFactory();
                    attributes.Add(objectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY));
                    List<IObjectHandle> keys = session.FindAllObjects(attributes);
                    Assert.AreEqual(1, keys.Count, "Unexpected number of private keys: {0}", keys.Count);


                    attributes.Clear();
                    attributes.Add(objectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
                    attributes.Add(objectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
                    List<IObjectHandle> certificates = session.FindAllObjects(attributes);
                    Assert.AreEqual(1, certificates.Count, "Unexpected number of certificates: {0}", certificates.Count);

                    List<CKA> certificateAttributeKeys = new List<CKA>();
                    certificateAttributeKeys.Add(CKA.CKA_VALUE);
                    certificateAttributeKeys.Add(CKA.CKA_LABEL);
                    List<IObjectAttribute> certificateAttributes = session.GetAttributeValue(certificates[0], certificateAttributeKeys);
                    Assert.AreEqual(2, certificateAttributes.Count, "Unexpected number of certificate attributes: {0}", certificateAttributes.Count);
                    string certificateLabel = certificateAttributes[1].GetValueAsString();
                    Console.WriteLine("Certificate label: {0}", certificateLabel);
                    byte[] certificateBytes = certificateAttributes[0].GetValueAsByteArray();
                    X509Certificate2 certificate = new X509Certificate2(certificateBytes);
                    Console.WriteLine("Subject: {0}", certificate.Subject.ToString());
                }
            }
        }
    }
}