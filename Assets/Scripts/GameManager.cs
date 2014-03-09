using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using SmartilleryUnityWrapper;
using System.Collections.Generic;

[RequireComponent(typeof(SmartilleryApiSim))]
public class GameManager : MonoBehaviour {

	public SmartilleryApiSim api;
	public float UpdateInterval;

	public float _timeSinceUpdate;
	private LocationInfo _lastLoc;
	private ActionQueue _actions;

	private ICollection<Launch> _launches;

	
	// Use this for initialization
	void Start () {
		_launches = new List<Launch>();
		_actions = new ActionQueue();
		var apis = GameObject.FindObjectsOfType<SmartilleryApiSim>();
		if(apis.Length > 1)
		{
			GetComponent<SmartilleryApiSim>().enabled = false;
			api = GameObject.FindObjectOfType<SmartilleryApiSim>();
		}
		else
		{
			api = GetComponent<SmartilleryApiSim>();
		}

		_timeSinceUpdate = UpdateInterval;

		if(Input.location.status != LocationServiceStatus.Running)
		{
			Input.location.Start();
		}
	}
	
	// Update is called once per frame
	void Update () {
		_timeSinceUpdate += Time.deltaTime;

		if(_timeSinceUpdate >= UpdateInterval && _actions.Empty)
		{
			LocationInfo locInfo = Input.location.lastData;

			_actions.AddAction(() => UpdateLocation(locInfo));
			_actions.AddAction(UpdateProjectiles);
			_actions.AddAction(UpdatePlayers);
			_actions.AddAction(() => _timeSinceUpdate = 0);
		}

		if(_actions.HasException)
		{
			try
			{
				_actions.GetException();
			}
			finally
			{
				_actions.ClearException();
			}
		}

	}
	
	public void FireProjectile(Projectile projectile)
	{
		_actions.AddAction(() => api.Launch(projectile.Angle, projectile.Bearing), true);
	}

	void UpdateLocation(LocationInfo locInfo)
	{
		//distance = LatLong.GetDistance(locInfo, _lastLoc);
		//TODO: only update if we have moved by a resonable amount
		Location location = new Location() {Latitude = locInfo.latitude, Longitude = locInfo.longitude};
		api.UpdateLocation(location);

		_lastLoc = locInfo;
	}

	void UpdateProjectiles()
	{
		Debug.Log ("Updating projectiles");
		if(_launches.Count == 0)
		{
			_launches = _launches.Concat(api.GetLaunches()).ToList();
		}
		else
		{
			DateTime since = _launches.Max(x => x.TimeFired);
			_launches = _launches.Concat(api.GetLaunches (since)).ToList();
		}

		foreach(Launch launch in _launches)
		{
			Debug.Log (launch);
		}

	}

	void UpdatePlayers()
	{
	}
}
