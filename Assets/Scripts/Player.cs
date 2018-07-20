using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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
	public Direction direction;
	[Space(10)]
	public GameObject shield;
	public int shieldManaCost;
	public float invincibilityTime;
	private float lastDamageTime;
	[Space(10)]
	public float catchClickTime;
	public float speed;
	private float lastClickTime = 0;
	private bool isDoubleClicked = false;
	private Animator animator;
	private Spell spell;
	
	
	private void OnTriggerEnter2D(Collider2D collision) {
		if(collision.tag == "Shell") {
			Destroy(collision.gameObject);
			if (Time.time - lastDamageTime > invincibilityTime) {
				lastDamageTime = Time.time;
				PlayerParametersController.Instance.Mana -= shieldManaCost;
				shield.GetComponent<Animator>().SetTrigger("shield");

			}
		}
	}
	void Awake() {
		animator = GetComponent<Animator>();
		spell = GetComponent<Spell>();

		//GetComponent<Rigidbody2D>().velocity = new Vector3(1, 0, dir.z) * speed;
	}
	private void SetDirection(float x, float y) {
		if (Mathf.Abs(x) > Mathf.Abs(y))
			direction = x < 0 ? Direction.Left : Direction.Right;
		else
			direction = y < 0 ? Direction.Down : Direction.Up;
		
	}
	private void Movement()
	{
		var directionVector = PlayerInput.GetTargetDirection();
		SetDirection(directionVector.x, directionVector.y);
		transform.Translate(speed * Time.deltaTime * PlayerInput.GetTargetDirection()); //??
	}

	void Update() {
		
		PlayerAnimation.SetDirectionAnimation(animator, direction);
		PlayerAnimation.SetAnimationFlags(animator, isDoubleClicked,!isDoubleClicked);
	}

	void FixedUpdate()
	{
		Movement();
	}
}
