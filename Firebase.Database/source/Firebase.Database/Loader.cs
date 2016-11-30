﻿using System;

namespace Firebase.Database
{
	public class Loader
	{
		static Loader ()
		{
			Firebase.InstanceID.Loader.ForceLoad ();
			Firebase.Analytics.Loader.ForceLoad ();
		}

		public static void ForceLoad () { }
	}
}

namespace ApiDefinition
{
	partial class Messaging
	{
		static Messaging ()
		{
			Firebase.Database.Loader.ForceLoad ();
		}
	}
}
