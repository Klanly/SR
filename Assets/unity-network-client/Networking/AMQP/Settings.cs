using System;
using RabbitMQ.Client;

namespace AMQP
{
	public class Settings
	{
		public string hostName { get; protected set; }
		public IProtocol protocol { get; protected set; }
		public String virtualHost { get; protected set; }
		public int? port { get; protected set; }
		public String userName { get; protected set; }
		public String password { get; protected set; }
		public ushort? heartbeat { get; protected set; }
		
		public Settings(string hostName)
		{
			this.hostName = hostName;
			virtualHost = null;
			protocol = null;
			port = null;
			userName = null;
			password = null;
			heartbeat = null;
		}
		
		public Settings Protocol(IProtocol protocol)
		{
			this.protocol = protocol;
			return this;
		}
		
		public Settings VirtualHost(string virtualHost)
		{
			this.virtualHost = virtualHost;
			return this;
		}
		
		public Settings Port(int port)
		{
			this.port = port;
			return this;
		}
		
		public Settings Username(string userName)
		{
			this.userName = userName;
			return this;
		}
		
		public Settings Password(string password)
		{
			this.password = password;
			return this;
		}
		
		public Settings Heartbeat(ushort heartbeat)
		{
			this.heartbeat = heartbeat;
			return this;
		}
		
		public override string ToString()
		{
			string output = "";
			output += "[";
			output += "Hostname: " + hostName;
			if (virtualHost != null)
				output += ", VirtualHost: " + virtualHost;
			if (protocol != null)
				output += ", Protocol: " + protocol.ToString();
			if (port != null)
				output += ", Port: " + port;
			if (userName != null)
				output += ", Username: " + userName;
			if (password != null)
				output += ", Password: " + password;
			if (heartbeat != null)
				output += ", Heartbeat: " + heartbeat;
			output += "]";
			return output;
		}
	}
}