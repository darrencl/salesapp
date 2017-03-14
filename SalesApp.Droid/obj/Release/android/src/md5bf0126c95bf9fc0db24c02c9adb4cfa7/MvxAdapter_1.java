package md5bf0126c95bf9fc0db24c02c9adb4cfa7;


public class MvxAdapter_1
	extends md5bf0126c95bf9fc0db24c02c9adb4cfa7.MvxAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MvvmCross.Binding.Droid.Views.MvxAdapter`1, MvvmCross.Binding.Droid, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null", MvxAdapter_1.class, __md_methods);
	}


	public MvxAdapter_1 () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MvxAdapter_1.class)
			mono.android.TypeManager.Activate ("MvvmCross.Binding.Droid.Views.MvxAdapter`1, MvvmCross.Binding.Droid, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public MvxAdapter_1 (android.content.Context p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == MvxAdapter_1.class)
			mono.android.TypeManager.Activate ("MvvmCross.Binding.Droid.Views.MvxAdapter`1, MvvmCross.Binding.Droid, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
