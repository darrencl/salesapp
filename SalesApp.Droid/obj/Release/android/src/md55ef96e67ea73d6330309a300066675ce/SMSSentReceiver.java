package md55ef96e67ea73d6330309a300066675ce;


public class SMSSentReceiver
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("SalesApp.Droid.Views.SMSSentReceiver, SalesApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SMSSentReceiver.class, __md_methods);
	}


	public SMSSentReceiver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SMSSentReceiver.class)
			mono.android.TypeManager.Activate ("SalesApp.Droid.Views.SMSSentReceiver, SalesApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
