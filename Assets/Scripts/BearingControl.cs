using UnityEngine;
using System.Collections;

public class BearingControl : MonoBehaviour {

	public float scalar;
	public Artillery Target;
	public TextMesh Label;
	private Quaternion _lastRotation;

	// Use this for initialization
	void Start () {
		_lastRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		float dif = RotationHelper.GetSignedRotation(_lastRotation, transform.rotation, false);

		Target.bearing += dif * scalar;
		_lastRotation = transform.rotation;
	}
	void LateUpdate()
	{
		if(Label != null)
		{
			Label.text = Target.bearing.ToString();
		}
	}
	
}
