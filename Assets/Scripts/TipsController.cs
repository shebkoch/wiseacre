using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsController : Singleton<TipsController>
{
	public List<TipsGroup> tipsGroups;
	public List<GameObject> onScene = new List<GameObject>();
	public void SetTips(Vector2 direction) {
		foreach (var item in onScene) {
			Destroy(item);
		}
		var rand = Random.Range(0, tipsGroups.Count);
		TipsGroup currentGroup = tipsGroups[rand];

		var height = LevelGenerator.Instance.height;
		var width = LevelGenerator.Instance.width;
		List<Vector2> positions = new List<Vector2>() {
			new Vector2(height / 2, 0),
			new Vector2(height / 2, width - 1),
			new Vector2(0, width / 2),
			new Vector2(height - 1, width / 2)
		};
		GameObject parent = new GameObject();
		parent.name = "Tips";
		//TODO: neuro
		foreach (var position in positions) {
			Tip mark = new Tip(Random.Range(-1f, 1f));
			GameObject tip = currentGroup.GetNearTip(mark).tip;
			onScene.Add(Instantiate(tip, position, Quaternion.identity, parent.transform));
		}

	}
}
