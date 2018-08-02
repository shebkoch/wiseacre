using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour {
	public GameObject castField;
	public GameObject controls;

	public bool isActive;
	void Enable() {
		castField.SetActive(true);
		controls.SetActive(false);
	}
	void Unable() {
		castField.SetActive(false);
		controls.SetActive(true);
	}

	public void Toggle() {
		isActive = !isActive;
		if (isActive)
			Enable();
		else
			Unable();
	}

}
