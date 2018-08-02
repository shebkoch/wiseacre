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
	[Space(10)]
	public List<Image> hearts;
	public Image health;
	[Space(10)]
	public GameObject endMenu;
	[Space(10)]
	public Text manaText;

	public Image manaImage;
	public Text addMana;
	
	public void SetHpImage(float hp) {
		health.fillAmount = hp;
	}
	public void SetManaImage(float mana) {
		manaImage.fillAmount = mana;
	}
	public void SetAddManaImage(float delta) {
		addMana.text = "+" + delta;
		Invoke("ClearAddMana", 1f);
	}
	private void ClearAddMana() {
		addMana.text = "";
	}
	public void StartFade() {
		fade.color = fadeColor;
		isFading = true;
	}
	public void GameOver() {
		endMenu.SetActive(true);
	}
	public void Restart() {
		Application.LoadLevel(0); //todo
	}
	public void Quit() {
		NeuralAdapter.Save();
		Application.Quit();
	}
	public void Update() {
		if (isFading) {
			fade.color -= new Color(0, 0, 0, fadeSpeed);
			if (fade.color.a <= 0) isFading = false;
		}
	}
}
