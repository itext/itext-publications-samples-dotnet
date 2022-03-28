### Generating The Test Key Material

A PKCS#12 store with keys and associated self-signed certificates has been generated in the `keystore` folder containing:

* a RSA private key and associated certificate with the friendly name "RSAkey",
* a DSA private key and associated certificate with the friendly name "DSAkey", and
* an ECDSA private key and associated certificate with the friendly name "ECDSAkey".

For DSA weak parameters have been chosen for reasons:

* According to the [Acrobat DC Digital Signatures Guide](https://www.adobe.com/devnet-docs/acrobatetk/tools/DigSigDC/standards.html) Adobe Acrobat only supports DSA with SHA1.
* Microsoft .Net security APIs partially turn out to only support DSA with key sizes up to 1024 bits.