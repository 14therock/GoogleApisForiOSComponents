﻿using Foundation;
using UIKit;

using Firebase.InstanceID;
using Firebase.Analytics;
using Firebase.CloudMessaging;
using System;

namespace CloudMessagingSample
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public event EventHandler<UserInfoEventArgs> NotificationReceived;

		// class-level declarations

		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			// Override point for customization after application launch.
			// If not required for your application you can safely delete this method
			(Window.RootViewController as UINavigationController).PushViewController (new UserInfoViewController (this), true);

			// Monitor token generation
			InstanceId.Notifications.ObserveTokenRefresh (TokenRefreshNotification);

			// Register your app for remote notifications.
			var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
			var settings = UIUserNotificationSettings.GetSettingsForTypes (allNotificationTypes, null);
			UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications ();

			App.Configure ();

			return true;
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
			Messaging.SharedInstance.Disconnect ();
			Console.WriteLine ("Disconnected from FCM");
		}

		public override void DidReceiveRemoteNotification (UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			// If you are receiving a notification message while your app is in the background,
			// this callback will not be fired 'till the user taps on the notification launching the application.

			// If you disable method swizzling, you'll need to call this method. 
			// This lets FCM track message delivery and analytics, which is performed
			// automatically with method swizzling enabled.
			//Messaging.GetInstance ().AppDidReceiveMessage (userInfo);

			if (NotificationReceived == null)
				return;
			
			var e = new UserInfoEventArgs { UserInfo = userInfo };
			NotificationReceived (this, e);
		}

		// You'll need this method if you set "FirebaseAppDelegateProxyEnabled": NO in GoogleService-Info.plist
		//public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		//{
		//	InstanceId.SharedInstance.SetApnsToken (deviceToken, ApnsTokenType.Sandbox);
		//}

		void TokenRefreshNotification (object sender, NSNotificationEventArgs e)
		{
			// This method will be fired everytime a new token is generated, including the first
			// time. So if you need to retrieve the token as soon as it is available this is where that
			// should be done.
			//var refreshedToken = InstanceId.SharedInstance.Token;

			ConnectToFCM (Window.RootViewController);

			// TODO: If necessary send token to application server.
		}

		public static void ConnectToFCM (UIViewController fromViewController)
		{
			Messaging.SharedInstance.Connect (error => {
				if (error != null) {
					ShowMessage ("Unable to connect to FCM", error.LocalizedDescription, fromViewController);
				} else {
					ShowMessage ("Success!", "Connected to FCM", fromViewController);
					Console.WriteLine ($"Token: {InstanceId.SharedInstance.Token}");
				}
			});
		}

		public static void ShowMessage (string title, string message, UIViewController fromViewController, Action actionForOk = null)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				var alert = UIAlertController.Create (title, message, UIAlertControllerStyle.Alert);
				alert.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, (obj) => {
					if (actionForOk != null) {
						actionForOk ();
					}
				}));
				fromViewController.PresentViewController (alert, true, null);
			} else {
				new UIAlertView (title, message, null, "Ok", null).Show ();
			}
		}
	}
}


