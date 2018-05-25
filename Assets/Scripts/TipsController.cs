using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Teller
{
	public Dictionary<string, float> tips;
	//private string fileName;
	public GameObject gameObject;

	bool TryParseTips(string s) {    //TODO
		tips = new Dictionary<string, float>();
		try 
		{
			s = s.Replace("{", "");
			s = s.Replace("}", "");
			s = s.Replace("\"", "");
			s = s.Replace("\t", "");
			s = s.Replace(",\r\n", ";");
			string[] arr = s.Split(';');
			foreach (var item in arr) {
				var elem = item.Split(':');
				tips.Add(elem[0], float.Parse(elem[1]));
			}
		}
		catch (System.Exception) {
			return false;
		}
		return true;
	}
	public Teller(string filename) { 
		using (StreamReader sr = new StreamReader(filename)) {
			TryParseTips(sr.ReadToEnd());
		}
	}
	public string GetAppropriate(float mark) {
		var min = 100f;
		string result = "";
		foreach (var tip in tips) {
			var buff = Mathf.Abs(tip.Value - mark);
			if (buff < mark) {
				mark = buff;
				result = tip.Key;
			}
		}
		return result;
	}
}
public class TipsController : Singleton<TipsController>
{
	public List<TipsGroup> tipsGroups;
	public List<GameObject> onScene = new List<GameObject>();
	public Dictionary<string, int> angryTips;
	public Dictionary<string, int> kindlyTips;
	public Dictionary<string, int> crazyTips;
	private string angryFileName = "angry.json";
	private string kindlyFileName = "kindly.json";
	private string FileName = "crazy.json";
	public void TESTING() {
		var t = new Teller("crazy.json");
		
	}
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
