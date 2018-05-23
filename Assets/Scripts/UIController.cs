using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController> {
	[Header("Fading")]
	public Color fadeColor;
	public Image fade;
	public float fadeSpeed;
	private bool isFading;
	//[Space(10)]



	public void StartFade() {
		fade.color = fadeColor;
		isFading = true;
	}
	public void Update() {
		if (isFading) {
			fade.color -= new Color(0, 0, 0, fadeSpeed);
			if (fade.color.a <= 0) isFading = false;
		}
	}
}
