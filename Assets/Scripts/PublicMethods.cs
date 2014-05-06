using UnityEngine;
using System.Collections;

public class PublicMethods : MonoBehaviour {

	/// <summary>
	/// Increments the current speed towards the target speed based on the acceleration and direction.
	/// </summary>
	/// <returns>
	/// Current speed IF current speed matches the target speed.
	/// Target speed IF current speed exceeds the target speed.
	/// </returns>
	/// <param name="current">Current speed</param>
	/// <param name="target">Target speed</param>
	/// <param name="accel">Acceleration</param>
	public static float IncrementTowards(float current, float target, float accel) {
		if (current == target) {
			return current;
		} else {
			float dir = Mathf.Sign(target-current); // left or right
			current += accel*Time.deltaTime*dir;
			return (dir == Mathf.Sign(target-current)) ? current : target; // if current passed target, return target
		}
	}
}
