using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class Teller
{
	public int id;
	public List<string> tips;
	private List<string> tipsCopy;
	//private string fileName;
	public GameObject gameObject;
	public string currentPhrase;
	bool TryParseTips(string s) {
		tips = new List<string>();
		try 
		{
			s = s.Replace("\r\n", ";");
			string[] arr = s.Split(';');
			tips.Clear();
			tips.AddRange(arr);
		}
		catch (System.Exception e) {
			Debug.Log(e.Message);
			return false;
		}
		return true;
	}
	public Teller(string filename, GameObject gameObject, int id) {
		this.gameObject = gameObject;
		TextAsset phrase = Resources.Load<TextAsset>("Phrases/" + filename);
		if (phrase) {
			//using (StreamReader sr = new StreamReader(new MemoryStream(phrase.bytes))) {
			TryParseTips(phrase.text);   //todo
		} else Debug.Log("phrase " + filename + " failed to load");
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
	string DirectionToString(Direction direction) {
		if (direction == Direction.Left) return "налево";
		if (direction == Direction.Right) return "направо";
		if (direction == Direction.Down) return "наверх";
		if (direction == Direction.Up) return "вниз";
		Debug.Log(direction + "is not direction");
		return "вниз";
	}
	public void SetDirectionPhrase(Direction direction) {
		if (tipsCopy == null || tipsCopy.Count == 0) tipsCopy = tips;
		var rand = Random.Range(0, tipsCopy.Count);
		currentPhrase = tipsCopy[rand] + " " + DirectionToString(direction);
	}
	//public void SetAppropriate(float mark) {
	//	var min = 100f;
	//	string result = "";
	//	foreach (var tip in tips) {
	//		var buff = Mathf.Abs(tip.Value - mark);
	//		if (buff < min) {
	//			min = buff;
	//			result = tip.Key;
	//		}
	//	}
	//	currentPhrase = result;
	//}
	
	public void Place(Vector2 position, int textBounds) {
		gameObject.transform.position = position;
		var textMesh = gameObject.GetComponentInChildren<TextMesh>();	
		textMesh.text = TextBound(currentPhrase, textBounds);
	}
}
public static class NeuralAdapter
{
	static private bool isInitialized = false;
	static private NeuralNetwork net;
	static private float[] input;
	const int inputStateCount = 6;
	const int outputStateCount = 4;
	const int hiddenCount = 10;
	const int vectorToIntFactor = 100;
	static Dictionary<Teller, float[]> inputs = new Dictionary<Teller, float[]>();
	static Dictionary<Teller, float> outputs = new Dictionary<Teller, float>();
	public static Direction GetPhraseDirection(Teller teller,int trapFree, Vector2 curDirection, int curLevel, Direction prev) {
		if (!isInitialized) {
			int[] layers = new int[] { inputStateCount, hiddenCount, hiddenCount, 1 };
			isInitialized = true;
			net = new NeuralNetwork(layers);
		}
		//TODO
		float maxId = 3;
		float maxTraps = 4;
		float maxVector = 100;
		float maxLevel = 15;
		float maxDirection = 4;
		input = new float[inputStateCount] { teller.id / maxId,
											trapFree / maxTraps,
											curDirection.x / maxVector,
											curDirection.y / maxVector,
											curLevel / maxLevel,
											(float)prev / maxDirection };
		if (!inputs.ContainsKey(teller)) inputs.Add(teller,input);
		inputs[teller] = input;
		float[] outputArr = net.GetAnswer(input);
		float output = outputArr[0];
		if (!outputs.ContainsKey(teller)) outputs.Add(teller, output);
		outputs[teller] = output;
		return FloatToDirection(output);
	}
	static Direction FloatToDirection(float f) {
		return (Direction)((f + 1) * 1.5);
	}
	const int damagedFactor = 3;
	const int unDamagedFactor = 1;
	public static void Study(bool isDamaged) {
		foreach (var item in inputs) {
			if (isDamaged) net.Study(input, damagedFactor);
			else net.Study(input, unDamagedFactor);
		}
	}
	static public void Save() {
		net.Save();
	}
}
public class TipsController : Singleton<TipsController>
{
	public class Answer
	{
		public Teller teller;
		public Direction direction;
		public Answer(Teller teller,Direction direction) {
			this.teller = teller;
			this.direction = direction;
		}
	}
	public GameObject crazyObject;
	public GameObject kindlyObject;
	public GameObject angryObject;
	public List<Answer> answers;

	private bool isInitialized = false; 

	public int textBound;
	private string angryFileName = "angry.json";
	private string kindlyFileName = "kindly.json";
	private string fileName = "crazy.json";

	public List<TipsGroup> tipsGroups;
	public List<GameObject> onScene = new List<GameObject>();
	public void Init() {
		Teller crazy = new Teller("crazy", crazyObject, 1);
		Teller kindly = new Teller("kindly", kindlyObject, 2);
		Teller angry = new Teller("angry", angryObject, 3);
		answers = new List<Answer>() {
			new Answer(crazy, Direction.Down),
			new Answer(kindly, Direction.Down),
			new Answer(angry, Direction.Down)
		};
		isInitialized = true;
	}
	public void Study(bool isDamaged) {
		NeuralAdapter.Study(isDamaged);
	}
	public void SetTips(List<Vector2> possibleDirection) {
		if (!isInitialized) Init();
		var length = possibleDirection.Count;
		for (int i = 0; i < length; i++) {
			int randDirection = Random.Range(0, possibleDirection.Count);
			//TODO:
			answers[i].direction = NeuralAdapter.GetPhraseDirection(answers[i].teller,
																	(int)LevelBridge.Instance.trapFree,
																	possibleDirection[randDirection],
																	LevelGenerator.Instance.curLevelNumber,
																	answers[i].direction);
			
			answers[i].teller.SetDirectionPhrase(answers[i].direction);
			answers[i].teller.Place(possibleDirection[randDirection], textBound);
			possibleDirection.RemoveAt(randDirection);
		}
	}
	//TODO
	public void Clear() {
		crazyObject.transform.position = new Vector3(-1000, -1000, -1000);
		angryObject.transform.position = new Vector3(-1000, -1000, -1000);
		kindlyObject.transform.position = new Vector3(-1000, -1000, -1000);
	}
	public void ClearData() {
		SaveLoad.DeleteFile();
	}
}
