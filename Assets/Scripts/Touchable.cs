using UnityEngine;
using System.Collections;


public abstract class Touchable : MonoBehaviour {

	public abstract void BeginTouch(Vector2 position);

	public abstract void Touching(Vector2 position);

	public abstract void EndTouch();

	public abstract void DoubleTouch(Vector2 touch1, Vector2 touch2);
}
