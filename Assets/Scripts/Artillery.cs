using UnityEngine;
using System.Collections;

public class Artillery : MonoBehaviour {

	public double bearing;
	public double elevation;

	public GameObject Turret;
	public GameObject Barrel;

	// Use this for initialization
	void Start () {
		bearing = 0;
		elevation = 45;
	}

	// Update is called once per frame
	void Update () {
	
		Barrel.transform.localRotation = Quaternion.AngleAxis((float)elevation, Vector3.left);
		Turret.transform.localRotation = Quaternion.AngleAxis((float)bearing, Vector3.up);

	}
}
