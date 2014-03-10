using UnityEngine;
using System.Collections;

public class TouchZoom : Touchable {

	#region implemented abstract members of Touchable

	public override void DoubleTouch (Vector2 touch1, Vector2 touch2)
	{
		throw new System.NotImplementedException ();
	}

	public override void BeginTouch (Vector2 position)
	{
		throw new System.NotImplementedException ();
	}

	public override void Touching (Vector2 position)
	{
		throw new System.NotImplementedException ();
	}

	public override void EndTouch ()
	{
		throw new System.NotImplementedException ();
	}

#endregion
	
}
