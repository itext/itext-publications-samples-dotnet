This project contains generic `IExternalSignature` and `IExternalSignatureContainer` implementations.

### Restrictions

* `X509Certificate2Signature` is geared towards DER/Standard encoding of [EC]DSA signatures. For use with \*-PLAIN-\* algorithms you'll have to remove the calls of PlainToDer and maybe even introduce a DerToPlain method.
