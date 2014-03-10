using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float Angle;
	public Vector3 FiringPosition;
	public float Bearing;
	public float Velocity;

	public GameObject ShellModel;
	public GameObject CraterModel;
	public GameObject Explosion;

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
		Gravity += 0.1f * 9.81f * Time.deltaTime;
		this.transform.Translate(Vector3.down * Gravity * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider collider)
	{
		ShellModel.SetActive(false);
		CraterModel.SetActive(true);
		Explosion.SetActive(true);
		Vector3 pos = this.transform.position;
		pos.y = 0;
		this.transform.position = pos;
		this.transform.rotation = Quaternion.identity;
		this.transform.Rotate(Vector3.up * Random.Range(0f, 360f));
		Destroy (this);
	}


}
