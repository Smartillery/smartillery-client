using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SmartilleryUnityWrapper;
using System.Linq;
using System.Threading;

public class SmartilleryApiSim : MonoBehaviour, ISmartilleryApi {

	public bool ServerConnected;
	public string ValidUsername;
	public string ValidPassword;

	public float PlayerVelocity;
	public float LaunchVelocity;

	public int LoginTimeMs;
	public int LagTimeMs;

	public Vector2 LandLocationOffset;

	public Location[] EnemyLocations;

	private Location lastPlayerLocation; 
	private DateTime lastPlayerLocationTime;
	private Location playerCurrentLocation;

	private List<Player> _enemies;
	private List<Launch> _launches;

	// Use this for initialization
	void Start () {
		
		_enemies = new List<Player>();
		for(int i = 0; i < EnemyLocations.Length; i++)
		{
			_enemies.Add(new Player(){CurrentLocation = EnemyLocations[i], Destination = EnemyLocations[i], Velocity = PlayerVelocity});
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region ISmartilleryApi implementation

	public void Login (string username, string password)
	{
		AssertServerConnected();

		Thread.Sleep(LoginTimeMs);

		if(username != ValidUsername || password != ValidPassword)
			throw new UnAuthorizedException();

		Debug.Log("Logged in " + username);
	}

	public void UpdateLocation (Location location)
	{
		AssertServerConnected();
	
		lastPlayerLocation = location;
		lastPlayerLocationTime = DateTime.Now;

		Debug.Log("Player location updated " + location);
	}
		
	public void Launch (double angle, double bearing)
	{
		AssertServerConnected();

		UpdatePlayerLocation();

		Launch launch = new Launch(){
			Angle = angle,
			Bearing = bearing,
			TimeFired = DateTime.Now,
			FiringLocation = playerCurrentLocation,
			FiringVelocity = LaunchVelocity,
		};

		CalculateLanding(launch);

		_launches.Add(launch);
	}

	public Player GetCurrentPlayer ()
	{		
		AssertServerConnected();

		UpdatePlayerLocation();

		return new Player(){ CurrentLocation = playerCurrentLocation, Destination = lastPlayerLocation, Velocity = PlayerVelocity};
	}

	public ICollection<Player> GetEnemyPlayers ()
	{
		AssertServerConnected();

		return _enemies;
	}

	public ICollection<Launch> GetLaunches ()
	{
		AssertServerConnected();

		return _launches;
	}

	public ICollection<Launch> GetLaunches (DateTime since)
	{
		AssertServerConnected();

		return _launches.Where(x => x.TimeFired > since).Select(x => {x.CurrentTime = DateTime.Now; return x;}).ToList();
	}

	private void CalculateLanding(Launch launch)
	{
		double angleInRad = launch.Angle;
		double flightTimeSeconds = (Math.Cos(angleInRad * launch.Velocity) / 9.81f) * 2;
		var flightTime = TimeSpan.FromSeconds(flightTimeSeconds);

		launch.TimeLand = launch.TimeFired + flightTime;

		double distKm = launch.Velocity / 1000.0f;
		launch.ActualLanding = CalcDestination(launch.FiringLocation, launch.Bearing, distKm);
	}

	/*

 * Returns the destination point from this point having travelled the given distance
 * (in km) on the given initial bearing (bearing may vary before destination is reached)
 *
 *   see http://williams.best.vwh.net/avform.htm#LL
 *
 * @param   {Number} brng: Initial bearing in degrees
 * @param   {Number} dist: Distance in km
 * @returns {LatLon} Destination point
 *

LatLon.prototype.destinationPoint = function(brng, dist) 
	{
		dist = typeof(dist)=='number' ? dist : typeof(dist)=='string' && dist.trim()!='' ? +dist : NaN;
		dist = dist/this._radius;  // convert dist to angular distance in radians
		brng = brng.toRad();  // 
		var lat1 = this._lat.toRad(), lon1 = this._lon.toRad();
		
		var lat2 = Math.asin( Math.sin(lat1)*Math.cos(dist) + 
		                     Math.cos(lat1)*Math.sin(dist)*Math.cos(brng) );
		var lon2 = lon1 + Math.atan2(Math.sin(brng)*Math.sin(dist)*Math.cos(lat1), 
		                             Math.cos(dist)-Math.sin(lat1)*Math.sin(lat2));
		lon2 = (lon2+3*Math.PI) % (2*Math.PI) - Math.PI;  // normalise to -180..+180º
		
		return new LatLon(lat2.toDeg(), lon2.toDeg());
	}
	 */

	private Location CalcDestination(Location start, double bearingDeg, double distanceKm)
	{
		double radius = 6378.1;  //radius of the earth in KM
		double distRad = distanceKm/radius;
		double bearingRad = DegToRad(bearingDeg);
		Location startRad = new Location(){Latitude = DegToRad(start.Latitude), Longitude = DegToRad(start.Longitude)};
		Location destRad = new Location();
		destRad.Latitude = Math.Asin(Math.Sin(startRad.Latitude) * Math.Cos(distRad) + Math.Cos(startRad.Latitude)*Math.Sin(distRad)*Math.Cos(bearingRad));
		destRad.Longitude = startRad.Longitude + Math.Atan2(Math.Sin(bearingRad)*Math.Sin(distRad)*Math.Cos(startRad.Latitude), Math.Cos(distRad)-Math.Sin(startRad.Latitude)*Math.Sin(destRad.Latitude));
		destRad.Longitude = (destRad.Longitude+3*Math.PI) % (2*Math.PI) - Math.PI;  //normalize to -180..+180
		return new Location() {Latitude = RadToDeg(destRad.Latitude), Longitude = RadToDeg(destRad.Longitude)};
	}

	private double DegToRad(double deg)
	{
		return deg * Math.PI / 180.0;
	}

	private double RadToDeg(double rad)
	{
		return 180.0 * rad / Math.PI;
	}

	private void UpdatePlayerLocation()
	{
		TimeSpan time = DateTime.Now - lastPlayerLocationTime;

		float positionDelta = (float)time.TotalSeconds * PlayerVelocity;

		Vector2 start = new Vector2((float)playerCurrentLocation.Latitude, (float)playerCurrentLocation.Longitude);
		Vector2 dest = new Vector2((float)lastPlayerLocation.Latitude, (float)lastPlayerLocation.Longitude);

		Vector2 newLoc =  Vector2.MoveTowards(start, dest, positionDelta);

		playerCurrentLocation = new Location{Latitude = newLoc.x, Longitude = newLoc.y};
	}

	private void AssertServerConnected()
	{
		if(!ServerConnected)
		{
			throw new ConnectionTimeoutException();
		}

		Thread.Sleep(LagTimeMs);
	}

	#endregion
}
