# EncryptionProgramming

I ran the notebook on Google Colab. It works better on it.

Running this code on Linux probably works better too.

Image generates by the DALL·E on ChatGPT

https://colab.research.google.com/

b'' represents bye string in python

Private Key:b'-----BEGIN RSA PRIVATE KEY-----\nMIIEowIBAAKCAQEAratBhWAUto1FbVixv0Qwh2yeQMW/GQ375FULqcdMW4Uput/U\nwIqwKwW/lOvS+icykiN20+xONysCtI7w0Lc0XZ0JuJGu1GzYJ9EtL3R9SQ2EShQI\n4HRBdmm80bO++rJ6P1hbh7MXmFPb2IggF8Lg3oNyhrd8lVrNlwvelFICaic4fPlq\nP2sgcelZ7gvyFXbNqgtle7YPssz7O43yfjEdnOBcInm/fQA3EjDS/OLzauJXVKQI\nydV282h3FddZc8covQ7qr0uCJx53bAgr+i80jQ6VFshls3tJdYnkyOxlLbBypaBB\nh4pUKsSh2I6r1LF/Kdz3x577FTxxfaZYFJ363wIDAQABAoIBABCqsk+MAIDifuq3\nC9voveWJQYjC0YukgWuQ09030LapyW7zQzY1OSHv28p9dVJnh51pxOIiuADoYkqU\nuzy0kFR5mTC63nXpejccBoOa4NktcGgxkwaDcbLdA+92GhpyHXRN1P7pa5bFWYBm\n0/mjzLPeFpQiMNUekUNxMqu2USABueyKKf7N81NEINVxUqbVlgIUXV8nP9Nozq/6\nsVaY9QJMOgoDN4C1Xnc84sRI51pP1E7GrygOcS6TBO7DiUWZu0bmHtOHJQdYKN3G\nOA/2hwkuuQBKanSaB4UV8oYJyKyljac3yNB1jZHEVDERoRQpVTkyZNWwD/EGKd9Q\nUcLOIWECgYEAv6A5MqiaJIkr8dVsrCqUWwYCRDWXaOKF+B1QBV9irlfwz0xY6zNF\nGjULCDwzR0LmlUdcjnpvrmz/VlMYNKBh5jDfasKwc2AF5tKlLQy4LQKURUBj8ugr\nTN6e5ETqerPA9IUuSj2nEPmEeVf0xTQyGfmYqXgS1kjDo7wJKFYet78CgYEA6AK+\nSEdOQwdE0hVRoPaTscOV4dFMQokvQhgPWXBc2WllVIjnmb15c71S+M0IVn0LduPS\n4zD9FgrzdL1F/ZpkJUbp44nH1BW0pr6BD7r9aN9+zVLS93wGKtK7ZUvYc5Yc554E\nBslVXxn5CCXXYFtohmaZqIZrQCr1hW2tV6sghOECgYBh/KRC60Qm+p2mA+SWBQ/n\nm6L9Dgpmb67huNt6Y9QqIn3ZAslVO9pSFF2X0HDIN8WBAASsNp91KfdHRSZTgs+M\nzeDwzq070hYyefRMnPxwx6jZ4Js7Us0ReaT2ROdB5zj70D5jaDNN0smS4w8e6BnW\nfnM59VRsjri7uSNVpPQAMQKBgQCpGi7EkaxaMHcZxE4dyvr1Sv/4ektiB4k5XD37\ny2jxUd94UNy1cqOOF0TdcNuN5lAv1HfF/dPJeCvgP4A/CoPJo7kfjjHmw/dKvXlm\nFL1U7ekHEEIR/gSku7m4aCKYhKYGr2Zx59bgnRakuKgVZCp4I1oFugt71pPjL4Bz\ncJggIQKBgBd/Z5+NUQOgkLD+m5lF3/GbNCOtDF3MqcjCGQewqJSp3qqJMB9gscLC\nam5fJh3e0Rw8o+6zG0V+JqPe0vl+WOd5xWZ3+DU3ahGKXynQYSU+QFNLJLoz4dFD\neH0x5RZ3pfsjhFy0wtCqhFN4ilE8JwGUUviClmypOh4Ql+xo5UmN\n-----END RSA PRIVATE KEY-----'

Public Key:b'-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAratBhWAUto1FbVixv0Qw\nh2yeQMW/GQ375FULqcdMW4Uput/UwIqwKwW/lOvS+icykiN20+xONysCtI7w0Lc0\nXZ0JuJGu1GzYJ9EtL3R9SQ2EShQI4HRBdmm80bO++rJ6P1hbh7MXmFPb2IggF8Lg\n3oNyhrd8lVrNlwvelFICaic4fPlqP2sgcelZ7gvyFXbNqgtle7YPssz7O43yfjEd\nnOBcInm/fQA3EjDS/OLzauJXVKQIydV282h3FddZc8covQ7qr0uCJx53bAgr+i80\njQ6VFshls3tJdYnkyOxlLbBypaBBh4pUKsSh2I6r1LF/Kdz3x577FTxxfaZYFJ36\n3wIDAQAB\n-----END PUBLIC KEY-----'

Message should be 'Secret Message'
![Candy](candy.png)

# Password Generator RFC

RFC 972：Password Generator Protocol：该协议定义了一个基于TCP的密码生成服务，旨在为用户提供随机生成的密码。它强调了使用高随机性种子的重要性，并建议避免使用基于时间的种子。 
RFC EDITOR

RFC 4086：Randomness Requirements for Security：该文档提供了安全应用中随机数生成的指导原则，强调了高质量随机数在密码学中的关键作用。

RFC 2898：PKCS #5: Password-Based Cryptography Specification Version 2.0：该标准描述了基于密码的加密方法，包括密钥派生函数PBKDF2，该函数常用于安全密码哈希。

RFC 6238：Time-Based One-Time Password Algorithm (TOTP)：该RFC定义了基于时间的一次性密码算法，广泛用于双因素认证中。 

...,

RFC ****: placeholder

# Alice and Bob in Cryptographic Systems

In cryptographic systems, Alice and Bob are commonly used as placeholder names to explain how cryptographic protocols work. Alice wants to send a secure message to Bob, and they use various cryptographic techniques to ensure the message's confidentiality, integrity, and authenticity.

## Example: Secure Communication between Alice and Bob

### Step 1: Key Generation

Alice and Bob each generate a pair of public and private keys. The public key is shared with everyone, while the private key is kept secret.

### Step 2: Encryption

Alice wants to send a secret message to Bob. She uses Bob's public key to encrypt the message. Only Bob's private key can decrypt this message.

```python
from cryptography.hazmat.primitives.asymmetric import rsa
from cryptography.hazmat.primitives import serialization, hashes
from cryptography.hazmat.primitives.asymmetric import padding

# Load Bob's public key
with open("public_key.pem", "rb") as key_file:
    public_key = serialization.load_pem_public_key(key_file.read())

# Alice's secret message
message = b"Hello, Bob!"

# Encrypt the message using Bob's public key
encrypted_message = public_key.encrypt(
    message,
    padding.OAEP(
        mgf=padding.MGF1(algorithm=hashes.SHA256()),
        algorithm=hashes.SHA256(),
        label=None
    )
)

print("Encrypted message:", encrypted_message)
```

### Step 3: Decryption

Bob receives the encrypted message and uses his private key to decrypt it.

```python
from cryptography.hazmat.primitives import serialization, hashes
from cryptography.hazmat.primitives.asymmetric import padding

# Load Bob's private key
with open("private_key.pem", "rb") as key_file:
    private_key = serialization.load_pem_private_key(key_file.read(), password=None)

# Decrypt the message using Bob's private key
decrypted_message = private_key.decrypt(
    encrypted_message,
    padding.OAEP(
        mgf=padding.MGF1(algorithm=hashes.SHA256()),
        algorithm=hashes.SHA256(),
        label=None
    )
)

print("Decrypted message:", decrypted_message.decode())
```

In this example, Alice and Bob use RSA encryption to securely communicate. Alice encrypts the message with Bob's public key, and Bob decrypts it with his private key. This ensures that only Bob can read the message, even if it is intercepted by someone else.

# Why did the scarecrow become a successful cryptographer?

Because he was outstanding in his field!

![Funny Image](funny_image.png)

# Spanish Encryption

This section describes how to use the Spanish encryption Jupyter notebook.

## Instructions

1. Open the `SpanishEncryption.ipynb` notebook in Jupyter or Google Colab.
2. Follow the instructions in the notebook to run the cells.
3. The notebook includes functions for encryption and decryption of Spanish text using a simple substitution cipher.
4. Examples and test cases are provided to demonstrate the usage of the functions.

