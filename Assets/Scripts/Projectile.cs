using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float Angle;
	public Vector3 FiringPosition;
	public float Bearing;
	public float Velocity;

	public float Gravity;

	// Use this for initialization
	void Start () {
	
		this.transform.rotation = Quaternion.identity;
		this.transform.Rotate(Vector3.up, Bearing);
		this.transform.Rotate(Vector3.right, -Angle);
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.Translate(Vector3.forward * Velocity * Time.deltaTime);
		Gravity += 9.81f * Time.deltaTime;
		this.transform.Translate(Vector3.down * Gravity * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider collider)
	{
		Destroy (this);
	}


}
