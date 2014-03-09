using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class TouchController : MonoBehaviour {

	private bool _touchDown;
	private Touchable _activeObject;

	// Use this for initialization
	void Start () {
		_touchDown = false;
	}
	
	// Update is called once per frame
	void Update () {
		var touches = Input.touches;

		//if there are no touches, clear the touchdown flag
		if(touches.Length == 0)
		{
			if(_activeObject != null)
				_activeObject.EndTouch();
			_touchDown = false;
			_activeObject = null;
		}

		//if we haven't touched and there is one touch, find touchable object under finger
		if(touches.Length == 1 && _touchDown == false)
		{
			Debug.Log("touched!");

			Ray screenRay = camera.ScreenPointToRay(touches[0].position);
			RaycastHit hitInfo;
			if(Physics.Raycast(screenRay, out hitInfo, camera.farClipPlane))
			{
				Touchable obj = hitInfo.collider.gameObject.GetComponent<Touchable>();
				if(obj != null)
				{
					_activeObject = obj;
					obj.BeginTouch(touches[0].position);
				}
			}

			_touchDown = true;
		}

		//if we are touching, go update the active object
		if(_touchDown)
		{
			if(_activeObject != null)
				_activeObject.Touching(touches[0].position);
		}
	}
}
