using UnityEngine;
using System.Collections;

public class RotationHelper {

	static public float GetSignedRotation(Quaternion rotationA, Quaternion rotationB, bool yZplane)
	{
		// get a "forward vector" for each rotation
		Vector3 forwardA = rotationA * Vector3.forward;
		Vector3 forwardB = rotationB * Vector3.forward;

		float aPt = yZplane ? forwardA.y : forwardA.x;
		float bPt = yZplane ? forwardB.y : forwardB.x;


		// get a numeric angle for each vector, on the X-Z plane (relative to world forward)
		var angleA = Mathf.Atan2(aPt, forwardA.z) * Mathf.Rad2Deg;
		var angleB = Mathf.Atan2(bPt, forwardB.z) * Mathf.Rad2Deg;
		
		// get the signed difference in these angles
		return Mathf.DeltaAngle( angleA, angleB );
	}
}
