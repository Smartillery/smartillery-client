using UnityEngine;
using System.Collections;

public class Artillery : MonoBehaviour {

	public float bearing;
	public float elevation;

	public GameObject Turret;
	public GameObject Barrel;

	public float RecoilSpeed;
	public float RecoilAmount;

	private bool _firing;
	private Vector3 BarrelLocalPosition;
	// Use this for initialization
	void Start () {
		bearing = 0;
		elevation = 0;
		_firing = false;
		BarrelLocalPosition = Barrel.transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
	
		if(bearing / 360 > 0)
			bearing %= 360;
		elevation = Mathf.Clamp(elevation, 0, 89);

		Barrel.transform.localRotation = Quaternion.AngleAxis(elevation, Vector3.left);
		Turret.transform.localRotation = Quaternion.AngleAxis(bearing, Vector3.up);

		if(_firing)
		{
			if(Vector3.Distance(BarrelLocalPosition, Barrel.transform.localPosition) == 0)
			{
				_firing = false;
			}
			Barrel.transform.localPosition = Vector3.MoveTowards(Barrel.transform.localPosition, BarrelLocalPosition, RecoilSpeed * Time.deltaTime);
		}
	}

	public void Fire()
	{
		if(_firing)
			return;

		_firing = true;
		Barrel.transform.Translate(Vector3.back * RecoilAmount);
	}
}
