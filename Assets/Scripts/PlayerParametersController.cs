using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParametersController : Singleton<PlayerParametersController> {
	
	[SerializeField]
	private int hp;
	public int Health
	{
		get { return hp; }
		set {
				hp = value;
				UIController.Instance.SetHpImage(value);
				if (hp <= 0) UIController.Instance.GameOver();
				
			}
		}

	private int mana;
	[SerializeField]
	private int startMana;
	public int Mana
	{
		get { return mana; }
		set {
			if (mana < value) UIController.Instance.SetAddManaImage(value-mana);
			mana = value;
				if (mana <= 0) {
					mana = startMana;
					Health--;
				}
			UIController.Instance.SetManaImage(mana);
			
		}
	}
	private void Start() {
		Mana = startMana;
	}
}
