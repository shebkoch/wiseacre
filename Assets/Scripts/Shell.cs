using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "static") { Destroy(gameObject);}
	}
}
