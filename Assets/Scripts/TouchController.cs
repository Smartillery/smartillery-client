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
			Touchable obj = GetTouchableObject(touches[0]);
			if(obj != null)
			{
				_activeObject = obj;
				obj.BeginTouch(touches[0].position);
			}

			_touchDown = true;
		}

		if(touches.Length == 2)
		{
			Touchable obj = GetTouchableObject(touches[0]);

			Touchable obj1 = GetTouchableObject(touches[1]);

			if(obj != null && obj == obj1)
				obj.DoubleTouch(touches[0].position, touches[1].position);

		}

		//if we are touching, go update the active object
		if(_touchDown)
		{
			if(_activeObject != null)
				_activeObject.Touching(touches[0].position);
		}
	}

	private Touchable GetTouchableObject(Touch touch)
	{
		Ray screenRay = camera.ScreenPointToRay(touch.position);
		RaycastHit hitInfo;
		if(Physics.Raycast(screenRay, out hitInfo, camera.farClipPlane))
		{
			return hitInfo.collider.gameObject.GetComponent<Touchable>();
		}
		return null;
	}
}
