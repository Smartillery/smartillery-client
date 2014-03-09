using UnityEngine;
using System.Collections;
using System.Net;

using SimpleJSON;

public class SimpleJsonTest : MonoBehaviour {

	public TextMesh mesh;

	// Use this for initialization
	void Start () {
	
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
