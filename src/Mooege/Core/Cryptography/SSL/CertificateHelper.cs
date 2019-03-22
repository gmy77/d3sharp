﻿/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenSSL.Core;
using OpenSSL.Crypto;
using OpenSSL.X509;

namespace Mooege.Core.Cryptography.SSL
{
    public static class CertificateHelper
    {
        public static X509Certificate Certificate = null;

        static CertificateHelper()
        {
            Certificate = CreateCertificate();
        }

        private static X509Certificate CreateCertificate()
        {
            BigNumber bn = 0x10001;
            var rsa = new RSA();
            rsa.GenerateKeys(2048, bn, null, null);
            var key = new CryptoKey(rsa);

            var cert = new X509Certificate(
                new SimpleSerialNumber().Next(),
                new X509Name("Mooege"),
                new X509Name("Mooege"),
                key,
                DateTime.Now,
                DateTime.Now + TimeSpan.FromDays(365));

            cert.PrivateKey = key;
            return cert;
        }
    }
}
