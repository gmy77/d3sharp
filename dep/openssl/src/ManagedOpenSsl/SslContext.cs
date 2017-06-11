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
using OpenSSL.Crypto;
using OpenSSL.X509;

namespace OpenSSL
{
	delegate int ClientCertCallbackHandler(Ssl ssl, out X509Certificate cert, out CryptoKey key);

    #region PSK delegates
    delegate int PskClientCallbackHandler(Ssl ssl, String hint, out String identity, uint max_identity_len, out byte[] psk, uint max_psk_len);
    delegate int PskServerCallbackHandler(Ssl ssl, String identity, out byte[] psk, uint max_psk_len);
    #endregion
	/// <summary>
	/// Wraps the SST_CTX structure and methods
	/// </summary>
	internal class SslContext : Base, IDisposable
	{
		#region SSL_CTX
		[StructLayout(LayoutKind.Sequential)]
		private struct SSL_CTX
		{
			public IntPtr method; //SSL_METHOD
			public IntPtr cipher_list;  // STACK_OF(SSL_CIPHER)
			public IntPtr cipher_list_by_id; // STACK_OF(SSL_CIPHER)
			public IntPtr cert_store; //X509_STORE
			public IntPtr sessions; //lhash_st of SSL_SESSION
			public int session_cache_size;
			public IntPtr session_cache_head; //ssl_session_st
			public IntPtr session_cache_tail; // ssl_session_st
			public int session_cache_mode;
			public int session_timeout;
			public IntPtr new_session_cb; // int (*new_session_cb)(SSL*, SSL_SESSION*)
			public IntPtr remove_session_cb; // void (*remove_session_cb)(SSL*,SSL_SESSION*)
			public IntPtr get_session_cb; // SSL_SESSION*(*get_session_cb)(SSL*, uchar* data, int len, int* copy)
			#region stats
			public int stats_sess_connect;	/* SSL new conn - started */
			public int stats_sess_connect_renegotiate;/* SSL reneg - requested */
			public int stats_sess_connect_good;	/* SSL new conne/reneg - finished */
			public int stats_sess_accept;	/* SSL new accept - started */
			public int stats_sess_accept_renegotiate;/* SSL reneg - requested */
			public int stats_sess_accept_good;	/* SSL accept/reneg - finished */
			public int stats_sess_miss;		/* session lookup misses  */
			public int stats_sess_timeout;	/* reuse attempt on timeouted session */
			public int stats_sess_cache_full;	/* session removed due to full cache */
			public int stats_sess_hit;		/* session reuse actually done */
			public int stats_sess_cb_hit;	/* session-id that was not in the cache was passed back via the callback.  This
					         * indicates that the application is supplying session-id's from other processes - spooky :-) */
			#endregion
			public int references;
			public IntPtr app_verify_callback; //int (*app_verify_callback)(X509_STORE_CTX *, void *)
			public IntPtr app_verify_arg;
			public IntPtr default_passwd_callback; //pem_password_cb
			public IntPtr default_passwd_callback_userdata;
			public IntPtr client_cert_cb; //int (*client_cert_cb)(SSL *ssl, X509 **x509, EVP_PKEY **pkey)
			public IntPtr app_gen_cookie_cb; //int (*app_gen_cookie_cb)(SSL *ssl, unsigned char *cookie, unsigned int *cookie_len);
			public IntPtr app_verify_cookie_cb; //int (*app_verify_cookie_cb)(SSL *ssl, unsigned char *cookie, unsigned int cookie_len); 
			#region CRYPTO_EX_DATA ex_data;
			public IntPtr ex_data_sk;
			public int ex_data_dummy;
			#endregion
			public IntPtr rsa_md5; //EVP_MD
			public IntPtr md5; //EVP_MD
			public IntPtr sha1; //EVP_MD
			public IntPtr extra_certs; //STACK_OF(X509)
			public IntPtr comp_methods; //STACK_OF(SSL_COMP)
			public IntPtr info_callback; //void (*info_callback)(const SSL *ssl,int type,int val)
			public IntPtr client_CA; //STACK_OF(X509_NAME)
			public uint options;
			public uint mode;
			public int max_cert_list;
			public IntPtr cert; //cert_st
			public int read_ahead;
			public IntPtr msg_callback; //void (*msg_callback)(int write_p, int version, int content_type, const void *buf, size_t len, SSL *ssl, void *arg);
			public IntPtr msg_callback_arg;
			public int verify_mode;
			public uint sid_ctx_length;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = Native.SSL_MAX_SID_CTX_LENGTH)]
			public byte[] sid_ctx;
			public IntPtr default_verify_callback; //int (*default_verify_callback)(int ok,X509_STORE_CTX *ctx)
			public IntPtr generate_session_id; //typedef int (*GEN_SESSION_CB)(const SSL *ssl, unsigned char *id,unsigned int *id_len);
			#region X509_VERIFY_PARAM
			public IntPtr x509_verify_param_name;
			public long x509_verify_param_check_time;
			public int x509_verify_param_inh_flags;
			public int x509_verify_param_flags;
			public int x509_verify_param_purpose;
			public int x509_verify_param_trust;
			public int x509_verify_param_depth;
			public IntPtr x509_verify_param_policies;
			#endregion
#if __UNUSED__
	            int purpose;		/* Purpose setting */
	            int trust;		/* Trust setting */
#endif
			public int quiet_shutdown;
			//#if (! OPENSSL_ENGINE)
			// Engine to pass requests for client certs to
			public IntPtr client_cert_engine;
			//#endif
			//#if (! OPENSSL_NO_TLSEXT)
			public IntPtr tlsext_servername_callback; //int (*tlsext_servername_callback)(SSL*, int *, void *)
			public IntPtr tlsext_servername_arg;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] tlsext_tick_key_name;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] tlsext_tick_hmac_key;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] tlsext_tick_aes_key;
			public IntPtr tlsext_ticket_key_cb; //int (*tlsext_ticket_key_cb)(SSL *ssl,unsigned char *name, unsigned char *iv,EVP_CIPHER_CTX *ectx,HMAC_CTX *hctx, int enc);
			public IntPtr tlsext_status_cb; //int (*tlsext_status_cb)(SSL *ssl, void *arg);
			public IntPtr tlsext_status_arg;
            // **************************   WARNING   **************************
            // This StructLayout is not correct! Do not use it directly!
            // Only use the provided callbacks to interact with the SslContext.
            // *****************************************************************
            public IntPtr tlsext_opaque_prf_input_callback; // int (*tlsext_opaque_prf_input_callback)(SSL *, void *peerinput, size_t len, void *arg);
            public IntPtr tlsext_opaque_prf_input_callback_arg; // void *tlsext_opaque_prf_input_callback_arg;
			//#endif
            // Note that you should not set these values directly, but use the provided callback setters.
            //#ifndef OPENSSL_NO_PSK
            public IntPtr psk_identity_hint; //char *psk_identity_hint;
            public IntPtr psk_client_callback; //unsigned int (*psk_client_callback)(SSL *ssl, const char *hint, char *identity, unsigned int max_identity_len, unsigned char *psk, unsigned int max_psk_len);
            public IntPtr psk_server_callback; //unsigned int (*psk_server_callback)(SSL *ssl, const char *identity, unsigned char *psk, unsigned int max_psk_len);
            //#endif
		}
		#endregion

        #region PSK Callbacks
        private PskClientCallbackThunk _pskClientCallbackThunk;
        private PskServerCallbackThunk _pskServerCallbackThunk;
        #endregion
		#region Members

		//private SSL_CTX raw;
		private VerifyCertCallbackThunk _verifyCertCallbackThunk;
		private ClientCertCallbackThunk _clientCertCallbackThunk;
		#endregion

		/// <summary>
		/// Calls SSL_CTX_new()
		/// </summary>
		/// <param name="sslMethod"></param>
		public SslContext(SslMethod sslMethod) :
			base(Native.ExpectNonNull(Native.SSL_CTX_new(sslMethod.Handle)), true)
		{
		}

		#region Properties

		/// <summary>
		/// Calls SSL_CTX_set_options
		/// </summary>
		public SslOptions Options
		{
			set { Native.ExpectSuccess(Native.SSL_CTX_set_options(this.ptr, (int)value)); }
			get { return (SslOptions)Native.SSL_CTX_get_options(this.ptr); }
		}

		public SslMode Mode
		{
			set { Native.ExpectSuccess(Native.SSL_CTX_set_mode(this.ptr, (int)value)); }
			get { return (SslMode)Native.SSL_CTX_get_mode(this.ptr); }
		}

		#endregion

		internal class ClientCertCallbackThunk
		{
			private ClientCertCallbackHandler OnClientCertCallback;
			private Native.client_cert_cb nativeCallback;

			public Native.client_cert_cb Callback
			{
				get
				{
					if (this.OnClientCertCallback == null)
					{
						return null;
					}
					if (this.nativeCallback != null)
					{
						return this.nativeCallback;
					}
					else
					{
						this.nativeCallback = new Native.client_cert_cb(this.OnClientCertThunk);
						return this.nativeCallback;
					}
				}
			}

			public ClientCertCallbackThunk(ClientCertCallbackHandler callback)
			{
				this.OnClientCertCallback = callback;
			}

			internal int OnClientCertThunk(IntPtr ssl_ptr, out IntPtr cert_ptr, out IntPtr key_ptr)
			{
				X509Certificate cert = null;
				CryptoKey key = null;
				Ssl ssl = new Ssl(ssl_ptr, false);
				cert_ptr = IntPtr.Zero;
				key_ptr = IntPtr.Zero;

				int nRet = OnClientCertCallback(ssl, out cert, out key);
				if (nRet != 0)
				{
					if (cert != null)
					{
						cert_ptr = cert.Handle;
					}
					if (key != null)
					{
						key_ptr = key.Handle;
					}
				}
				return nRet;
			}

		}

		internal class VerifyCertCallbackThunk
		{
			private RemoteCertificateValidationHandler OnVerifyCert;
			private Native.VerifyCertCallback nativeCallback;

			public Native.VerifyCertCallback Callback
			{
				get
				{
					if (this.OnVerifyCert == null)
					{
						return null;
					}
					if (this.nativeCallback != null)
					{
						return this.nativeCallback;
					}
					else
					{
						this.nativeCallback = new Native.VerifyCertCallback(OnVerifyCertThunk);
						return this.nativeCallback;
					}
				}
			}

			public VerifyCertCallbackThunk(RemoteCertificateValidationHandler callback)
			{
				this.OnVerifyCert = callback;
			}

			internal int OnVerifyCertThunk(int ok, IntPtr store_ctx)
			{
				X509StoreContext ctx = new X509StoreContext(store_ctx, false);
				X509Certificate cert = ctx.CurrentCert;
				int depth = ctx.ErrorDepth;
				VerifyResult result = (VerifyResult)ctx.Error;
				// build the X509Chain from the store
				X509Store store = ctx.Store;
				Core.Stack<X509Object> objStack = store.Objects;
				X509Chain chain = new X509Chain();
				foreach (X509Object obj in objStack)
				{
					X509Certificate objCert = obj.Certificate;
					if (objCert != null)
					{
						chain.Add(objCert);
					}
				}
				// Call the managed delegate
				if (OnVerifyCert(this, cert, chain, depth, result))
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}
		}

		#region Methods

		/// <summary>
		/// Sets the certificate store for the context - calls SSL_CTX_set_cert_store
		/// The X509Store object and contents will be freed when the context is disposed.
		/// Ensure that the store object and it's contents have IsOwner set to false
		/// before assigning them into the context.
		/// </summary>
		/// <param name="store"></param>
		public void SetCertificateStore(X509Store store)
		{
			// Remove the native pointer ownership from the object
			// Reference counts don't work for the X509_STORE, so
			// we just remove ownership from the X509Store object
			store.IsOwner = false;
			Native.SSL_CTX_set_cert_store(this.ptr, store.Handle);
		}

		/// <summary>
		/// Sets the certificate verification mode and callback - calls SSL_CTX_set_verify
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="callback"></param>
		public void SetVerify(VerifyMode mode, RemoteCertificateValidationHandler callback)
		{
			this._verifyCertCallbackThunk = new VerifyCertCallbackThunk(callback);
			Native.SSL_CTX_set_verify(this.ptr, (int)mode, _verifyCertCallbackThunk.Callback);
		}

		/// <summary>
		/// Sets the certificate verification depth - calls SSL_CTX_set_verify_depth
		/// </summary>
		/// <param name="depth"></param>
		public void SetVerifyDepth(int depth)
		{
			Native.SSL_CTX_set_verify_depth(this.ptr, depth);
		}

		public Core.Stack<X509Name> LoadClientCAFile(string filename)
		{
			IntPtr stack = Native.SSL_load_client_CA_file(filename);
			Core.Stack<X509Name> name_stack = new Core.Stack<X509Name>(stack, true);
			return name_stack;
		}

		/// <summary>
		/// Calls SSL_CTX_set_client_CA_list/SSL_CTX_get_client_CA_list
		/// The Stack and the X509Name objects contined within them
		/// are freed when the context is disposed.  Make sure that
		/// the Stack and X509Name objects have set IsOwner to false
		/// before assigning them to the context.
		/// </summary>
		public Core.Stack<X509Name> CAList
		{
			get
			{
				IntPtr ptr = Native.SSL_CTX_get_client_CA_list(this.ptr);
				Core.Stack<X509Name> name_stack = new Core.Stack<X509Name>(ptr, false);
				return name_stack;
			}
			set
			{
				// Remove the native pointer ownership from the Stack object
				value.IsOwner = false;
				Native.SSL_CTX_set_client_CA_list(this.ptr, value.Handle);
			}
		}

		public int LoadVerifyLocations(string caFile, string caPath)
		{
			return Native.ExpectSuccess(Native.SSL_CTX_load_verify_locations(this.ptr, caFile, caPath));
		}

		public int SetDefaultVerifyPaths()
		{
			return Native.ExpectSuccess(Native.SSL_CTX_set_default_verify_paths(this.ptr));
		}

		public int SetCipherList(string cipherList)
		{
			return Native.ExpectSuccess(Native.SSL_CTX_set_cipher_list(this.ptr, cipherList));
		}

		public int UseCertificate(X509Certificate cert)
		{
			return Native.ExpectSuccess(Native.SSL_CTX_use_certificate(this.ptr, cert.Handle));
		}

		public int UseCertificateChainFile(string filename)
		{
			return Native.ExpectSuccess(Native.SSL_CTX_use_certificate_chain_file(this.ptr, filename));
		}

		public int UsePrivateKey(CryptoKey key)
		{
			return Native.ExpectSuccess(Native.SSL_CTX_use_PrivateKey(this.ptr, key.Handle));
		}

		public int UsePrivateKeyFile(string filename, SslFileType type)
		{
			return Native.ExpectSuccess(Native.SSL_CTX_use_PrivateKey_file(this.ptr, filename, (int)type));
		}

		public int CheckPrivateKey()
		{
			return Native.ExpectSuccess(Native.SSL_CTX_check_private_key(this.ptr));
		}

		public int SetSessionIdContext(byte[] sid_ctx)
		{
			return Native.ExpectSuccess(Native.SSL_CTX_set_session_id_context(this.ptr, sid_ctx, (uint)sid_ctx.Length));
		}

		public void SetClientCertCallback(ClientCertCallbackHandler callback)
		{
			_clientCertCallbackThunk = new ClientCertCallbackThunk(callback);
			Native.SSL_CTX_set_client_cert_cb(this.ptr, _clientCertCallbackThunk.Callback);
		}

		public List<string> GetCipherList()
		{
			List<string> ret = new List<string>();
			SSL_CTX raw = (SSL_CTX)Marshal.PtrToStructure(ptr, typeof(SSL_CTX));
			Core.Stack<SslCipher> stack = new Core.Stack<SslCipher>(raw.cipher_list, false);
			foreach (SslCipher cipher in stack)
			{
				IntPtr cipher_ptr = Native.SSL_CIPHER_description(cipher.Handle, null, 0);
				if (cipher_ptr != IntPtr.Zero)
				{
					string strCipher = Marshal.PtrToStringAnsi(cipher_ptr);
					ret.Add(strCipher);
					Native.OPENSSL_free(cipher_ptr);
				}
			}
			return ret;
		}

		#endregion

        #region PSK callback functions
        internal class PskClientCallbackThunk
        {
            private PskClientCallbackHandler OnPskClientCallback;
            private Native.psk_client_callback nativeCallback;

            public Native.psk_client_callback Callback
            {
                get
                {
                    if (this.OnPskClientCallback == null)
                    {
                        return null;
                    }
                    if (this.nativeCallback != null)
                    {
                        return this.nativeCallback;
                    }
                    else
                    {
                        this.nativeCallback = new Native.psk_client_callback(this.OnPskClientThunk);
                        return this.nativeCallback;
                    }
                }
            }

            public PskClientCallbackThunk(PskClientCallbackHandler callback)
            {
                this.OnPskClientCallback = callback;
            }

            // void SSL_CTX_set_psk_client_callback(SSL_CTX *ctx, unsigned int (*psk_client_callback)
            //  (SSL *ssl, const char *hint, char *identity, unsigned int max_identity_len, unsigned char *psk,	unsigned int max_psk_len));
            // 	return psk_len;
            //
            // Note that the buffers where we will put the results were allocated by the native code.
            internal int OnPskClientThunk(IntPtr ssl_ptr, IntPtr hint_ptr, IntPtr identity_ptr, uint max_identity_len, IntPtr psk_ptr, uint max_psk_len)
            {
                // Note that the ssl reference is not needed by this function.

                String hint = null; // hint passed by server for selecting identity; ignore it (not used by bnet)
                String identity = null;
                byte[] psk = null;

                int nRet = OnPskClientCallback(null, hint, out identity, max_identity_len, out psk, max_psk_len);
                if (nRet == 0)
                    return nRet;

                if (psk == null)
                    throw new ArgumentNullException("PSK Client callback psk cannot be set to null.");

                if (identity.Length > max_identity_len - 1) // need 1 byte for NULL terminator
                    throw new ArgumentOutOfRangeException("PSK Client callback identity length (" + identity.Length + ") is greater than max_identity_len (" + (max_identity_len-1) + ").");
                if (psk.Length > max_psk_len)
                    throw new ArgumentOutOfRangeException("PSK Client callback psk length (" + psk.Length + ") is greater than max_psk_len (" + max_psk_len + ").");

                CopyStringValueToNativeCharBuffer(identity, identity_ptr);
                CopyBytesToNativeCharBuffer(psk, psk_ptr);

                // Must return the length of the psk.
                return psk.Length;
            }
        }

        internal class PskServerCallbackThunk
        {
            private PskServerCallbackHandler OnPskServerCallback;
            private Native.psk_server_callback nativeCallback;

            public Native.psk_server_callback Callback
            {
                get
                {
                    if (this.OnPskServerCallback == null)
                    {
                        return null;
                    }
                    if (this.nativeCallback != null)
                    {
                        return this.nativeCallback;
                    }
                    else
                    {
                        this.nativeCallback = new Native.psk_server_callback(this.OnPskServerThunk);
                        return this.nativeCallback;
                    }
                }
            }

            public PskServerCallbackThunk(PskServerCallbackHandler callback)
            {
                this.OnPskServerCallback = callback;
            }

            // void SSL_CTX_set_psk_server_callback(SSL_CTX *ctx, unsigned int (*psk_server_callback)
            //  (SSL *ssl, const char *identity, unsigned char *psk, unsigned int max_psk_len)); 
            // 	return psk_len;
            //
            // Note that the buffers where we will put the results were allocated by the native code.
            internal int OnPskServerThunk(IntPtr ssl_ptr, IntPtr identity_ptr, IntPtr psk_ptr, uint max_psk_len)
            {
                // Note that the ssl reference is not needed by this function.

                byte[] psk = null;

                // Convert the raw C char buffer into a string.
                // Note that the trailing NULLs (0x00) need to be stripped.
                const int max_identity_len = 0x80;
                byte[] identity_bytes = new byte[max_identity_len]; 
                Marshal.Copy(identity_ptr, identity_bytes, 0, max_identity_len);
                int index = Array.FindIndex(identity_bytes, 0, (b) => b == 0);
                if (index > max_identity_len )
                    throw new ArgumentOutOfRangeException("PSK Server callback identity could not be determined from native char buffer.");

                byte[] identity_bytes_null_stripped = new byte[index];
                for (int i = 0; i < index; i++)
                {
                    identity_bytes_null_stripped[i] = identity_bytes[i];
                }
                String identity = System.Text.ASCIIEncoding.ASCII.GetString(identity_bytes_null_stripped);

                int nRet = OnPskServerCallback(null, identity, out psk, max_psk_len);
                if (nRet == 0)
                    return nRet;

                if (psk == null)
                    throw new ArgumentNullException("PSK Server callback psk cannot be set to null.");

                if (psk.Length > max_psk_len)
                    throw new ArgumentOutOfRangeException("PSK Server callback psk length (" + psk.Length + ") is greater than max_psk_len (" + max_psk_len + ").");

                CopyBytesToNativeCharBuffer(psk, psk_ptr);

                // Must return the length of the psk.
                return psk.Length;
            }
        }

        public void SetPskClientCallback(PskClientCallbackHandler callback)
        {
            _pskClientCallbackThunk = new PskClientCallbackThunk(callback);
            Native.SSL_CTX_set_psk_client_callback(this.ptr, _pskClientCallbackThunk.Callback);
        }

        public void SetPskServerCallback(PskServerCallbackHandler callback)
        {
            _pskServerCallbackThunk = new PskServerCallbackThunk(callback);
            Native.SSL_CTX_set_psk_server_callback(this.ptr, _pskServerCallbackThunk.Callback);
        }


        // Copy the given Managed byte array into a pre-allocated native char buffer.
        private static void CopyBytesToNativeCharBuffer(byte[] bytes, IntPtr intPtr)
        {
            Marshal.Copy(bytes, 0, intPtr, bytes.Length);
        }

        // Copy the value of the given Managed string into a pre-allocated native char buffer.
        private static void CopyStringValueToNativeCharBuffer(string str, IntPtr intPtr)
        {
            if (str == null)
            {
                Marshal.Copy(new byte[] { 0x00 } , 0, intPtr, 1);
            }
            else
            {
                Marshal.Copy(ConvertStringToBytes(str), 0, intPtr, str.Length + 1);
            }
        }

        // Convert the given string into a null-terminated byte array.
        private static byte[] ConvertStringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length + 1]; // add 1 for null terminator

            int i = 0;
            foreach (char c in str)
            {
                bytes[i] = (byte)c;
                i++;
            }
            bytes[i] = 0x00; // null terminate

            return bytes;
        }

        #endregion
		#region IDisposable Members

		/// <summary>
		/// base override - calls SSL_CTX_free()
		/// </summary>
		protected override void OnDispose()
		{
			Native.SSL_CTX_free(this.ptr);
		}

		#endregion
	}
}
