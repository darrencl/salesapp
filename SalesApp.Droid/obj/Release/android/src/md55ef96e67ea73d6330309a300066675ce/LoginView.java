package md55ef96e67ea73d6330309a300066675ce;


public class LoginView
	extends mvvmcross.droid.views.MvxActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onUserLeaveHint:()V:GetOnUserLeaveHintHandler\n" +
			"";
		mono.android.Runtime.register ("SalesApp.Droid.Views.LoginView, SalesApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LoginView.class, __md_methods);
	}


	public LoginView () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LoginView.class)
			mono.android.TypeManager.Activate ("SalesApp.Droid.Views.LoginView, SalesApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onUserLeaveHint ()
	{
		n_onUserLeaveHint ();
	}

	private native void n_onUserLeaveHint ();

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
