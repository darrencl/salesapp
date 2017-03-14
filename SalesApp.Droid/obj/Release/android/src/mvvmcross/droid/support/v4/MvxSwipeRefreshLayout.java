package mvvmcross.droid.support.v4;


public class MvxSwipeRefreshLayout
	extends android.support.v4.widget.SwipeRefreshLayout
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MvvmCross.Droid.Support.V4.MvxSwipeRefreshLayout, MvvmCross.Droid.Support.Core.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MvxSwipeRefreshLayout.class, __md_methods);
	}


	public MvxSwipeRefreshLayout (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == MvxSwipeRefreshLayout.class)
			mono.android.TypeManager.Activate ("MvvmCross.Droid.Support.V4.MvxSwipeRefreshLayout, MvvmCross.Droid.Support.Core.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public MvxSwipeRefreshLayout (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == MvxSwipeRefreshLayout.class)
			mono.android.TypeManager.Activate ("MvvmCross.Droid.Support.V4.MvxSwipeRefreshLayout, MvvmCross.Droid.Support.Core.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
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
