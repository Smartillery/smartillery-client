using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LoginUI : MonoBehaviour {

	public string UserName;
	public string Password;

	public RectOffset LabelOffset;
	public Rect UserRect;
	public Rect PassRect;
	public Rect ButtonRect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{

		GUI.Label(LabelOffset.Add(UserRect), "Username");
		UserName = GUI.TextArea(UserRect, UserName);
		GUI.Label(LabelOffset.Add(PassRect), "Password");
		Password = GUI.PasswordField(PassRect, Password, '*');

		if(GUI.Button(ButtonRect, "Login"))
		{
			Application.LoadLevel("Game");
		}
	}
}
