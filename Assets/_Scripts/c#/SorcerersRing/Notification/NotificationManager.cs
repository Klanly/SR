// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NotificationManager
{
	public enum NotificationType
	{
		Upgrade,
		Spirit,
		Transmutation
	}

	public NotificationManager ()
	{
		
	
	}

	public static void RequestPermission ()
	{
		#if UNITY_IOS
		ISN_LocalNotificationsController.Instance.RequestNotificationPermissions ();
		#endif
	}

	private void OnNotificationScheduleResult (SA.Common.Models.Result result)
	{
		#if UNITY_IOS
//		ISN_LocalNotificationsController.OnNotificationScheduleResult -= OnNotificationScheduleResult;
		
		string msg = string.Empty;
		
		if(result.IsSucceeded) {
			msg = "Notification was successfully scheduled";    
		} else {
			msg = "Notification scheduling failed";
		}
		
		IOSMessage.Create("Notification Schedule Result", msg);
		#endif
	}

	public static void SendLocalNotification (DateTime time, NotificationType type)
	{

		string message = "";
		string action = "";
		if (type == NotificationType.Upgrade) {
			action = "Upgrade";
			message = "Your upgrade is complete";
		} else if (type == NotificationType.Spirit) {
			action = "Spirit";
			message = "Your spirit upgrade is complete";
		} else if (type == NotificationType.Transmutation) {
			action = "Transmutation";
			message = "Your transmutation upgrade is complete";
		}

		#if UNITY_ANDROID
		List<LocalNotificationTemplate> PendingNotofications;
		PendingNotofications = AndroidNotificationManager.instance.LoadPendingNotifications ();
		foreach (LocalNotificationTemplate notification in PendingNotofications) {
			if (notification.title.Equals (action) && !notification.IsFired) {
				return;
			}
		}

		var diff = time - DateTime.Now;
		int NotificationId = AndroidNotificationManager.instance.ScheduleLocalNotification (action, message, (int)diff.TotalSeconds);
		PlayerPrefs.SetInt (action, NotificationId);
			
		#endif

		#if UNITY_IOS
		List<ISN_LocalNotification> PendingNotifications;
		PendingNotifications = ISN_LocalNotificationsController.Instance.LoadPendingNotifications();

		foreach(ISN_LocalNotification notification in PendingNotifications) {
			if(notification.Data.ToString().Equals(type.ToString())) {
				Debug.LogError("Already scheduled a notification so returning");
				return;
			}
		}

//		ISN_LocalNotificationsController.OnNotificationScheduleResult += OnNotificationScheduleResult;
		
		ISN_LocalNotification notification1 =  new ISN_LocalNotification(time,message, false);
		notification1.SetData(type.ToString());
		notification1.Schedule();
		#endif

	}

	public static void CancelLocalNotification (NotificationType type)
	{

		#if UNITY_ANDROID
		string action = type.ToString ();
		List<LocalNotificationTemplate> PendingNotofications;
		PendingNotofications = AndroidNotificationManager.instance.LoadPendingNotifications ();
		foreach (LocalNotificationTemplate notification in PendingNotofications) {
			if (notification.title.Equals (action) && !notification.IsFired) {
				int notificationId = PlayerPrefs.GetInt (action, -1);
				if (notificationId != -1)
					AndroidNotificationManager.Instance.CancelLocalNotification (notificationId);
			}
		}
		#endif

		#if UNITY_IOS
		List<ISN_LocalNotification> PendingNotifications;
		PendingNotifications = ISN_LocalNotificationsController.Instance.LoadPendingNotifications();
		
		foreach(ISN_LocalNotification notification in PendingNotifications) {
			if(notification.Data.ToString().Equals(type.ToString())) {
				ISN_LocalNotificationsController.Instance.CancelLocalNotificationById(notification.Id);
				break;
			}
		}
		#endif

	}

}

