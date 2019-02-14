using UnityEngine;
using System.Collections;

using System;

namespace Game.UI
{
    public class chatrow : MonoBehaviour
    {
        public UILabel message;
        public UILabel userName;
        public UILabel memberType;
        public UIButton userNameButton;
        public UILabel _time;
        private DateTime pushTime;
		private long _timeStamp;

        void Start()
        {
//			StartCoroutine("UpdateTimer");
			InvokeRepeating("Refresh", 60, 60);
        }

		public DateTime PushTime
		{
			set
			{
				pushTime = value;
				Refresh();
			}
			get
			{
				return pushTime;
			}
		}
		public long TimeStamp
		{
			set
			{
				_timeStamp = value;
				Refresh();
			}
			get
			{
				return _timeStamp;
			}
		}

        public void Refresh()
        {
			long serverTime = InitGameVersions.instance.ServerTime;
			var posixTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);
			TimeSpan diff = posixTime.AddMilliseconds(serverTime) - posixTime.AddMilliseconds(TimeStamp);
			int totalDays = (int)diff.TotalDays;
			int totalHours = (int)diff.TotalHours;
			int totalMinutes = (int)diff.TotalMinutes;
			if(totalDays > 0) {
				_time.text = totalDays + "d";
			} else if(totalHours > 0) {
				_time.text = totalHours + "h";
			} else if(totalMinutes > 0) {
				_time.text = totalMinutes + "m";
			} else if(totalMinutes <= 0) {
				_time.text = "Just Now";
			}
//			Debug.LogError(" time - "+_time.text);
        }
    }
}