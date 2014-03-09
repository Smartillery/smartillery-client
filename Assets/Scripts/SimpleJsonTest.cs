using UnityEngine;
using System.Collections;
using System.Net;
using System;

using SimpleJSON;

public class SimpleJsonTest : MonoBehaviour {

	public TextMesh mesh;

	// Use this for initialization
	void Start () {
		try{
		WebClient client = new WebClient();
		var data = client.DownloadString("http://ip.jsontest.com");

		var node = JSONNode.Parse(data);

		if(mesh != null)
		{
			mesh.text = node["ip"];
		}
		else
		{
			Debug.Log(node["ip"]);
			}
		}
		catch(Exception ex){
			mesh.text = ex.Message;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
