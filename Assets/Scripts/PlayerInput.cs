using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;

public static class PlayerInput {
	private static Direction playerDirection;
	public static Direction PlayerDirection {
		get {
			SetDirection();
			return playerDirection;
		}	
	}

	public static void SetDirection() {
		if (TargetDirection != Vector3.zero)
			playerDirection = GetDirection(TargetDirection.x, TargetDirection.y);
	}
	public static Vector3 TargetDirection {
		get {
			return new Vector3(
			CrossPlatformInputManager.GetAxis("Horizontal"),
			CrossPlatformInputManager.GetAxis("Vertical"),
			0);
		}
	}

	public static Vector3 AttackDirection {
		get { return new Vector3(
			CrossPlatformInputManager.GetAxis("HorizontalAttack"),
			CrossPlatformInputManager.GetAxis("VerticalAttack"),
			0); }
	}

	public static Vector3? GetMovementTouchPos()
	{
		return GetTouchPos(true);
	}

	public static Vector3? GetMovementTouchScreenPos() {
		return GetTouchPos(true, true);
	}

	
	public static Vector3? GetCastTouchPos()
	{
		return GetTouchPos(false);
	}

	public static Vector3? GetCastTouchScreenPos() {
		return GetTouchPos(false, true);
	}
	

	private static Vector3? GetTouchPos(bool isleft, bool isScreen = false)
	{
		#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{	
			var mousePos = Input.mousePosition;
			var mouseWorldPos = isScreen ? mousePos : Camera.main.ScreenToWorldPoint(mousePos);
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

		Vector3 touchPos = touch.Value.position;
		var touchWorldPos = isScreen ? touchPos : Camera.main.ScreenToWorldPoint(touchPos);
		touchPos.z = 0;
		return touchWorldPos;
	}
	private static Direction GetDirection(float x, float y) {
		if (Mathf.Abs(x) > Mathf.Abs(y))
			return x < 0 ? Direction.Left : Direction.Right;
		else
			return y < 0 ? Direction.Down : Direction.Up;
	}

}
