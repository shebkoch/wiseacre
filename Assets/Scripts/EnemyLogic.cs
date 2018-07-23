using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {
	public bool isAlive = true;
	public int chanceToGainMana;
	public int manaGainCount;
	public GameObject shell;
	public float shellСooldown;
	private float shellTime;
	public float shellForce;
	public float scatter;
	
	public void Awake() {
		shellTime = Time.time;
	}
	public bool IsAlive() {
		return isAlive;
		
	}

	public void ForceKill() {
		isAlive = false;
		var animator = GetComponent<Animator>();
		if (animator)
			GetComponent<Animator>().SetTrigger("Die");
		GetMana();
	}

	private void GetMana() {
		if (!isAlive) return;
		var rand = Random.Range(0, 100);
		if (rand < chanceToGainMana) {
			PlayerParametersController.Instance.Mana += manaGainCount;
			var manaGem = Instantiate(Resources.Load<GameObject>("mana"), transform.position, Quaternion.identity);
			Destroy(manaGem, 2f);
		}
	}

	bool IsReady() {
		if (Time.time - shellTime > shellСooldown) {
			shellTime = Time.time + Random.Range(-1,1) * scatter;
			return true;
		}
		return false;
	}
	void Launch() {
		var player = GameObject.FindGameObjectWithTag("Player");
		var shellCopy = Instantiate(shell, transform.position, Quaternion.identity, transform);
		//shellCopy.transform.LookAt(player.transform);
		shellCopy.transform.right = player.transform.position - shellCopy.transform.position;
		var direction = player.transform.position - shellCopy.transform.position;
		direction.Normalize();
		//shellCopy.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector3.forward * 10);
		shellCopy.GetComponent<Rigidbody2D>().AddForce(direction * shellForce, ForceMode2D.Force);
		//shellCopy.GetComponent<Rigidbody2D>().AddForce( new Vector3(shellCopy.transform.forward.x, 0, shellCopy.transform.forward.z) * shellForce);
	}

	public void OnTriggerEnter2D(Collider2D collider) {
		
		if (collider.tag == "goodFire") {
			var animator = GetComponent<Animator>();
			if (animator) GetComponent<Animator>().SetTrigger(isAlive ? "FireDie" : "Die");
			isAlive = false;
			GetMana();
		}

		if (collider.tag == "tornado") {
			isAlive = false;
			GetMana();//rf
			gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
		}
	}
	void Update() {
		//TODO
		if (shell) {
			if (isAlive && IsReady()) Launch();
		}
	}
}
