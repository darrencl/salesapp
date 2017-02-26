package mvvmcross.droid.simple;


public class MvxSimpleBindingActivity
	extends mvvmcross.droid.views.MvxActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MvvmCross.Droid.Simple.MvxSimpleBindingActivity, MvvmCross.Droid, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null", MvxSimpleBindingActivity.class, __md_methods);
	}


	public MvxSimpleBindingActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MvxSimpleBindingActivity.class)
			mono.android.TypeManager.Activate ("MvvmCross.Droid.Simple.MvxSimpleBindingActivity, MvvmCross.Droid, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
