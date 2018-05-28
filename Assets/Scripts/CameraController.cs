using System.Collections;

using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform Target;
	public Vector3 Offset;
	public float Velocity;
	public float MinDistance;
	private float zPlane;
	void Awake() {
		zPlane = transform.position.z;
	}
	private void Movement() {
		if (!Target) return;
		var targetPos = Target.position + Offset;
		targetPos.z = zPlane;

		if (Vector3.Distance(transform.position, targetPos) < MinDistance) return;

		var newPos = Vector3.Slerp(transform.position, targetPos, Velocity * Time.fixedDeltaTime);
		transform.Translate(transform.InverseTransformPoint(newPos));
	}
	void LateUpdate() {
		Movement();
	}
}