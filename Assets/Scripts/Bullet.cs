using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class Bullet : MonoBehaviour {
	public GameObject bulletPrefab;
	public float force;
	public float cooldown;

	private float lastAttackTime;

	private void Update() {
		var touchPos = PlayerInput.GetCastTouchScreenPos();
		if(touchPos.HasValue) 
			Attack();
	}

	private void Attack() {
		if (Time.time - lastAttackTime < cooldown) return;
		lastAttackTime = Time.time;
		var attackDirection = PlayerInput.AttackDirection;
		if(attackDirection == Vector3.zero) return;
		var playerPos = FindObjectOfType<Player>().transform.position;
		var fireball = Instantiate(bulletPrefab, playerPos, Quaternion.identity);
		var targetPos = playerPos + attackDirection;
		fireball.transform.right = targetPos - fireball.transform.position; //rf
		var direction = targetPos - fireball.transform.position;
		direction.Normalize();
		fireball.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Force);
	}
	
	
}
