// Copyright (c) 2009 Ben Henderson
// All rights reserved.

// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. The name of the author may not be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
// OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using OpenSSL.Core;

namespace OpenSSL
{
	class SslCipher : Base, IStackable
	{
		#region SSL_CIPHER
		[StructLayout(LayoutKind.Sequential)]
		struct SSL_CIPHER
		{
			public int valid;
			public IntPtr name; // text name
			public uint id; // id, 4 bytes, first is version
            public uint algorithm_mkey;	/* key exchange algorithm */
            public uint algorithm_auth;	/* server authentication */
            public uint algorithm_enc;	/* symmetric encryption */
            public uint algorithm_mac;	/* symmetric authentication */
            public uint algorithm_ssl;	/* (major) protocol version */
			public uint algo_strength; // strength and export flags
			public uint algorithm2; // extra flags
			public int strength_bits; // number of bits really used
			public int alg_bits; // number of bits for algorithm
//			public uint mask; // used for matching
//			public uint mask_strength; // also used for matching
		}
		#endregion

		bool isInitialized = false;
		private SSL_CIPHER raw;
		private CipherAlgorithmType cipherAlgorithm = CipherAlgorithmType.None;
		private int cipherStrength = 0;
		private HashAlgorithmType hashAlgorithm = HashAlgorithmType.None;
		private ExchangeAlgorithmType keyExchangeAlgorithm = ExchangeAlgorithmType.None;
		private AuthenticationMethod authMethod = AuthenticationMethod.None;
		private int keyExchangeStrength = 0;
		private SslProtocols sslProtocol = SslProtocols.None;

		public SslCipher() :
			this(IntPtr.Zero, false)
		{
		}

		public SslCipher(IntPtr ptr, bool owner) :
			base(ptr, owner)
		{
			Initialize();
		}

		internal SslCipher(IStack stack, IntPtr ptr) :
			base(ptr, true)
		{
			Initialize();
		}

		/// <summary>
		/// Returns SSL_CIPHER_name()
		/// </summary>
		public string Name
		{
			get { return Native.SSL_CIPHER_name(this.ptr); }
		}

		/// <summary>
		/// Returns SSL_CIPHER_description()
		/// </summary>
		public string Description
		{
			get
			{
				byte[] buf = new byte[512];
				Native.SSL_CIPHER_description(this.ptr, buf, buf.Length);
				string ret = Encoding.ASCII.GetString(buf);
				return ret;
			}
		}

		/// <summary>
		/// Returns SSL_CIPHER_get_bits()
		/// </summary>
		public int Strength
		{
			get
			{
				Initialize();
				if (cipherStrength == 0)
				{
					int nAlgBits = 0;
					return Native.SSL_CIPHER_get_bits(this.Handle, out nAlgBits);
				}
				return cipherStrength;
			}
		}

		public const int SSL_EXPORT = 0x00000002;
		public const int SSL_EXP40 = 0x00000008;
		public const int SSL_EXP56 = 0x00000010;

		private bool IsExport(uint algo_strength)
		{
			return (algo_strength & SSL_EXPORT) > 0;
		}

		private int ExportPrivateKeyLength(uint algo_strength)
		{
			if ((algo_strength & SSL_EXP40) > 0)
			{
				return 512;
			}
			return 1024;
		}

        // [D3Inferno]
        // I don't think this method is up to date.
        private int ExportKeyLength(uint algorithm_enc, uint algo_strength)
		{
			if ((algo_strength & SSL_EXP40) > 0)
			{
				return 5;
			}
			else
			{
                if ((algorithm_enc & SSL_DES) > 0)
				{
					return 8;
				}
				return 7;
			}
		}

        // [D3Inferno]
        // These definitions come from ssl/ssl_locl.h
        // WARNING: The "algorithms" value was broken up into 5 variables in version 0.9.9.

        /* Bits for algorithm_mkey (key exchange algorithm) */
        public const int SSL_kRSA = 0x00000001; /* RSA key exchange */
        public const int SSL_kDHr = 0x00000002; /* DH cert, RSA CA cert */ /* no such ciphersuites supported! */
        public const int SSL_kDHd = 0x00000004; /* DH cert, DSA CA cert */ /* no such ciphersuite supported! */
        public const int SSL_kEDH = 0x00000008; /* tmp DH key no DH cert */
        public const int SSL_kKRB5 = 0x00000010; /* Kerberos5 key exchange */
        public const int SSL_kECDHr = 0x00000020; /* ECDH cert, RSA CA cert */
        public const int SSL_kECDHe = 0x00000040; /* ECDH cert, ECDSA CA cert */
        public const int SSL_kEECDH = 0x00000080; /* ephemeral ECDH */
        public const int SSL_kPSK = 0x00000100; /* PSK */
        public const int SSL_kGOST= 0x00000200; /* GOST key exchange */
        public const int SSL_kSRP= 0x00000400; /* SRP */

        /* Bits for algorithm_auth (server authentication) */
        public const int SSL_aRSA = 0x00000001; /* RSA auth */
        public const int SSL_aDSS = 0x00000002; /* DSS auth */
        public const int SSL_aNULL = 0x00000004; /* no auth (i.e. use ADH or AECDH) */
        public const int SSL_aDH = 0x00000008; /* Fixed DH auth (kDHd or kDHr) */ /* no such ciphersuites supported! */
        public const int SSL_aECDH = 0x00000010; /* Fixed ECDH auth (kECDHe or kECDHr) */
        public const int SSL_aKRB5 = 0x00000020; /* KRB5 auth */
        public const int SSL_aECDSA= 0x00000040; /* ECDSA auth*/
        public const int SSL_aPSK = 0x00000080; /* PSK auth */
        public const int SSL_aGOST94= 0x00000100; /* GOST R 34.10-94 signature auth */
        public const int SSL_aGOST01 = 0x00000200; /* GOST R 34.10-2001 signature auth */

/*
		public const int SSL_NULL = (SSL_eNULL);
		public const int SSL_ADH = (SSL_kEDH | SSL_aNULL);
		public const int SSL_RSA = (SSL_kRSA | SSL_aRSA);
		public const int SSL_DH = (SSL_kDHr | SSL_kDHd | SSL_kEDH);
		public const int SSL_ECDH = (SSL_kECDH | SSL_kECDHE);
		public const int SSL_FZA = (SSL_aFZA | SSL_kFZA | SSL_eFZA);
		public const int SSL_KRB5 = (SSL_kKRB5 | SSL_aKRB5);
*/

        /* Bits for algorithm_enc (symmetric encryption) */
        public const int SSL_DES = 0x00000001;
        public const int SSL_3DES = 0x00000002;
        public const int SSL_RC4 = 0x00000004;
        public const int SSL_RC2 = 0x00000008;
        public const int SSL_IDEA = 0x00000010;
        public const int SSL_eNULL = 0x00000020;
        public const int SSL_AES128 = 0x00000040;
        public const int SSL_AES256 = 0x00000080;
        public const int SSL_CAMELLIA128 = 0x00000100;
        public const int SSL_CAMELLIA256 = 0x00000200;
        public const int SSL_eGOST2814789CNT = 0x00000400;
        public const int SSL_SEED = 0x00000800;
        public const int SSL_AES128GCM = 0x00001000;
        public const int SSL_AES256GCM = 0x00002000;

        public const int SSL_AES = (SSL_AES128|SSL_AES256|SSL_AES128GCM|SSL_AES256GCM);
        public const int SSL_CAMELLIA = (SSL_CAMELLIA128|SSL_CAMELLIA256);

        /* Bits for algorithm_mac (symmetric authentication) */
        public const int SSL_MD5 = 0x00000001;
        public const int SSL_SHA1 = 0x00000002;
        public const int SSL_GOST94 = 0x00000004;
        public const int SSL_GOST89MAC = 0x00000008;
        public const int SSL_SHA256 = 0x00000010;
        public const int SSL_SHA384 = 0x00000020;
        /* Not a real MAC, just an indication it is part of cipher */
        public const int SSL_AEAD = 0x00000040;
		public const int SSL_SHA = (SSL_SHA1);

        /* Bits for algorithm_ssl (protocol version) */
        public const int SSL_SSLV2 = 0x00000001;
        public const int SSL_SSLV3 = 0x00000002;
        public const int SSL_TLSV1 = SSL_SSLV3;	/* for now */
        public const int SSL_TLSV1_2 = 0x00000004;

        /* Bits for algorithm2 (handshake digests and other extra flags) */
        public const int SSL_HANDSHAKE_MAC_MD5 = 0x10;
        public const int SSL_HANDSHAKE_MAC_SHA = 0x20;
        public const int SSL_HANDSHAKE_MAC_GOST94 = 0x40;
        public const int SSL_HANDSHAKE_MAC_SHA256 = 0x80;
        public const int SSL_HANDSHAKE_MAC_SHA384 = 0x100;
        public const int SSL_HANDSHAKE_MAC_DEFAULT = (SSL_HANDSHAKE_MAC_MD5 | SSL_HANDSHAKE_MAC_SHA);

		/* Flags for the SSL_CIPHER.algorithm2 field */
		public const int SSL2_CF_5_BYTE_ENC = 0x01;
		public const int SSL2_CF_8_BYTE_ENC = 0x02;

		private void Initialize()
		{
			if (this.ptr == IntPtr.Zero || isInitialized)
			{
				return;
			}

			isInitialized = true;

			// marshal the structure
			raw = (SSL_CIPHER)Marshal.PtrToStructure(ptr, typeof(SSL_CIPHER));
			// start picking the data out
			bool isExport = IsExport(raw.algo_strength);
			int privateKeyLength = ExportPrivateKeyLength(raw.algo_strength);
			int keyLength = ExportKeyLength(raw.algorithm_enc, raw.algo_strength);

			// Get the SSL Protocol version
            if ((raw.algorithm_ssl & SSL_SSLV2) == SSL_SSLV2)
			{
				sslProtocol = SslProtocols.Ssl2;
			}
            else if ((raw.algorithm_ssl & SSL_SSLV3) == SSL_SSLV3)
			{
				sslProtocol = SslProtocols.Tls; // SSL3 & TLS are the same here...
			}
            else if ((raw.algorithm_ssl & SSL_TLSV1_2) == SSL_TLSV1_2)
            {
                sslProtocol = SslProtocols.Tls; // WARNING: TLSV1_2 support not fully implemented
            }

			// set the keyExchange strength
			keyExchangeStrength = privateKeyLength;

			// Get the Key Exchange cipher and strength
            switch (raw.algorithm_mkey)
			{
				case SSL_kRSA:
					keyExchangeAlgorithm = ExchangeAlgorithmType.RsaKeyX;
					break;
				case SSL_kDHr:
				case SSL_kDHd:
				case SSL_kEDH:
					keyExchangeAlgorithm = ExchangeAlgorithmType.DiffieHellman;
					break;
				case SSL_kKRB5:
					keyExchangeAlgorithm = ExchangeAlgorithmType.Kerberos;
					break;
                case SSL_kECDHr:
                case SSL_kECDHe:
                case SSL_kEECDH:
					keyExchangeAlgorithm = ExchangeAlgorithmType.ECDiffieHellman;
					break;
                case SSL_kPSK:
					keyExchangeAlgorithm = ExchangeAlgorithmType.PSK;
					break;
                case SSL_kGOST:
                    keyExchangeAlgorithm = ExchangeAlgorithmType.GOST;
                    break;
                case SSL_kSRP:
                    keyExchangeAlgorithm = ExchangeAlgorithmType.SRP;
                    break;
			}

			// Get the authentication method
            switch (raw.algorithm_auth)
			{
				case SSL_aRSA:
					authMethod = AuthenticationMethod.Rsa;
					break;
				case SSL_aDSS:
					authMethod = AuthenticationMethod.Dss;
					break;
				case SSL_aDH:
					authMethod = AuthenticationMethod.DiffieHellman;
					break;
				case SSL_aKRB5:         /* VRS */
					authMethod = AuthenticationMethod.Kerberos;
					break;
				case SSL_aNULL:
					authMethod = AuthenticationMethod.None;
					break;
				case SSL_aECDSA:
					authMethod = AuthenticationMethod.ECDsa;
					break;
                case SSL_aPSK:
                    authMethod = AuthenticationMethod.PSK;
                    break;
                case SSL_aGOST94:
                    authMethod = AuthenticationMethod.GOST;
                    break;
                case SSL_aGOST01:
                    authMethod = AuthenticationMethod.GOST;
                    break;
			}
			// Get the symmetric encryption cipher info
            switch (raw.algorithm_enc)
			{
				case SSL_DES:
					cipherAlgorithm = CipherAlgorithmType.Des;
					if (isExport && keyLength == 5)
					{
						cipherStrength = 40;
					}
					else
					{
						cipherStrength = 56;
					}
					break;
				case SSL_3DES:
					cipherAlgorithm = CipherAlgorithmType.TripleDes;
					cipherStrength = 168;
					break;
				case SSL_RC4:
					cipherAlgorithm = CipherAlgorithmType.Rc4;
					if (isExport)
					{
						if (keyLength == 5)
						{
							cipherStrength = 40;
						}
						else
						{
							cipherStrength = 56;
						}
					}
					else
					{
						if ((raw.algorithm2 & SSL2_CF_8_BYTE_ENC) == SSL2_CF_8_BYTE_ENC)
						{
							cipherStrength = 64;
						}
						else
						{
							cipherStrength = 128;
						}
					}
					break;
				case SSL_RC2:
					cipherAlgorithm = CipherAlgorithmType.Rc2;
					if (isExport)
					{
						if (keyLength == 5)
						{
							cipherStrength = 40;
						}
						else
						{
							cipherStrength = 56;
						}
					}
					else
					{
						cipherStrength = 128;
					}
					break;
				case SSL_IDEA:
					cipherAlgorithm = CipherAlgorithmType.Idea;
					cipherStrength = 128;
					break;
				case SSL_eNULL:
					cipherAlgorithm = CipherAlgorithmType.None;
					break;
                case SSL_AES128:
                    cipherAlgorithm = CipherAlgorithmType.Aes128;
                    cipherStrength = 128;
                    break;
                case SSL_AES256:
                    cipherAlgorithm = CipherAlgorithmType.Aes256;
                    cipherStrength = 256;
                    break;
                case SSL_AES128GCM:
                    cipherAlgorithm = CipherAlgorithmType.Aes128GCM;
                    cipherStrength = 128;
                    break;
                case SSL_AES256GCM:
                    cipherAlgorithm = CipherAlgorithmType.Aes256GCM;
                    cipherStrength = 256;
					break;
/*
					switch (raw.strength_bits)
					{
						case 128: cipherAlgorithm = CipherAlgorithmType.Aes128; break;
						case 192: cipherAlgorithm = CipherAlgorithmType.Aes192; break;
						case 256: cipherAlgorithm = CipherAlgorithmType.Aes256; break;
					}
*/
                case SSL_CAMELLIA128:
                    cipherAlgorithm = CipherAlgorithmType.Seed;
					cipherStrength = 128;
                    break;
                case SSL_CAMELLIA256:
                    cipherAlgorithm = CipherAlgorithmType.Seed;
					cipherStrength = 128;
                    break;
/*
					switch (raw.strength_bits)
					{
						case 128: cipherAlgorithm = CipherAlgorithmType.Camellia128; break;
						case 256: cipherAlgorithm = CipherAlgorithmType.Camellia256; break;
					}
*/
                case SSL_eGOST2814789CNT:
                    cipherAlgorithm = CipherAlgorithmType.eGOST2814789CNT;
                    cipherStrength = 128; // ???
                    break;
				case SSL_SEED:
					cipherAlgorithm = CipherAlgorithmType.Seed;
					cipherStrength = 128;
					break;
			}

			// Get the MAC info
            switch (raw.algorithm_mac)
			{
				case SSL_MD5:
					hashAlgorithm = HashAlgorithmType.Md5;
					break;
				case SSL_SHA1:
					hashAlgorithm = HashAlgorithmType.Sha1;
					break;
                case SSL_GOST94:
                    hashAlgorithm = HashAlgorithmType.Gost94;
                    break;
                case SSL_GOST89MAC:
                    hashAlgorithm = HashAlgorithmType.Gost89MAC;
                    break;
                case SSL_SHA256:
                    hashAlgorithm = HashAlgorithmType.Sha256;
                    break;
                case SSL_SHA384:
                    hashAlgorithm = HashAlgorithmType.Sha384;
					break;
				default:
					hashAlgorithm = HashAlgorithmType.None;
					break;
			}
		}

		public CipherAlgorithmType CipherAlgorithm
		{
			get
			{
				Initialize();
				return cipherAlgorithm;
			}
		}

		public HashAlgorithmType HashAlgorithm
		{
			get
			{
				Initialize();
				return hashAlgorithm;
			}
		}

		public ExchangeAlgorithmType KeyExchangeAlgorithm
		{
			get
			{
				Initialize();
				return keyExchangeAlgorithm;
			}
		}

		public int KeyExchangeStrength
		{
			get
			{
				Initialize();
				return keyExchangeStrength;
			}
		}

		public SslProtocols SslProtocol
		{
			get
			{
				Initialize();
				return sslProtocol;
			}
		}

		public AuthenticationMethod AuthenticateionMethod
		{
			get
			{
				Initialize();
				return authMethod;
			}
		}

		protected override void OnDispose()
		{
			Native.OPENSSL_free(this.ptr);
		}
	}
}
