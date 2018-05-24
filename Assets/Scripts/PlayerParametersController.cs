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
}
