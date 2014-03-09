using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Threading;
using SmartilleryUnityWrapper;

[ExecuteInEditMode]
public class LoginUI : MonoBehaviour {

	public SmartilleryApiSim api;

	public GUISkin Skin;

	public string UserName;
	public string Password;
	public string ErrorString;

	public RectOffset LabelOffset;
	public Rect UserRect;
	public Rect PassRect;
	public Rect ButtonRect;
	public Rect ErrorRect;

	private ActionThread _loginThread;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if(_loginThread != null)
		{
			if(_loginThread.Complete)
			{
				try
				{
					_loginThread.Wait();
					Application.LoadLevel("Game");
				}
				catch(UnAuthorizedException)
				{
					ErrorString = "Invalid Password";
				}
				catch(ConnectionTimeoutException)
				{
					ErrorString = "Can not connect to server";
				}
			}
		}
	}

	void OnGUI()
	{
		GUI.skin = Skin;
		GUI.Label(LabelOffset.Add(UserRect), "Username");
		UserName = GUI.TextField(UserRect, UserName);
		GUI.Label(LabelOffset.Add(PassRect), "Password");
		Password = GUI.PasswordField(PassRect, Password, '*');
		GUI.Label(ErrorRect, ErrorString);

		if(GUI.Button(ButtonRect, "Login"))
		{
			ErrorString = "Logging in...";
			_loginThread = new ActionThread(() => {
				api.Login(UserName, Password);
			});
		}
	}
}
