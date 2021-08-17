Imports System.Security.Cryptography

Public Class SecurePassword
    ' This class largely replicates a Java implementation linked below, with two main differences.
    '
    ' Differences:
    '    1. This implementation supports multiple algorithms with the possibility to add more in the future.
    '    2. Method slowEquals is replaced with byteArraysAreEqual, containing an appropriate explanation.
    '
    ' Java source: https://github.com/defuse/password-hashing/blob/master/PasswordStorage.java

    ' Password settings. May be adjusted.
    Private ReadOnly HASH_ALGORITHM As HashAlgorithmName = HashAlgorithmName.SHA256
    Private ReadOnly PBKDF2_ITERATIONS As Integer = 64000
    Private ReadOnly HASH_BYTE_SIZE As Integer = 256 / 8          ' 256 bits / 8 = 32 bytes
    Private ReadOnly SALT_BYTE_SIZE As Integer = 192 / 8          ' 192 bits / 8 = 24 bytes

    ' Password format indexes, used for verifying passwords. Do not change these values.
    Private ReadOnly HASH_SECTIONS As Integer = 5
    Private ReadOnly HASH_ALGORITHM_INDEX As Integer = 0
    Private ReadOnly ITERATION_INDEX As Integer = 1
    Private ReadOnly HASH_SIZE_INDEX As Integer = 2
    Private ReadOnly SALT_INDEX As Integer = 3
    Private ReadOnly HASH_INDEX As Integer = 4

    Public Function CreateHash(password As String) As String
        ' Returns a hashed password in the format "algorithm:iterations:hashSize:salt:hash".

        ' Generate random salt.
        Dim salt() As Byte = New Byte(SALT_BYTE_SIZE) {}
        Using rngCsp As New RNGCryptoServiceProvider()
            rngCsp.GetBytes(salt)
        End Using

        ' Hash the password.
        Dim hash() As Byte = pbkdf2(password, salt, PBKDF2_ITERATIONS, HASH_ALGORITHM)

        ' Build the return string.
        Dim parts As String = HASH_ALGORITHM.Name + ":" +
                              PBKDF2_ITERATIONS.ToString() + ":" +
                              hash.Length.ToString() + ":" +
                              Convert.ToBase64String(salt) + ":" +
                              Convert.ToBase64String(hash)

        Return parts

    End Function

    Public Function VerifyPassword(password As String, correctHash As String) As Boolean
        ' Verifies whether passwords match. Returns True / False.

        ' Split correctHash on ":" and check whether it's in the valid format (of length HASH_SECTIONS).
        Dim params() As String = correctHash.Split(":"c)
        If params.Length <> HASH_SECTIONS Then
            Throw New Exception("Fields are missing from the password hash.")
        End If

        ' Dictionary of currently accepted (and supported) algorithms. May be extended in the future with stronger algorithms.
        ' Do NOT remove any algorithms from this list, as doing so may break users' passwords. For example if their password was
        ' hashed using SHA256, and it's removed from the dictionary below; then it won't be possible to recreate their password
        ' for verification purposes.
        Dim hashAlgorithms As New Dictionary(Of String, HashAlgorithmName) From {
            {"SHA256", HashAlgorithmName.SHA256},
            {"SHA384", HashAlgorithmName.SHA384},
            {"SHA512", HashAlgorithmName.SHA512}
        }

        ' Check whether the algorithm is valid (i.e. contained within the hashAlgorithms dictionary).
        Dim algorithmName As String = params(HASH_ALGORITHM_INDEX)
        If Not hashAlgorithms.ContainsKey(algorithmName) Then
            Throw New Exception("Unsupported hash algorithm.")
        End If

        ' Convert string algorithmName to HashAlgorithmName using the dictionary.
        Dim algorithm As HashAlgorithmName = hashAlgorithms(algorithmName)

        ' Parse the iteration count.
        Dim iterations As Integer
        Try
            iterations = Integer.Parse(params(ITERATION_INDEX))
        Catch ex As Exception
            Throw New Exception("Could not parse the iteration count as an integer.", ex)
        End Try

        ' Make sure iteration count is valid.
        If iterations < 1 Then
            Throw New Exception("Invalid number of iterations. Must be >= 1.")
        End If

        ' Try decoding the salt.
        Dim salt() As Byte
        Try
            salt = Convert.FromBase64String(params(SALT_INDEX))
        Catch ex As Exception
            Throw New Exception("Base64 decoding of salt failed.", ex)
        End Try

        ' Try decoding the hash.
        Dim hash() As Byte
        Try
            hash = Convert.FromBase64String(params(HASH_INDEX))
        Catch ex As Exception
            Throw New Exception("Base64 decoding of pbkdf2 output failed.", ex)
        End Try

        ' Parse the hash size.
        Dim storedHashSize As Integer
        Try
            storedHashSize = Integer.Parse(params(HASH_SIZE_INDEX))
        Catch ex As Exception
            Throw New Exception("Could not parse the hash size as an integer.", ex)
        End Try

        If storedHashSize <> hash.Length Then
            Throw New Exception("Hash length doesn't match stored hash length.")
        End If

        ' Recreate the hash using the data from params, and compare it to the real hash.
        Dim recreatedHash() As Byte = pbkdf2(password, salt, iterations, algorithm)
        Return byteArraysAreEqual(hash, recreatedHash)

    End Function

    Private Function byteArraysAreEqual(a() As Byte, b() As Byte) As Boolean
        ' Notes:
        ' The Java source named this function slowEquals, however I wasn't able to replicate its behaviour in VB.NET.
        ' In C# there's an ideal method for this purpose called CryptographicOperations.FixedTimeEquals; but it's unavailable in VB.NET.
        ' The implementation below is not ideal as it uses early return so it's not slow / constant time, which is much preferred
        ' according to my research into this topic.
    
        If a.Length <> b.Length Then
            Return False
        End If
    
        For i As Integer = 0 To a.Length - 1 Step i + 1
            If a(i) <> b(i) Then
                Return False
            End If
        Next
  
        Return True
  
    End Function

    Private Function pbkdf2(password As String, salt() As Byte, iterations As Integer, algorithm As HashAlgorithmName) As Byte()
        ' Hashes the password and returns it as a byte array.
        
        Dim hash() As Byte
        
        Using pbkdf2_ As New Rfc2898DeriveBytes(password, salt, iterations, algorithm)
            hash = pbkdf2_.GetBytes(HASH_BYTE_SIZE)
        End Using
        
        Return hash
  
    End Function

End Class
