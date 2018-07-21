using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
<<<<<<< HEAD

=======
>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
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
	public Vector2 moveVec;
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
<<<<<<< HEAD
		if (Mathf.Abs(x) > Mathf.Abs(y))
			direction = x < 0 ? Direction.Left : Direction.Right;
		else
			direction = y < 0 ? Direction.Down : Direction.Up;
		
=======
		if (Mathf.Abs(x) > Mathf.Abs(y)) {
			if (x < 0) direction = Direction.Left;
			else direction = Direction.Right;
		} else {
			if (y < 0) direction = Direction.Down;
			else direction = Direction.Up;
		}
	}
	private void Movement() {
		//var touchPos = GetMouseWorldPos();
		//Vector3 directionVector = touchPos - transform.position;
		//transform.position = Vector3.MoveTowards(transform.position, touchPos, speed * Time.deltaTime);
		SetDirection(moveVec.x, moveVec.y);
		var pos = transform.position + new Vector3(moveVec.x, moveVec.y,transform.position.z);
		transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
	}
	private void CatchSpellStart() {
		//if (Time.time - lastClickTime < catchClickTime) {
		//	isDoubleClicked = true;
		//	spell.SetActiveDraw(true);
		//} else isDoubleClicked = false;
		//lastClickTime = Time.time;
		if (Joystick.isPress == false) {
			isDoubleClicked = true;
			spell.SetActiveDraw(true);
		} else isDoubleClicked = false;
>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
	}
	private void Movement()
	{
		var directionVector = PlayerInput.GetTargetDirection();
		SetDirection(directionVector.x, directionVector.y);
		transform.Translate(speed * Time.deltaTime * PlayerInput.GetTargetDirection()); //??
	}
<<<<<<< HEAD

	void Update() {
		
		PlayerAnimation.SetDirectionAnimation(animator, direction);
		PlayerAnimation.SetAnimationFlags(animator, isDoubleClicked,!isDoubleClicked);
=======
	void Update() {

		if (Input.GetMouseButtonDown(0)) {
			CatchSpellStart();
		}

		if (Input.GetMouseButtonUp(0)) {
			isDoubleClicked = false;
		}
		moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
										CrossPlatformInputManager.GetAxis("Vertical"));
		PlayerAnimation.SetDirectionAnimation(animator, direction);
		PlayerAnimation.SetAnimationFlags(animator, isDoubleClicked, !isDoubleClicked);		//moveVec == Vector2.zero);

>>>>>>> ee8372107ccc4202b8a4c10e0919c8af43ee5e7c
	}

	void FixedUpdate()
	{
		Movement();
	}
}
