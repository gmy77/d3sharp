/*
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
using OpenSSL.X509;

namespace Mooege.Core.Cryptography.SSL
{
    public static class CertificateHelper
    {
        // OpenSSL & OpenSSL.net implementation:

        public static X509Chain serverCAChain = null;
        public static X509Certificate serverCertificate = null;

        static CertificateHelper()
        {
            string serverCertPath = @"server.pfx";
            string serverPrivateKeyPassword = "p@ssw0rd";
            string caFilePath = "ca_chain.pem";

            serverCAChain = LoadCACertificateChain(caFilePath);
            serverCertificate = LoadPKCS12Certificate(serverCertPath, serverPrivateKeyPassword);
        }

        private static X509Certificate LoadPKCS12Certificate(string certFilename, string password)
        {
            using (BIO certFile = BIO.File(certFilename, "r"))
            {
                return X509Certificate.FromPKCS12(certFile, password);
            }
        }

        private static X509Chain LoadCACertificateChain(string caFilename)
        {
            using (BIO bio = BIO.File(caFilename, "r"))
            {
                return new X509Chain(bio);
            }
        }

        // Microsoft SChannel Implementation:
        //public const string SertificateFile = "mooege.pfx";
        //public static X509Certificate Certificate = null;

        //if (!CertificateExists())
        //    Create();

        //Certificate = new X509Certificate2(SertificateFile, "mooege");

        //private static void Create()
        //{
        //    byte[] certificate = CertificateCreator.CreateSelfSignCertificatePfx("CN=mooege.org", DateTime.Parse("2011-01-01"), DateTime.Parse("2013-01-01"), "mooege");

        //    using (var writer = new BinaryWriter(File.Open(SertificateFile, FileMode.Create)))
        //    {
        //        writer.Write(certificate);
        //    }
        //}

        //public static bool CertificateExists()
        //{
        //    return File.Exists(SertificateFile);
        //}
    }
}
