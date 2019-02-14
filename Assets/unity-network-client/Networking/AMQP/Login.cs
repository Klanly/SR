using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AMQP
{
	public class Login : Request
	{
		public Login(string action = null, Dictionary<string, object> parameters = null, string data = null): base(action, parameters, data, null)
		{
		
		}
		
		public Login(string action = null, Dictionary<string, object> parameters = null, byte[] data = null): base(action, parameters, data, null)
		{

		}

		public override void SendInternal()
		{
			base.SendToSpecificExchange(Client.LoginExchange);
			if (IsSent)
			{
				Client.Unreplied.Add(CorrelationID , this);
			}
		}
	}
}
