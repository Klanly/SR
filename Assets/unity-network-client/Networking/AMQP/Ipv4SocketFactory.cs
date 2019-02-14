using System;
using System.Net;
using System.Net.Sockets;

namespace AMQP
{
	public static class Ipv4SocketFactory
	{
		public static TcpClient GetSocket(AddressFamily addressFamily)		
		{
			TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork);	
			tcpClient.NoDelay = true;
			return tcpClient;
		}
	}
}