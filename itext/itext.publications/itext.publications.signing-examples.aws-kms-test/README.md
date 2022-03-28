# AWS KMS Test Profile

The tests use the default profile the credentials for which can be configured in `~/.aws/credentials`:

    [default]
    aws_access_key_id = ABCDEFGHIJKLMNOPQRST
    aws_secret_access_key = uv+WxyZ012/34+567890ab/cdEFGhIjKLMnoPQrS

Furthermore, the region to use is expected to be configured, e.g. in `~/.aws/config`:

    [default]
    region = RE-GION-0

Alternatively you can use environment variables or change the code slightly for customized credential and region supply. Read the AWS documentation for details.

# AWS KMS Test Keys

The tests use KMS keys configured as follows

* **alias/SigningExamples-RSA_2048** - an *asymmetric* key of type *RSA_2048* for *signing and verification* using the *RSASSA_x_SHA_y* algorithms configured with the alias *SigningExamples-RSA_2048*.
* **alias/SigningExamples-ECC_NIST_P256** - an *asymmetric* key of type *ECC_NIST_P256* for *signing and verification* using the *ECDSA_SHA_256* algorithm configured with the alias *SigningExamples-ECC_NIST_P256*.
