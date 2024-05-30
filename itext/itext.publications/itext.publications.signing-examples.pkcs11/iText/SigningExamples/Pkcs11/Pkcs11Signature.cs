using System;
using System.Collections.Generic;
using System.Linq;
using iText.Signatures;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.HighLevelAPI.Factories;
using Net.Pkcs11Interop.HighLevelAPI.MechanismParams;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;

namespace iText.SigningExamples.Pkcs11
{
    public class Pkcs11Signature : IExternalSignature, IDisposable
    {
        private IPkcs11Library pkcs11Library;
        private IObjectHandle privateKeyHandle;

        private X509Certificate[] chain;
        private string signatureAlgorithmName;
        private string digestAlgorithmName;
        private byte[] pin;

        private List<CKA> pkAttributeKeys;
        private List<CKA> certAttributeKeys;
        private ObjectAttributeFactory objectAttributeFactory;
        private SlotInfo selectedSlot;
        private ISession chachedSession;
        private bool loggedIn = false;

        public Pkcs11Signature(string libraryPath)
        {
            pkAttributeKeys = new List<CKA>();
            pkAttributeKeys.Add(CKA.CKA_KEY_TYPE);
            pkAttributeKeys.Add(CKA.CKA_LABEL);
            pkAttributeKeys.Add(CKA.CKA_ID);

            certAttributeKeys = new List<CKA>();
            certAttributeKeys.Add(CKA.CKA_VALUE);
            certAttributeKeys.Add(CKA.CKA_LABEL);
            certAttributeKeys.Add(CKA.CKA_ID);
            certAttributeKeys.Add(CKA.CKA_CERTIFICATE_CATEGORY);

            objectAttributeFactory = new ObjectAttributeFactory();

            Pkcs11InteropFactories factories = new Pkcs11InteropFactories();
            pkcs11Library = factories.Pkcs11LibraryFactory.LoadPkcs11Library(factories, libraryPath, AppType.MultiThreaded);
        }

        /// <summary>
        /// List the available slots and their token.
        /// </summary>
        /// <returns>the available slots and their token</returns>
        public List<Pkcs11Signature.SlotInfo> GetAvailbaleSlots()
        {
            var result = new List<Pkcs11Signature.SlotInfo>();
            foreach (var slot in pkcs11Library.GetSlotList(SlotsType.WithOrWithoutTokenPresent))
            {
                result.Add(new Pkcs11Signature.SlotInfo(slot));
            }
            return result;
        }

        /// <summary>       
        /// List the key info of keys and linked certificates available trough the selected Pkcs11 library.
        /// 
        /// A key can be used by multiple certificates.
        /// </summary>
        /// <returns>a list of key info of keys and linked certificates available trough the selected Pkcs11 library</returns>
        public List<Pkcs11KeyInfo> GetCertificatesWithPrivateKeys(SlotInfo slotInfo)
        {
            var result = new List<Pkcs11KeyInfo>();
            if (slotInfo.TokenPresent)
            {
                ISession session = getSession(slotInfo);
                {
                    List<IObjectAttribute> attributes = new List<IObjectAttribute>();
                    attributes.Add(objectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY));
                    List<IObjectHandle> keys = session.FindAllObjects(attributes);

                    foreach (IObjectHandle key in keys)
                    {
                        List<IObjectAttribute> keyAttributes = session.GetAttributeValue(key, pkAttributeKeys);

                        attributes.Clear();
                        attributes.Add(objectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
                        attributes.Add(objectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
                        attributes.Add(objectAttributeFactory.Create(CKA.CKA_ID, keyAttributes[2].GetValueAsByteArray()));
                        List<IObjectHandle> certificates = session.FindAllObjects(attributes);
                        foreach (var linkedCertificate in certificates)
                        {
                            List<IObjectAttribute> certificateAttributes = session.GetAttributeValue(linkedCertificate, certAttributeKeys);
                            result.Add(new Pkcs11KeyInfo(slotInfo, keyAttributes[2].GetValueAsByteArray(), keyAttributes[1]?.GetValueAsString(), certificateAttributes[0].GetValueAsByteArray(), certificateAttributes[1]?.GetValueAsString()));
                        }
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// Selects the key and certificate that will be used for signing.
        /// </summary>
        /// <param name="key"> The key and certificate to be used.</param>
        /// <param name="keyId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Pkcs11Signature SelectSigningKeyAndCertificate(Pkcs11KeyInfo key)
        {
            var session = getSession(key.SlotInfo);
            ObjectAttributeFactory objectAttributeFactory = new ObjectAttributeFactory();

            List<IObjectAttribute> attributes = new List<IObjectAttribute>();
            attributes.Add(objectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY));
            attributes.Add(objectAttributeFactory.Create(CKA.CKA_ID, key.keyId));
            List<IObjectHandle> keys = session.FindAllObjects(attributes);

            if (keys.Count != 1)
            {
                throw new Exception("Key " + System.Convert.ToBase64String(key.keyId) + " not found in token " + key.SlotInfo.TokenModel + " " + key.SlotInfo.TokenLabel);
            }

            privateKeyHandle = keys[0];

            List<IObjectAttribute> keyAttributes = session.GetAttributeValue(privateKeyHandle, pkAttributeKeys);

            ulong type = keyAttributes[0].GetValueAsUlong();
            switch (type)
            {
                case (ulong)CKK.CKK_RSA:
                    signatureAlgorithmName = "RSA";
                    break;
                case (ulong)CKK.CKK_DSA:
                    signatureAlgorithmName = "DSA";
                    break;
                case (ulong)CKK.CKK_ECDSA:
                    signatureAlgorithmName = "ECDSA";
                    break;
            }

            attributes.Clear();
            attributes.Add(objectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
            attributes.Add(objectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
            attributes.Add(objectAttributeFactory.Create(CKA.CKA_VALUE, key.certificateBytes));
            List<IObjectHandle> certificates = session.FindAllObjects(attributes);
            if (certificates.Count != 1)
            {
                ;
                throw new Exception("Certificate " + key.certificate + "not found in token " + key.SlotInfo.TokenModel + " " + key.SlotInfo.TokenLabel);
            }

            IObjectHandle certificate = certificates[0];
            List<IObjectAttribute> certificateAttributes = session.GetAttributeValue(certificate, certAttributeKeys);
            X509Certificate x509Certificate = new X509Certificate(X509CertificateStructure.GetInstance(certificateAttributes[0].GetValueAsByteArray()));

            List<X509Certificate> x509Certificates = new List<X509Certificate>();
            x509Certificates.Add(x509Certificate);
            attributes.Clear();
            attributes.Add(objectAttributeFactory.Create(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
            attributes.Add(objectAttributeFactory.Create(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
            List<IObjectHandle> otherCertificates = session.FindAllObjects(attributes);
            foreach (IObjectHandle otherCertificate in otherCertificates)
            {
                if (!certificate.ObjectId.Equals(otherCertificate.ObjectId))
                {
                    certificateAttributes = session.GetAttributeValue(otherCertificate, certAttributeKeys);
                    X509Certificate otherX509Certificate = new X509Certificate(X509CertificateStructure.GetInstance(certificateAttributes[0].GetValueAsByteArray()));
                    x509Certificates.Add(otherX509Certificate);
                }
            }
            this.chain = x509Certificates.ToArray();
            return this;
        }

        public void Dispose()
        {
            if (pin != null)
            {
                Array.Clear(pin, 0, pin.Length);
            }
            selectedSlot?.Slot.CloseAllSessions();
            pkcs11Library?.Dispose();
        }

        public X509Certificate[] GetChain()
        {
            checkKeySelected();
            return chain;
        }

        public bool UsePssForRsaSsa { get; set; }

        /// <summary>
        /// Set the pin when needed
        /// Remark, a copy of this value will be stored. You have to clean out the original value for security as soon as possible.
        /// 
        /// Sometimes the pin is only needed to perform the actual signing, sometimes it is needed for quering the keys.
        /// </summary>
        public byte[] Pin
        {
            get => pin;
            set
            {
                if (pin != null)
                {
                    Array.Clear(pin, 0, pin.Length);
                }
                if (value != null)
                {
                    pin = new byte[value.Length];
                    Array.Copy(value, pin, pin.Length);
                }
                else
                {
                    pin = null;
                }
            }
        }

        public string GetSignatureAlgorithmName()
        {
            checkKeySelected();
            return UsePssForRsaSsa && "RSA".Equals(signatureAlgorithmName) ? "RSASSA-PSS" : signatureAlgorithmName;
        }

        public ISignatureMechanismParams GetSignatureMechanismParameters()
        {
            checkKeySelected();
            return UsePssForRsaSsa && "RSA".Equals(signatureAlgorithmName) ? RSASSAPSSMechanismParams.CreateForDigestAlgorithm(digestAlgorithmName) : null;
        }

        public string GetDigestAlgorithmName()
        {
            return digestAlgorithmName;
        }

        public Pkcs11Signature SetDigestAlgorithmName(String digestAlgorithmName)
        {
            this.digestAlgorithmName = DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigest(digestAlgorithmName));
            return this;
        }

        public byte[] Sign(byte[] message)
        {
            checkKeySelected();
            MechanismFactory mechanismFactory = new MechanismFactory();
            IMechanism mechanism;

            switch (signatureAlgorithmName)
            {
                case "DSA":
                    switch (digestAlgorithmName)
                    {
                        case "SHA1":
                            mechanism = mechanismFactory.Create(CKM.CKM_DSA_SHA1);
                            break;
                        case "SHA224":
                            mechanism = mechanismFactory.Create(CKM.CKM_DSA_SHA224);
                            break;
                        case "SHA256":
                            mechanism = mechanismFactory.Create(CKM.CKM_DSA_SHA256);
                            break;
                        case "SHA384":
                            mechanism = mechanismFactory.Create(CKM.CKM_DSA_SHA384);
                            break;
                        case "SHA512":
                            mechanism = mechanismFactory.Create(CKM.CKM_DSA_SHA512);
                            break;
                        default:
                            throw new ArgumentException("Not supported: " + digestAlgorithmName + "with" + signatureAlgorithmName);
                    }
                    break;
                case "ECDSA":
                    switch (digestAlgorithmName)
                    {
                        case "SHA1":
                            mechanism = mechanismFactory.Create(CKM.CKM_ECDSA_SHA1);
                            break;
                        case "SHA224":
                            mechanism = mechanismFactory.Create(CKM.CKM_ECDSA_SHA224);
                            break;
                        case "SHA256":
                            mechanism = mechanismFactory.Create(CKM.CKM_ECDSA_SHA256);
                            break;
                        case "SHA384":
                            mechanism = mechanismFactory.Create(CKM.CKM_ECDSA_SHA384);
                            break;
                        case "SHA512":
                            mechanism = mechanismFactory.Create(CKM.CKM_ECDSA_SHA512);
                            break;
                        default:
                            throw new ArgumentException("Not supported: " + digestAlgorithmName + "with" + signatureAlgorithmName);
                    }
                    break;
                case "RSA":
                    if (UsePssForRsaSsa)
                    {
                        MechanismParamsFactory mechanismParamsFactory = new MechanismParamsFactory();
                        IMechanismParams pssParams = null;
                        switch (digestAlgorithmName)
                        {
                            case "SHA1":
                                pssParams = mechanismParamsFactory.CreateCkRsaPkcsPssParams((ulong)CKM.CKM_SHA_1, (ulong)CKG.CKG_MGF1_SHA1, (ulong)(DigestAlgorithms.GetOutputBitLength(digestAlgorithmName) / 8));
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA1_RSA_PKCS_PSS, pssParams);
                                break;
                            case "SHA224":
                                pssParams = mechanismParamsFactory.CreateCkRsaPkcsPssParams((ulong)CKM.CKM_SHA224, (ulong)CKG.CKG_MGF1_SHA224, (ulong)(DigestAlgorithms.GetOutputBitLength(digestAlgorithmName) / 8));
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA224_RSA_PKCS_PSS, pssParams);
                                break;
                            case "SHA256":
                                pssParams = mechanismParamsFactory.CreateCkRsaPkcsPssParams((ulong)CKM.CKM_SHA256, (ulong)CKG.CKG_MGF1_SHA256, (ulong)(DigestAlgorithms.GetOutputBitLength(digestAlgorithmName) / 8));
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA256_RSA_PKCS_PSS, pssParams);
                                break;
                            case "SHA384":
                                pssParams = mechanismParamsFactory.CreateCkRsaPkcsPssParams((ulong)CKM.CKM_SHA384, (ulong)CKG.CKG_MGF1_SHA384, (ulong)(DigestAlgorithms.GetOutputBitLength(digestAlgorithmName) / 8));
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA384_RSA_PKCS_PSS, pssParams);
                                break;
                            case "SHA512":
                                pssParams = mechanismParamsFactory.CreateCkRsaPkcsPssParams((ulong)CKM.CKM_SHA224, (ulong)CKG.CKG_MGF1_SHA224, (ulong)(DigestAlgorithms.GetOutputBitLength(digestAlgorithmName) / 8));
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA512_RSA_PKCS_PSS, pssParams);
                                break;
                            default:
                                throw new ArgumentException("Not supported: " + digestAlgorithmName + "with" + signatureAlgorithmName);
                        }
                    }
                    else
                    {
                        switch (digestAlgorithmName)
                        {
                            case "SHA1":
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA1_RSA_PKCS);
                                break;
                            case "SHA224":
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA224_RSA_PKCS);
                                break;
                            case "SHA256":
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA256_RSA_PKCS);
                                break;
                            case "SHA384":
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA384_RSA_PKCS);
                                break;
                            case "SHA512":
                                mechanism = mechanismFactory.Create(CKM.CKM_SHA512_RSA_PKCS);
                                break;
                            default:
                                throw new ArgumentException("Not supported: " + digestAlgorithmName + "with" + signatureAlgorithmName);
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("Not supported: " + digestAlgorithmName + "with" + signatureAlgorithmName);
            }
            var session = getSession(selectedSlot);
            try
            {
                return session.Sign(mechanism, privateKeyHandle, message);
            }
            catch (Exception e)
            {
                if (pin != null)
                {
                    Array.Clear(pin, 0, pin.Length);
                    pin = null;
                }
                selectedSlot?.Slot.CloseAllSessions();
                throw;
            }
        }

        private ISession getSession(SlotInfo slotInfo)
        {
            if (slotInfo == selectedSlot && chachedSession != null)
            {
                logIn();
                return chachedSession;
            }
            if (chachedSession != null)
            {
                chachedSession.CloseSession();
                slotInfo.Slot.CloseAllSessions();
            }
            chachedSession = slotInfo.GetSession();
            selectedSlot = slotInfo;
            logIn();
            return chachedSession;
        }

        private void logIn()
        {
            if (pin != null && !loggedIn)
            {
                try
                {
                    chachedSession.Login(CKU.CKU_USER, pin);
                }
                catch (Exception e)
                {
                    Array.Clear(pin, 0, pin.Length);
                    pin = null;
                    throw;
                }
                loggedIn = true;
            }
        }


        private void checkKeySelected()
        {
            if (privateKeyHandle == null)
                throw new Exception("Invalid state, no key selected yet.");            
        }

        public class SlotInfo
        {
            private ISlotInfo slotInfo;
            private ISlot slot;
            private ITokenInfo tokenInfo;

            internal SlotInfo(ISlot slot)
            {
                this.slot = slot;
                this.slotInfo = slot.GetSlotInfo();
                if (slotInfo.SlotFlags.TokenPresent)
                {
                    this.tokenInfo = slot.GetTokenInfo();
                }
            }
            
            public ulong SlotId { get => slotInfo.SlotId; }
            public string SlotDescription { get => slotInfo.SlotDescription; }
            public bool HardwareSlot { get => slotInfo.SlotFlags.HardwareSlot; }
            public bool RemovableDevice { get => slotInfo.SlotFlags.RemovableDevice; }
            public bool TokenPresent { get => slotInfo.SlotFlags.TokenPresent; }
            public string TokenModel { get => tokenInfo?.Model; }
            public string TokenLabel { get => tokenInfo?.Label; }
            public bool LoginRequired { get => tokenInfo?.TokenFlags.LoginRequired ?? false; }

            internal ISlot Slot { get => slot; }
            internal ISlotInfo getSlotInfo() { return slotInfo; }
            internal ITokenInfo getTokenInfo() { return tokenInfo; }

            internal ISession GetSession()
            {
                return slot.OpenSession(SessionType.ReadOnly);
            }
        }

        /// <summary>
        /// Contains info about keys available trough pkcs 11 container    
        /// </summary>
        public class Pkcs11KeyInfo
        {
            /// <summary>
            /// The key id
            /// </summary>
            public byte[] keyId { get; }
            /// <summary>
            /// The label of the key
            /// </summary>
            public string keyLabel { get; }            

            /// <summary>
            /// The certificate associated with the key
            /// </summary>
            public X509Certificate certificate { get; }

            /// <summary>
            /// The label assigned to the certificate
            /// </summary>
            public string certificateLabel { get; }
            public SlotInfo SlotInfo { get; internal set; }

            internal byte[] certificateBytes;
            internal Pkcs11KeyInfo(SlotInfo slotInfo, byte[] keyId, string keyLabel, byte[] certificate, string certificateLabel)
            {
                this.SlotInfo = slotInfo;
                this.keyId = keyId;
                this.keyLabel = keyLabel;
                this.certificateBytes = certificate;
                this.certificate = new X509Certificate(X509CertificateStructure.GetInstance(certificate));
                this.certificateLabel = certificateLabel;
            }
        }
    }
}
