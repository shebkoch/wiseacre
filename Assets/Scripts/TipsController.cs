using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Teller
{
	public Dictionary<string, float> tips;
	//private string fileName;
	public GameObject gameObject;
	public string currentPhrase;
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
		catch (System.Exception e) {
			Debug.Log(e.Message);
			return false;
		}
		return true;
	}
	public Teller(string filename, GameObject gameObject) {
		this.gameObject = gameObject;
		using (StreamReader sr = new StreamReader(filename)) {
			TryParseTips(sr.ReadToEnd());	//todo
		}
	}
	private string TextBound(string text, int boundSize) {
		string result = "";
		var splitText = text.Split(' ');
		string buff = "";
		foreach (var item in splitText) {
			if (buff.Length + item.Length < boundSize) {
				buff += item;
			} else {
				result += System.Environment.NewLine;
				buff = item;
			}
			result += item + " "; 
		}
		return result;
	}
	public void SetAppropriate(float mark) {
		var min = 100f;
		string result = "";
		foreach (var tip in tips) {
			var buff = Mathf.Abs(tip.Value - mark);
			if (buff < min) {
				min = buff;
				result = tip.Key;
			}
		}
		currentPhrase = result;
	}
	public void Place(Vector2 position, int textBounds) {
		gameObject.transform.position = position;
		var textMesh = gameObject.GetComponentInChildren<TextMesh>();	
		textMesh.text = TextBound(currentPhrase, textBounds);
	}
}
public class TipsController : Singleton<TipsController>
{
	public GameObject crazyObject;
	public GameObject kindlyObject;
	public GameObject angryObject;
	public List<Teller> tellers;

	public int textBound;
	private string angryFileName = "angry.json";
	private string kindlyFileName = "kindly.json";
	private string fileName = "crazy.json";


	//delete
	public List<TipsGroup> tipsGroups;
	public List<GameObject> onScene = new List<GameObject>();
	
	public void SetTips(List<Vector2> possibleDirection) {
		Teller crazy = new Teller("crazy.json", crazyObject);
		Teller kindly = new Teller("kindly.json", kindlyObject);
		Teller angry = new Teller("angry.json", angryObject);

		List<Teller> tellers = new List<Teller>() { crazy,kindly,angry};
		var length = possibleDirection.Count;
		for (int i = 0; i < length; i++) {
			int randDirection = Random.Range(0, possibleDirection.Count);
			//TODO:
			var mark = Random.Range(-1f,1f);
			tellers[i].SetAppropriate(mark);
			tellers[i].Place(possibleDirection[randDirection], textBound);
			possibleDirection.RemoveAt(randDirection);
		}
	}
	//TODO
	public void Clear() {
		crazyObject.transform.position = new Vector3(-1000, -1000, -1000);
		angryObject.transform.position = new Vector3(-1000, -1000, -1000);
		kindlyObject.transform.position = new Vector3(-1000, -1000, -1000);
	}
}
