﻿using UnityEngine;
using System.Collections;

public class ElevationControl : MonoBehaviour {

	public float scalar;
	public Artillery Target;
	public TextMesh Label;
	private Quaternion _lastRotation;

	// Use this for initialization
	void Start () {		
		_lastRotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {		
		float dif = RotationHelper.GetSignedRotation(_lastRotation, transform.rotation, true);

		Target.elevation += -dif * scalar;
		_lastRotation = transform.rotation;
	}
	void LateUpdate()
	{
		if(Label != null)
		{
			Label.text = Target.elevation.ToString();
		}
	}
}
