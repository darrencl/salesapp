package mono;

import java.io.*;
import java.lang.String;
import java.util.Locale;
import java.util.HashSet;
import java.util.zip.*;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.res.AssetManager;
import android.util.Log;
import mono.android.Runtime;

public class MonoPackageManager {

	static Object lock = new Object ();
	static boolean initialized;

	static android.content.Context Context;

	public static void LoadApplication (Context context, ApplicationInfo runtimePackage, String[] apks)
	{
		synchronized (lock) {
			if (context instanceof android.app.Application) {
				Context = context;
			}
			if (!initialized) {
				android.content.IntentFilter timezoneChangedFilter  = new android.content.IntentFilter (
						android.content.Intent.ACTION_TIMEZONE_CHANGED
				);
				context.registerReceiver (new mono.android.app.NotifyTimeZoneChanges (), timezoneChangedFilter);
				
				System.loadLibrary("monodroid");
				Locale locale       = Locale.getDefault ();
				String language     = locale.getLanguage () + "-" + locale.getCountry ();
				String filesDir     = context.getFilesDir ().getAbsolutePath ();
				String cacheDir     = context.getCacheDir ().getAbsolutePath ();
				String dataDir      = getNativeLibraryPath (context);
				ClassLoader loader  = context.getClassLoader ();

				Runtime.init (
						language,
						apks,
						getNativeLibraryPath (runtimePackage),
						new String[]{
							filesDir,
							cacheDir,
							dataDir,
						},
						loader,
						new java.io.File (
							android.os.Environment.getExternalStorageDirectory (),
							"Android/data/" + context.getPackageName () + "/files/.__override__").getAbsolutePath (),
						MonoPackageManager_Resources.Assemblies,
						context.getPackageName ());
				
				mono.android.app.ApplicationRegistration.registerApplications ();
				
				initialized = true;
			}
		}
	}

	public static void setContext (Context context)
	{
		// Ignore; vestigial
	}

	static String getNativeLibraryPath (Context context)
	{
	    return getNativeLibraryPath (context.getApplicationInfo ());
	}

	static String getNativeLibraryPath (ApplicationInfo ainfo)
	{
		if (android.os.Build.VERSION.SDK_INT >= 9)
			return ainfo.nativeLibraryDir;
		return ainfo.dataDir + "/lib";
	}

	public static String[] getAssemblies ()
	{
		return MonoPackageManager_Resources.Assemblies;
	}

	public static String[] getDependencies ()
	{
		return MonoPackageManager_Resources.Dependencies;
	}

	public static String getApiPackageName ()
	{
		return MonoPackageManager_Resources.ApiPackageName;
	}
}

class MonoPackageManager_Resources {
	public static final String[] Assemblies = new String[]{
		/* We need to ensure that "SalesApp.Droid.dll" comes first in this list. */
		"SalesApp.Droid.dll",
		"MvvmCross.Binding.dll",
		"MvvmCross.Binding.Droid.dll",
		"MvvmCross.Core.dll",
		"MvvmCross.Droid.dll",
		"MvvmCross.Droid.Shared.dll",
		"MvvmCross.Droid.Support.Core.UI.dll",
		"MvvmCross.Droid.Support.Fragment.dll",
		"MvvmCross.Localization.dll",
		"MvvmCross.Platform.dll",
		"MvvmCross.Platform.Droid.dll",
		"MvvmCross.Plugins.DownloadCache.dll",
		"MvvmCross.Plugins.DownloadCache.Droid.dll",
		"MvvmCross.Plugins.File.dll",
		"MvvmCross.Plugins.File.Droid.dll",
		"PCLStorage.Abstractions.dll",
		"PCLStorage.dll",
		"Plugin.Connectivity.Abstractions.dll",
		"Plugin.Connectivity.dll",
		"Plugin.CurrentActivity.dll",
		"Plugin.Geolocator.Abstractions.dll",
		"Plugin.Geolocator.dll",
		"Plugin.Permissions.Abstractions.dll",
		"Plugin.Permissions.dll",
		"SQLite.Net.dll",
		"SQLite.Net.Platform.XamarinAndroid.dll",
		"System.Collections.Immutable.dll",
		"ZXing.Net.Mobile.Core.dll",
		"zxing.portable.dll",
		"ZXingNetMobile.dll",
		"Xamarin.Android.Support.Compat.dll",
		"Xamarin.Android.Support.Core.UI.dll",
		"Xamarin.Android.Support.Core.Utils.dll",
		"Xamarin.Android.Support.Media.Compat.dll",
		"Xamarin.Android.Support.Fragment.dll",
		"Xamarin.Android.Support.Transition.dll",
		"Xamarin.Android.Support.v4.dll",
		"Xamarin.Android.Support.v7.RecyclerView.dll",
		"Xamarin.Android.Support.Vector.Drawable.dll",
		"Xamarin.Android.Support.Animated.Vector.Drawable.dll",
		"Xamarin.Android.Support.v7.AppCompat.dll",
		"Xamarin.Android.Support.Design.dll",
		"Xamarin.GooglePlayServices.Basement.dll",
		"Xamarin.GooglePlayServices.Tasks.dll",
		"Xamarin.Firebase.Common.dll",
		"Xamarin.Firebase.Iid.dll",
		"Xamarin.GooglePlayServices.Base.dll",
		"Xamarin.Firebase.Messaging.dll",
		"SalesApp.Core.dll",
		"Newtonsoft.Json.dll",
	};
	public static final String[] Dependencies = new String[]{
	};
	public static final String ApiPackageName = null;
}
