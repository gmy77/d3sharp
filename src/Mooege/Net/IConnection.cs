/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Mooege.Net.MooNet.Packets;

using OpenSSL;

namespace Mooege.Net
{
    /// <summary>
    /// Connection interface.
    /// </summary>
    public interface IConnection
    {
        // [D3Inferno]
        // Returns true if the connection is now encrypted.
        // This will be true once Tls handshaking is complete and successful.
        bool IsEncrypted { get; }

        // Returns true is the EncryptRequest message has been sent to the client.
        // The last unencrypted packet should be the EncryptRequest NoData service response.
        bool IsEncryptRequestSent { get; set; }

        // Returns true if the connection is trying to establish a Tls connection.
        bool IsTlsHandshaking { get; set; }

        /// The underlying TLS stream. Required for encrypted communication.
        SslStream TLSStream { get; }

        // Notify the Connection of the encrypted TLSStream.
        void SetEncrypted(SslStream TLSStream);

        // Notify that Tls Authentication is complete (but perhaps not successful) so that it can once
        // again start listening for client data, but from now on, using the SslStream if successful.
        void TlsAuthenticationComplete();

        // Wrapper for the Send method that will send the data either to the
        // Socket (unecnrypted) or to the TLSStream (encrypted).
        // Note that the flags will be ignored for TLSStream.
        int _Send(byte[] buffer, int start, int count, SocketFlags flags);

        // Read bytes from the Sokcet into the buffer in a non-blocking call.
        // This allows us to read no more than the specified count number of bytes.
        int Receive(int start, int count);

        // Expose the RecvBuffer.
        byte[] RecvBuffer { get; }


        /// <summary>
        /// Returns true if there exists an active connection.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Returns remote endpoint.
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Returns local endpoint.
        /// </summary>
        IPEndPoint LocalEndPoint { get; }

        /// <summary>
        /// Gets or sets bound client.
        /// </summary>
        IClient Client { get; set; }

        /// <summary>
        /// Gets underlying socket.
        /// </summary>
        Socket Socket { get; }

        /// <summary>
        /// Sends a <see cref="PacketOut"/> to remote endpoint.
        /// </summary>
        /// <param name="packet"><see cref="PacketOut"/> to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(PacketOut packet);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer, SocketFlags flags);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="start">Start index to read from buffer.</param>
        /// <param name="count">Count of bytes to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer, int start, int count);

        /// <summary>
        /// Sends byte buffer to remote endpoint.
        /// </summary>
        /// <param name="buffer">Byte buffer to send.</param>
        /// <param name="start">Start index to read from buffer.</param>
        /// <param name="count">Count of bytes to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(byte[] buffer, int start, int count, SocketFlags flags);

        /// <summary>
        /// Sends an enumarable byte buffer to remote endpoint.
        /// </summary>
        /// <param name="data">Enumrable byte buffer to send.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(IEnumerable<byte> data);

        /// <summary>
        /// Sends an enumarable byte buffer to remote endpoint.
        /// </summary>
        /// <param name="data">Enumrable byte buffer to send.</param>
        /// <param name="flags">Sockets flags to use.</param>
        /// <returns>Returns count of sent bytes.</returns>
        int Send(IEnumerable<byte> data, SocketFlags flags);

        /// <summary>
        /// Kills the connection to remote endpoint.
        /// </summary>
        void Disconnect();
    }
}

