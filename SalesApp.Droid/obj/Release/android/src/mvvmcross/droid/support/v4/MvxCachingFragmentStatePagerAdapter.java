package mvvmcross.droid.support.v4;


public class MvxCachingFragmentStatePagerAdapter
	extends mvvmcross.droid.support.v4.MvxCachingFragmentPagerAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCount:()I:GetGetCountHandler\n" +
			"n_getPageTitle:(I)Ljava/lang/CharSequence;:GetGetPageTitle_IHandler\n" +
			"";
		mono.android.Runtime.register ("MvvmCross.Droid.Support.V4.MvxCachingFragmentStatePagerAdapter, MvvmCross.Droid.Support.Fragment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MvxCachingFragmentStatePagerAdapter.class, __md_methods);
	}


	public MvxCachingFragmentStatePagerAdapter () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MvxCachingFragmentStatePagerAdapter.class)
			mono.android.TypeManager.Activate ("MvvmCross.Droid.Support.V4.MvxCachingFragmentStatePagerAdapter, MvvmCross.Droid.Support.Fragment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public MvxCachingFragmentStatePagerAdapter (android.support.v4.app.FragmentManager p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == MvxCachingFragmentStatePagerAdapter.class)
			mono.android.TypeManager.Activate ("MvvmCross.Droid.Support.V4.MvxCachingFragmentStatePagerAdapter, MvvmCross.Droid.Support.Fragment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Support.V4.App.FragmentManager, Xamarin.Android.Support.Fragment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();


	public java.lang.CharSequence getPageTitle (int p0)
	{
		return n_getPageTitle (p0);
	}

	private native java.lang.CharSequence n_getPageTitle (int p0);

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
