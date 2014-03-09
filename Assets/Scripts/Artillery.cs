using UnityEngine;
using System.Collections;

public class Artillery : MonoBehaviour {

	public float bearing;
	public float elevation;

	public GameObject Turret;
	public GameObject Barrel;
	public GameObject MuzzleFlashPrefab;
	public GameObject EndOfBarrel;
	public AudioClip ArtillerySound;
	public Camera ArtilleryCamera;

	public float RecoilSpeed;
	public float RecoilAmount;

	private bool _firing;
	private Vector3 BarrelLocalPosition;
	private Vector3 TurretLocalPosition;
	// Use this for initialization
	void Start () {
		bearing = 0;
		elevation = 0;
		_firing = false;
		BarrelLocalPosition = Barrel.transform.localPosition;
		TurretLocalPosition = Turret.transform.localPosition;
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
			Turret.transform.localPosition = Vector3.MoveTowards(Turret.transform.localPosition, TurretLocalPosition, RecoilSpeed * Time.deltaTime);
		}
	}

	public void Fire()
	{
		if(_firing)
			return;

		_firing = true;
		Barrel.transform.Translate(Vector3.back * RecoilAmount);
		Turret.transform.Translate(Vector3.back * (RecoilAmount * 0.25f));
		Instantiate (MuzzleFlashPrefab, EndOfBarrel.transform.position, Quaternion.identity);
		AudioSource.PlayClipAtPoint (ArtillerySound, transform.position);
		Handheld.Vibrate();
	}
}
