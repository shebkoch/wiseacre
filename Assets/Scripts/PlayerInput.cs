using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInput
{
	public static Vector3 GetTargetDirection()
	{
		return new Vector3(
			CrossPlatformInputManager.GetAxis("Horizontal"),
			CrossPlatformInputManager.GetAxis("Vertical"),
			0);
	}

	public static Vector3? GetMovementTouchPos()
	{
		return GetTouchPos(true);
	}

	public static Vector3? GetCastTouchPos()
	{
		return GetTouchPos(false);
	}

	private static Vector3? GetTouchPos(bool isleft)
	{
		#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{	
			var mousePos = Input.mousePosition;
			var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
			mouseWorldPos.z = 0;
			if (isleft)
			{
				if (mousePos.x < Screen.width / 2)
					return mouseWorldPos;
			}
			else if (mousePos.x > Screen.width / 2)
					return mouseWorldPos;
		}
		#endif 
		Touch? touch = null;
		foreach (var inputTouch in Input.touches) {
			if (isleft)
			{
				if (inputTouch.position.x < Screen.width / 2) touch = inputTouch;
			}
			else
				if (inputTouch.position.x > Screen.width / 2) touch = inputTouch;
		}
		if (!touch.HasValue) return null;
		var worldPos = Camera.main.ScreenToWorldPoint(touch.Value.position);
		worldPos.z = 0;
		return worldPos;
	}

}
