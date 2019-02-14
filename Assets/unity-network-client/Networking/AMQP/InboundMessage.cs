using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Frankfort.Threading;

namespace AMQP
{
	public class InboundMessage : Message
	{
		public InboundMessage(string action = null, Dictionary<string, object> parameters = null, string data = null): base(action, parameters, data)
		{
		}
		
		public InboundMessage(string action = null, Dictionary<string, object> parameters = null, byte[] data = null): base(action, parameters, data)
		{
		}

		public virtual void Dispatch()
		{
			Client.OnReceivedCallback handler = null;
			if (Client.Handlers.TryGetValue (Action, out handler))
				Loom.DispatchToMainThread(() => { handler(this); } );
		}
	}
}