import bcrypt

# Function to hash a password
def hash_password(password: str) -> bytes:
    password_bytes = password.encode('utf-8')  # Encode the password to bytes
    hashed = bcrypt.hashpw(password_bytes, bcrypt.gensalt())
    return hashed

# Function to verify a password
def check_password(password: str, hashed: bytes) -> bool:
    password_bytes = password.encode('utf-8')  # Encode the password to bytes
    return bcrypt.checkpw(password_bytes, hashed)

# Example usage
password = 'P@ssw0rd😊'
hashed_password = hash_password(password)
print(f"Hashed Password: {hashed_password}")

# Verify the password
is_correct = check_password(password, hashed_password)
print(f"Password is correct: {is_correct}")
