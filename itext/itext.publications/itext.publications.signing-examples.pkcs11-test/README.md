# The PKCS#11 Devices Used

The tests here have been executed addressing a number of PKCS#11 modules and devices.

## SoftHSM

[SoftHSM2](https://www.opendnssec.org/softhsm/) has been installed using the [SoftHSM2 installer for MS Windows](https://github.com/disig/SoftHSM2-for-Windows) provided by [Disig a.s.](https://www.disig.sk/).

Using `softhsm2-util --init-token` a token has been initialized with SO PIN `1234` and User PIN `5678`. The automatically generated slot number on the test machine is `171137967`.

In that slot a RSA private key and associated certificate with the friendly name "RSAkey" have been generated.

## Utimaco Simulator

Another PKCS#11 device used is the [Utimaco Simulator](https://hsm.utimaco.com/products-hardware-security-modules/hsm-simulators/securityserver-simulator/). For the Utimaco PKCS#11 driver to address the correct device, a configuration file is required which must be named <tt>cs_pkcs11_R2.cfg</tt> and look like this:

    [Global]
    # Select the log level (NONE...TRACE)
    Logging = 1
    # Specifies the path where the logfile shall be created.
    Logpath = C:/temp/
    # Defines the maximum size of the logfile. If the maximum is reached,
    # old entries will be overwritten. Can be defined as value
    # in bytes or as formatted text. E.g. value of ‘1000’ means logsize
    # is 1000 bytes whereas value of ‘1000kb’ means 1000 kilobytes.
    # Allowed formats are ‘kb’, ‘mb’ and ‘gb’.
    Logsize = 10mb
    [CryptoServer]
    # Device address to connect a CryptoServer device
    Device = 3001@127.0.0.1

The full path and name of this file should be given in the environment variable <tt>CS_PKCS11_R2_CFG</tt>. If it is not, it is searched in certain default locations.

Using the Utimaco Administration Tools a token has been initialized in the slot 0 with SO PIN `1234` and User PIN `5678`.

In that slot a RSA private key and associated certificate with the friendly name "RSAkey" have been generated.

## Belgian ID cards

Yet another PKCS#11 device used are Belgian ID cards in ACS zetes card readers. The cards have been initialized for testing with the PIN `1234`.

## Entrust Signing Automation Service

PKCS#11 based signing has also been tested with the _Entrust Signing Automation Service_ (SAS). The client has been installed and configured according to the SAS User Guide.

The _administrator_ user has been created with PIN `1234`, resulting in matching `credentials` and `config` files being generated in `C:\Users\%USERNAME%\AppData\Roaming\Entrust\SigningClient\`.

As described in the chapter _Creating the Signing Key and the Certificate_ of the Guide the signing key and CSR have been created using

    signingclient create key --key-type RSA2048 --csr-out request.csr

and the provided certificate then has been imported using

    signingclient import certificate --cert ServerCertificate.crt

resulting in a key label `New Key` and a certificate label `CN=Entrust Limited,OU=ECS,O=Entrust Limited,L=Kanata,ST=Ontario,C=CA`. By using the information from the chapter _Command-line Reference_ one can get better labels.

## D-Trust cards

The next PKCS#11 device tested here are D-Trust qualified signature cards 3.1 in Reiner SCT cyberJack e-com card readers addressed via the Nexus Personal PKCS#11 driver. The cards have been initialized for testing with the PIN `12345678`.

One can select the correct key/certificate pair in slot 1 by the friendly name "Signaturzertifikat".
