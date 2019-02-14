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
	public class Reply : InboundMessage
	{
		public string CorrelationID { get; protected set; }
		
		public Reply(string correlationId, string action = null, Dictionary<string, object> parameters = null, string data = null): base(action, parameters, data)
		{
			CorrelationID = correlationId;
		}

		public Reply(string correlationId, string action = null, Dictionary<string, object> parameters = null, byte[] data = null): base(action, parameters, data)
		{
			CorrelationID = correlationId;
		}

		public override void Dispatch()
		{
			Request request = null;
			if (Client.Unreplied.TryGetValue(CorrelationID, out request))
			{
                if(Client.GetReplies)
                {
				    lock (Client.Unreplied)
					    Client.Unreplied.Remove(CorrelationID);

                    if (request.onReply != null)
                        Loom.DispatchToMainThread(() => { request.onReply(request, this); });
                }
				
			}
			else
				base.Dispatch();
		}

		public override string ToString()
		{
			return "CorrelationID: " + CorrelationID + "\n" + base.ToString();
		}
	}
}