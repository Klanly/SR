using UnityEngine;
using System;

namespace AMQP
{
	public enum ExceptionType
	{
		Timeout,
		NoInternetAccess,
		NotConnectedToServer,
		CouldNotConnect,
		CouldNotDisconnect,
		CouldNotSend
	}

	public class Exception: System.Exception
	{
		public ExceptionType Type; 

		public Exception(ExceptionType type, string message = ""): base(message)
		{
			this.Type = type;
		}

		public Exception(ExceptionType type, string message, System.Exception innerException): base(message, innerException)
		{
			this.Type = type;
		}

		public override string ToString ()
		{
			string output = Type.ToString();
			if (Message != "")
				output += "\n" + Message;
			if (InnerException != null)
				output += "\n" + InnerException.ToString();
			return output;
		}
	}
}