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
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Mooege.Core.Cryptography.SSL
{
    public static class CertificateHelper
    {
        public const string SertificateFile = "mooege.pfx";
        public static X509Certificate Certificate = null;

        static CertificateHelper()
        {
            if (!CertificateExists())
                Create();

            Certificate = new X509Certificate2(SertificateFile, "mooege");
        }

        private static void Create()
        {
            byte[] certificate = CertificateCreator.CreateSelfSignCertificatePfx("CN=mooege.org", DateTime.Parse("2011-01-01"), DateTime.Parse("2013-01-01"), "mooege");

            using (var writer = new BinaryWriter(File.Open(SertificateFile, FileMode.Create)))
            {
                writer.Write(certificate);
            }
        }

        public static bool CertificateExists()
        {
            return File.Exists(SertificateFile);
        }
    }
}
