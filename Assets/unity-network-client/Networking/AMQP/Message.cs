using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AMQP
{
	public class Message
	{
		public string Action { get; protected set; }
		public Dictionary<string, object> Parameters { get; protected set; }
		public byte[] Data { get; protected set; }

		public Message(string action = null, Dictionary<string, object> parameters = null, string data = null) : this(action, parameters, GetBytes(data))
		{
		}
		
		public Message(string action = null, Dictionary<string, object> parameters = null, byte[] data = null)
		{
			this.Action = action;
			this.Parameters = parameters;
			this.Data = data;
		}

		public override string ToString()
		{
			string output = "";

			if (Action != null)
				output += "Action: " + Action + "\n";

			if (Parameters != null)
			{
				output += "Parameters: {";
				foreach(KeyValuePair<string, object> pair in Parameters)
				{
					object value = (pair.Value is Byte[]) ? GetString((byte[]) pair.Value) : pair.Value;
					output += pair.Key + ": " + value + ", ";
				}
				output += "}\n";
			}

			if (Data != null)
				output += "Data: " + DataAsString + "\n";

			return output;
		}

		public string DataAsString
		{
			get
			{
				return GetString(Data);
			}
		}

		public static byte[] GetBytes(string str)
		{
			if (str != null)
				return System.Text.Encoding.UTF8.GetBytes(str);
			else
				return null;
		}
		
		public static string GetString(byte[] bytes)
		{
			if (bytes != null)
				return System.Text.Encoding.UTF8.GetString(bytes);
			else
				return null;
		}
	}
}