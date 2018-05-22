using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private enum Direction { Left, Right, Down, Up }
	private Direction direction;
	public float catchClickTime;
	private float lastClickTime = 0;
	private bool isDoubleClicked = false;

	private Spell spell;
	
	void Awake() {
		spell = GetComponent<Spell>();
	}
	void SetDirection(float x, float y) {
		if (Mathf.Abs(x) > Mathf.Abs(y)) {
			if (x < 0) direction = Direction.Left;
			else direction = Direction.Right;
		} else {
			if (y < 0) direction = Direction.Down;
			else direction = Direction.Up;
		}
	}
	void Movement() {
		var touchPos = GetMouseWorldPos();
		Vector3 directionVector = touchPos - transform.position;
		SetDirection(directionVector.x, directionVector.y);
		SetSprite();
		transform.position = Vector3.Lerp(transform.position, touchPos, Time.deltaTime);
	}
	void CatchDoubleClick() {
		if (Time.time - lastClickTime < catchClickTime) {
			isDoubleClicked = true;
			spell.SetActiveDraw(true);
		} else isDoubleClicked = false;
		lastClickTime = Time.time;
	}
	Vector3 GetMouseWorldPos() {
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		return mousePos;
	}
	void SetSprite() {
		//TODO: 
	}
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			CatchDoubleClick();
		}

		if (Input.GetMouseButtonUp(0)) {
			isDoubleClicked = false;
		}
	}
	void FixedUpdate () {
		if(Input.GetMouseButton(0)) {
			if (!isDoubleClicked)
				Movement();
		}
	}
}
