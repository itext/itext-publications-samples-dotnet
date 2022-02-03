# The Utimaco HSM Used

For the tests the [Utimaco Simulator](https://hsm.utimaco.com/products-hardware-security-modules/hsm-simulators/securityserver-simulator/) is used.

The Utimaco CryptoServer Key Storage Provider has been installed.

Using the Utimaco Administration Tools a _cryptographic user_ `CNG` with `CXI_GROUP=CNG` and HMAC password `5678` has been created.

For the Utimaco CNG provider to address the correct device and group, its configuration file `c:\ProgramData\Utimaco\CNG\cs_cng.cfg` is required to contain in particular these entries:

    KeysExternal = false
    Group = CNG
    Device = 3001@127.0.0.1

An ECDSA keypair then has been generated in that group using (on a single line)

    cngtool Export=allow Name=DEMOecdsa CreateKey=ECDSA,521

An associated self-signed certificate is automatically created by the tests in the Windows user certificate store.
