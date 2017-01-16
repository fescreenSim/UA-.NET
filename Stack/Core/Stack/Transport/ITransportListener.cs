/* Copyright (c) 1996-2016, OPC Foundation. All rights reserved.

   The source code in this file is covered under a dual-license scenario:
     - RCL: for OPC Foundation members in good-standing
     - GPL V2: everybody else

   RCL license terms accompanied with this source code. See http://opcfoundation.org/License/RCL/1.00/

   GNU General Public License as published by the Free Software Foundation;
   version 2 of the License are accompanied with this source code. See http://opcfoundation.org/License/GPLv2

   This source code is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Opc.Ua
{
    /// <summary>
    /// This is an interface to a listener which supports UA binary encoding.
    /// </summary>
    public interface ITransportListener : IDisposable
    {
        /// <summary>
        /// Opens the listener and starts accepting connection.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="settings">The settings to use when creating the listener.</param>
        /// <param name="callback">The callback to use when requests arrive via the channel.</param>
        /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
        /// <exception cref="ServiceResultException">Thrown if any communication error occurs.</exception>
        void Open(
            Uri baseAddress,
            TransportListenerSettings settings,
            ITransportListenerCallback callback);

        /// <summary>
        /// Closes the listener and stops accepting connection.
        /// </summary>
        /// <exception cref="ServiceResultException">Thrown if any communication error occurs.</exception>
        void Close();

        /// <summary>
        /// Raised when a new connection is waiting for a client.
        /// </summary>
        event EventHandler<ConnectionWaitingEventArgs> ConnectionWaiting;

        /// <summary>
        /// Raised when a monitored connection's status changed.
        /// </summary>
        event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;

        /// <summary>
        /// Creates a connection to a client. 
        /// </summary>
        void CreateConnection(Uri url);

        /// <summary>
        /// The protocol supported by the listener.
        /// </summary>
        string UriScheme { get; }
    }

    /// <summary>
    /// The arguments passed to the ConnectionWaiting event. 
    /// </summary>
    public interface ITransportWaitingConnection 
    {
        string ServerUri { get; }

        Uri EndpointUrl { get; }

        object Handle { get; set; }
    }

    /// <summary>
    /// The arguments passed to the ConnectionWaiting event. 
    /// </summary>
    public class ConnectionWaitingEventArgs : EventArgs, ITransportWaitingConnection
    {
        internal ConnectionWaitingEventArgs(string serverUrl, Uri endpointUrl, object socket)
        {
            ServerUri = serverUrl;
            EndpointUrl = endpointUrl;
            Socket = socket;
            Accepted = false;
        }

        public string ServerUri { get; private set; }

        public Uri EndpointUrl { get; private set; }

        public EndpointDescription Endpoint { get; set; }

        public object Handle { get { return Socket; } set { } }

        internal object Socket { get; }

        public bool Accepted { get; set; }
    }

    /// <summary>
    /// The arguments passed to the ConnectionStatus event. 
    /// </summary>
    public class ConnectionStatusEventArgs : EventArgs
    {
        internal ConnectionStatusEventArgs(Uri endpointUrl, ServiceResult channelStatus, bool closed)
        {
            EndpointUrl = endpointUrl;
            ChannelStatus = channelStatus;
            Closed = closed;
        }

        public Uri EndpointUrl { get; private set; }

        public ServiceResult ChannelStatus { get; private set; }

        public bool Closed { get; private set; }
    }
}