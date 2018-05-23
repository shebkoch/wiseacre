using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TipsGroup
{
	public List<Tip> tips;
	public Tip GetNearTip(Tip search) {
		float minDist = 100;
		Tip result = new Tip(0);
		foreach (var tip in tips) {
			if (Mathf.Abs(tip.Mark - search.Mark) < minDist) {
				result = tip;
				minDist = Mathf.Abs(tip.Mark - search.Mark);
			}
		}
		return result;
	}
}
[System.Serializable]
public class Tip
{
	public GameObject tip;
	[SerializeField]
	private float mark;
	public float Mark
	{
		get { return mark; }
		set {
			if (value > 1 && value <= 100) mark = value / 100;
			else if (value >= -1 && value <= 1) mark = value;
			else throw new System.ArgumentException("unexpected mark value");
		}
	}
	public Tip(GameObject tip, float _mark) {
		this.tip = tip;
		Mark = _mark;
	}
	public Tip(float _mark) {
		tip = null;
		Mark = _mark;
	}
}
