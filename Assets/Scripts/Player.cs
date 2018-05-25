using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private static class PlayerAnimation {
		static Direction oldDirection;
		public static void SetDirectionAnimation(Animator animator, Direction direction) {
			if (direction != oldDirection) {
				animator.SetTrigger("clear");
				animator.ResetTrigger("left");
				animator.ResetTrigger("right");
				animator.ResetTrigger("down");
				animator.ResetTrigger("up");
			}
			switch (direction) {
				case Direction.Left: animator.SetTrigger("left"); break;
				case Direction.Right: animator.SetTrigger("right"); break;
				case Direction.Down: animator.SetTrigger("down"); break;
				case Direction.Up: animator.SetTrigger("up"); break;
			}
			oldDirection = direction;
		}
		public static void SetAnimationFlags(Animator animator, bool isCasting, bool isMoving) {
			animator.SetBool("isCasting", isCasting);
			animator.SetBool("isMoving", isMoving);
		}
	}
	public enum Direction { Left, Right, Down, Up }
	public Direction direction;
	public float catchClickTime;
	public float speed;
	private float lastClickTime = 0;
	private bool isDoubleClicked = false;
	private Animator animator;
	private Spell spell;
	
	void Awake() {
		animator = GetComponent<Animator>();
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
		
		transform.position = Vector3.MoveTowards(transform.position, touchPos, speed * Time.deltaTime);
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
	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			CatchDoubleClick();
		}

		if (Input.GetMouseButtonUp(0)) {
			isDoubleClicked = false;
		}
		PlayerAnimation.SetDirectionAnimation(animator, direction);
		PlayerAnimation.SetAnimationFlags(animator, isDoubleClicked,!isDoubleClicked);

	}
	void FixedUpdate () {
		if(Input.GetMouseButton(0)) {
			if (!isDoubleClicked)
				Movement();
		}
	}
}
