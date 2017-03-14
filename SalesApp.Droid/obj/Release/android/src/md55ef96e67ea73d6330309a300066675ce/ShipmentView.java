package md55ef96e67ea73d6330309a300066675ce;


public class ShipmentView
	extends mvvmcross.droid.views.MvxActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"n_onStart:()V:GetOnStartHandler\n" +
			"n_onPause:()V:GetOnPauseHandler\n" +
			"";
		mono.android.Runtime.register ("SalesApp.Droid.Views.ShipmentView, SalesApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ShipmentView.class, __md_methods);
	}


	public ShipmentView () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ShipmentView.class)
			mono.android.TypeManager.Activate ("SalesApp.Droid.Views.ShipmentView, SalesApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();


	public void onStart ()
	{
		n_onStart ();
	}

	private native void n_onStart ();


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();

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
