using System;
using System.IO;
using System.Linq;
using System.Text;
using iText.Commons.Bouncycastle.Crypto;
using iText.Signatures;
using Org.BouncyCastle.Math;

namespace iText.Samples.Signatures.Chapter01
{
    public class C1_02_DigestBC
    {
        public static readonly String DEST = "results/signatures/chapter01/";

        public static readonly String EXPECTED_OUTPUT = "Digest using MD5: 16\n" +
                                                        "Digest: 5f4dcc3b5aa765d61d8327deb882cf99\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using SHA-1: 20\n" +
                                                        "Digest: 5baa61e4c9b93f3f0682250b6cf8331b7ee68fd8\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using SHA-224: 28\n" +
                                                        "Digest: d63dc919e201d7bc4c825630d2cf25fdc93d4b2f0d46706d29038d01\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using SHA-256: 32\n" +
                                                        "Digest: 5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using SHA-384: 48\n" +
                                                        "Digest: a8b64babd0aca91a59bdbb7761b421d4f2bb38280d3a75ba0f21f2" +
                                                        "bebc45583d446c598660c94ce680c47d19c30783a7\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using SHA-512: 64\n" +
                                                        "Digest: b109f3bbbc244eb82441917ed06d618b9008dd09b3befd1b5e07394" +
                                                        "c706a8bb980b1d7785e5976ec049b46df5f1326af5a2ea6d103fd07c95385ffab0cacbc86\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using RIPEMD128: 16\n" +
                                                        "Digest: c9c6d316d6dc4d952a789fd4b8858ed7\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using RIPEMD160: 20\n" +
                                                        "Digest: 2c08e8f5884750a7b99f6f2f342fc638db25ff31\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n" +
                                                        "Digest using RIPEMD256: 32\n" +
                                                        "Digest: f94cf96c79103c3ccad10d308c02a1db73b986e2c48962e96ecd305e0b80ef1b\n" +
                                                        "Is the password 'password'? True\n" +
                                                        "Is the password 'secret'? False\n";

        protected byte[] digest;
        protected IIDigest messageDigest;

        protected C1_02_DigestBC(String password, String algorithm)
        {
            messageDigest = DigestAlgorithms.GetMessageDigest(algorithm);
            digest = DigestAlgorithms.Digest(new MemoryStream(Encoding.UTF8.GetBytes(password)), messageDigest);
        }

        public static C1_02_DigestBC GetInstance(String password, String algorithm)
        {
            return new C1_02_DigestBC(password, algorithm);
        }

        public static void Main(String[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(DEST);
            directory.Create();

            TestAll();
        }

        public static void TestAll()
        {
            ShowTest("MD5");
            ShowTest("SHA-1");
            ShowTest("SHA-224");
            ShowTest("SHA-256");
            ShowTest("SHA-384");
            ShowTest("SHA-512");
            ShowTest("RIPEMD128");
            ShowTest("RIPEMD160");
            ShowTest("RIPEMD256");
        }

        public static void ShowTest(String algorithm)
        {
            C1_02_DigestBC app = GetInstance("password", algorithm);
            Console.WriteLine("Digest using " + algorithm + ": " + app.GetDigestSize());
            Console.WriteLine("Digest: " + app.GetDigestAsHexString());
            Console.WriteLine("Is the password 'password'? " + app.CheckPassword("password"));
            Console.WriteLine("Is the password 'secret'? " + app.CheckPassword("secret"));
        }

        public int GetDigestSize()
        {
            return digest.Length;
        }

        public String GetDigestAsHexString()
        {
            return new BigInteger(1, digest).ToString(16);
        }

        // This method checks if the digest of the password is equal
        // to the digest of the text line which is passed as argument
        public bool CheckPassword(String password)
        {
            return digest.SequenceEqual(DigestAlgorithms
                .Digest(new MemoryStream(Encoding.UTF8.GetBytes(password)), messageDigest));
        }
    }
}