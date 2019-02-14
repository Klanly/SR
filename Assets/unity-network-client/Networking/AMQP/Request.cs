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
	public class Request : OutboundMessage
	{
		public delegate void OnReplyCallback(Request request, Reply reply);
		public OnReplyCallback onReply { get; protected set; }

		public ServerRequest serverRequest;

		public string CorrelationID { get; protected set; }

		public Request(string action = null, Dictionary<string, object> parameters = null, string data = null): base(action, parameters, data)
		{
			CorrelationID = Guid.NewGuid().ToString();
		}

		public Request(string action = null, Dictionary<string, object> parameters = null, string data = null, ServerRequest serverRequest = null): base(action, parameters, data)
		{
			CorrelationID = Guid.NewGuid().ToString();
			this.serverRequest = serverRequest;
		}
		
		public Request(string action = null, Dictionary<string, object> parameters = null, byte[] data = null): base(action, parameters, data)
		{
			CorrelationID = Guid.NewGuid().ToString();
		}

		public Request(string action = null, Dictionary<string, object> parameters = null, byte[] data = null, ServerRequest serverRequest = null): base(action, parameters, data)
		{
			CorrelationID = Guid.NewGuid().ToString();
			this.serverRequest = serverRequest;
		}

		public virtual Request OnReply(OnReplyCallback onReply)
		{
			this.onReply = onReply;
			return this;
		}

		public override void SendInternal()
		{
			base.SendInternal();
            if (IsSent)
            {
                Client.Unreplied.Add(CorrelationID , this);
            }
		}

		protected override void Prepare(IBasicProperties properties)
		{
			base.Prepare (properties);
			properties.CorrelationId = CorrelationID;
		}

		public override string ToString()
		{
			return "CorrelationID: " + CorrelationID + "\n" + base.ToString();
		}
	}
}