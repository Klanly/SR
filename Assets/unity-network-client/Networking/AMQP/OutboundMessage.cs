using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace AMQP
{
	public class OutboundMessage : Message
	{
		public delegate void OnSentCallback (OutboundMessage message);

		public delegate void OnRetryCallback (OutboundMessage message);

		public delegate void OnFailedCallback (OutboundMessage message);

		public OnSentCallback onSent { get; protected set; }

		public OnRetryCallback onRetry { get; protected set; }

		public OnFailedCallback onFailed { get; protected set; }

		public bool IsSent { get; protected set; }

		public int IsRetry { get; protected set; }

		public float QueueTime { get; protected set; }

		public float SentTime { get; protected set; }

		protected IBasicProperties properties;

		public OutboundMessage (string action = null, Dictionary<string, object> parameters = null, string data = null): base(action, parameters, data)
		{
			IsSent = false;
			IsRetry = 0;
			SentTime = 0;
			QueueTime = 0;
//			PrepareProperties ();
		}

		public OutboundMessage (string action = null, Dictionary<string, object> parameters = null, byte[] data = null): base(action, parameters, data)
		{
			IsSent = false;
			IsRetry = 0;
			SentTime = 0;
			QueueTime = 0;
//			PrepareProperties ();
		}

		protected virtual void PrepareProperties ()
		{
			properties = Client.Channel.CreateBasicProperties ();
			properties.ReplyTo = Client.ReceiveQueue;
			properties.UserId = Client.ConnectionFactory.UserName;
			Prepare (properties);
		}

		public virtual OutboundMessage OnSent (OnSentCallback onSent)
		{
			this.onSent = onSent;
			return this;
		}
        
		public virtual OutboundMessage OnRetry (OnRetryCallback onRetry)
		{
			this.onRetry = onRetry;
			return this;
		}
        
		public virtual OutboundMessage OnFailed (OnFailedCallback onFailed)
		{
			this.onFailed = onFailed;
			return this;
		}

		public virtual void Send ()
		{
			//Farhan
			if (Client.Connection == null) {
				onFailed (this);
				return;
			}
			//

			lock (Client.Unsent) {
				QueueTime = Client.GetTime;
				Client.Unsent.Enqueue (this);
			}
		}

		public virtual void SendInternal ()
		{
			SendToSpecificExchange(Client.CommunicationExchange);
		}

		protected virtual void SendToSpecificExchange(string Exchange)
		{
			try {
				PrepareProperties();
				
				Client.Channel.BasicPublish(Exchange, "", false, false, properties, Data);
				IsSent = true;
				SentTime = Client.GetTime;
				
				Client.Log ("Sent ", ToString ());
				
				if (onSent != null)
					Loom.DispatchToMainThread (() => {
						onSent (this); });
				
			} catch (System.Exception ex) {
				Client.Log (new AMQP.Exception (AMQP.ExceptionType.CouldNotSend, ToString (), ex));
                IsSent = false;
                SentTime = 0;
                IsRetry++;
                Client.Unsent.Enqueue (this);
            }
        }
        
		public void Register()
		{
			SendToSpecificExchange(Client.LoginExchange);
		}

		public void Login()
		{
			SendToSpecificExchange(Client.LoginExchange);
		}

		protected virtual void Prepare (IBasicProperties properties)
		{
			if (Parameters == null)
				Parameters = new Dictionary<string, object> ();
            
			Parameters ["action"] = Action;
			Parameters ["version"] = "4";
            
			//properties.Type = Action;
			properties.Headers = Parameters;
		}
	}
}