# RSA Algorithm file encryption and decryption

## Settings

### SKey

* ModNumber - modulo number for RSA

* OpenKey - open RSA key [Optional]

* SecretKey - secret RSA key [Optional]

### Operations [array[Operation]] - array of encryption and decryption operations

#### Operation

* PathInput [string] - input file to perform the operation

* PathOutput [string] - output path to save the result (if the file already exists, the operation will be aborted)

* Operation [string] - two values are possible: Encrypt and Decrypt

## Usage

* Fill settings correctly (needs OpenKey for encryption and SecretKey for decryption)

* Launch program

# RSA key generator

* N = P * Q. P and Q are primes

* Generating P, Q

* Calculating Euler(N) = (P - 1) * (Q - 1)

* Finding open key E: GCD(E, Euler(N)) = 1 mod Euler(N)

* Calculating secret key D: D = E ^ (-1) mod Euler(N)

## Settings 

### MinPrimeValue 

* Lower bound for primes

### MaxPrimeValue 

* Upper bound for primes

### OutputPath 

* Path for generationg .txt file with RSA keys



