using UnityEngine;
using System.Collections;

public class RotationHelper {

	static public float GetSignedRotation(Quaternion rotationA, Quaternion rotationB, Vector3 reference)
	{
		// get a "forward vector" for each rotation
		var forwardA = rotationA * reference;
		var forwardB = rotationB * reference;
		
		// get a numeric angle for each vector, on the X-Z plane (relative to world forward)
		var angleA = Mathf.Atan2(forwardA.x, forwardA.z) * Mathf.Rad2Deg;
		var angleB = Mathf.Atan2(forwardB.x, forwardB.z) * Mathf.Rad2Deg;
		
		// get the signed difference in these angles
		return Mathf.DeltaAngle( angleA, angleB );
	}
}
