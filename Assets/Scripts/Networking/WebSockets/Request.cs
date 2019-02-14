using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace WebSocket_Client
{
    public class Request : Message
    {

        public delegate void OnReplyCallback(Reply reply);

        public OnReplyCallback onReply { get; set; }

        public delegate void OnFailureCallback(String failure);
        
        public OnFailureCallback onFailure { get; set; }

        public string CorrelationID { get; protected set; }

        public  float ElaspedTime;


        public Request(string action, Dictionary<string,object> data):base(action,string.Empty)
        {
            CorrelationID = Guid.NewGuid().ToString();
            data.Add("correlationId",CorrelationID);
            data.Add("action",action);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            this.Action = action;
            this.Data = Message.GetBytes(json);
        }

        public virtual Request OnReply(OnReplyCallback onReply)
        {
            this.onReply = onReply;
            return this;
        }

        public virtual Request OnFailure(OnFailureCallback onFailure)
        {
            this.onFailure = onFailure;
            Debug.Log("Time Out Occur:  " + onFailure);
            return this;
        }
    
        public void Send()
        {
            Client.Instance.Send(this);
        }
    
        public override string ToString()
        {
            return "CorrelationID: " + CorrelationID + "\n" + base.ToString();
        }
    }

}