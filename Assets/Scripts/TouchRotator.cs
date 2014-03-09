using UnityEngine;
using System.Collections;

public class TouchRotator : Touchable {

	public Vector3 RotationAxis;
	public bool yAxis;
	public bool xAxis;
	public float touchSpeed;

	private Vector2 _lastPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region implemented abstract members of Touchable

	public override void BeginTouch (Vector2 position)
	{
		_lastPosition = position;
	}

	public override void Touching (Vector2 position)
	{
		Vector2 deltaVector = _lastPosition - position;

		if(xAxis)
			transform.Rotate(RotationAxis * deltaVector.x * touchSpeed * Time.deltaTime, Space.World);
		if(yAxis)
			transform.Rotate(RotationAxis * -deltaVector.y * touchSpeed * Time.deltaTime, Space.World);

		_lastPosition = position;
	}

	public override void EndTouch ()
	{

	}

	#endregion
}
