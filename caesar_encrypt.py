sin = input()

n = 13
lower_case = "abcdefghijklmnopqrstuvwxyz"
alpha_set = set(lower_case + lower_case.upper())


def encrypt_char(s):
    decrypted_i = lower_case.index(s)
    encrypted_i = (decrypted_i + n) - 26
    return lower_case[encrypted_i]


def encrypt_message():
    d = {k: encrypt_char(k) for k in lower_case}
    out = ""
    for c in sin:
        if c in alpha_set:
            out += d[c] if c.islower() else d[c.lower()].upper()
        else:
            out += c
    return out


print(encrypt_message())
