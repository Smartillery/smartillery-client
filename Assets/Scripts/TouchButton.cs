using UnityEngine;
using System.Collections;

public class TouchButton : Touchable {

	public Artillery Artillery;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region implemented abstract members of Touchable

	public override void BeginTouch (Vector2 position)
	{
		Artillery.Fire();
	}

	public override void Touching (Vector2 position)
	{
	}

	public override void EndTouch ()
	{
	}

	#endregion
}
