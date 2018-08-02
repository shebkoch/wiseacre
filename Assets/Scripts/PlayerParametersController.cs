using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParametersController : Singleton<PlayerParametersController> {
	
	[SerializeField]
	private float hp;
	[SerializeField] 
	private float maxHp;
	public GameObject damagePanel;
	public float Health
	{
		get { return hp; }
		set {
				if (value < hp) {
					damagePanel.SetActive(true);
					Invoke("DisableDamagePanel", 1.5f);
				}
				hp = value;
				if (hp > maxHp) hp = maxHp;
				UIController.Instance.SetHpImage(hp/maxHp);
				if (hp <= 0) UIController.Instance.GameOver();
			}
		}

	private float mana;
	[SerializeField]
	private float startMana;
	[SerializeField]
	private float maxMana;
	[SerializeField]
	private float manaRegen;
	public float Mana
	{
		get { return mana; }
		set {
			if (mana < value) UIController.Instance.SetAddManaImage(value-mana);
			mana = value;
			if (mana <= 0)
				mana = 0;
			UIController.Instance.SetManaImage(mana/maxMana);
		}
	}
	private void Start() {
		Mana = startMana;
	}

	private void DisableDamagePanel() {
		damagePanel.SetActive(false);
	}

	private void Update() {
		mana += manaRegen*Time.deltaTime;
	}
}
