import hashlib
import os

# Generate a random salt
salt = os.urandom(16)

# Sample text (could be a petition document, for example)
text = "PETITION TO ADDRESS ACTS OF VANDALISM AS A NATIONAL SECURITY THREAT"

# Combine text with salt
text_with_salt = salt + text.encode()

# Compute SHA-256 hash
hash_object = hashlib.sha256(text_with_salt)
hashed_output = hash_object.hexdigest()

# Display salt and hashed output
salt.hex(), hashed_output
