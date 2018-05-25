using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {
	bool isAlive = true;
	public bool IsAlive() {
		return isAlive; //todo:
	}
	public void ForceKill() {
		isAlive = false;
		GetComponent<Animator>().SetTrigger("Die");
	}

	void Update() {
		//TODO
		if(isAlive)
			transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(Mathf.Abs(GetInstanceID()) % 5 * Random.Range(-1,1), GetInstanceID() % 5 + Random.Range(0,1)), 2 * Time.deltaTime);
	}
}
