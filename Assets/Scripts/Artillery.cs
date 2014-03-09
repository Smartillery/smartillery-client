using UnityEngine;
using System.Collections;

public class Artillery : MonoBehaviour {

	public float bearing;
	public float elevation;

	public GameObject Turret;
	public GameObject Barrel;

	// Use this for initialization
	void Start () {
		bearing = 0;
		elevation = 0;
	}

	// Update is called once per frame
	void Update () {
	
		if(bearing / 360 > 0)
			bearing %= 360;
		elevation = Mathf.Clamp(elevation, 0, 89);

		Barrel.transform.localRotation = Quaternion.AngleAxis(elevation, Vector3.left);
		Turret.transform.localRotation = Quaternion.AngleAxis(bearing, Vector3.up);

	}
}
