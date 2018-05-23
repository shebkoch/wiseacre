using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : Singleton<LevelGenerator> {
	public int width;
	public int height;
	public float elementsDistance;
	private LevelElement[,] level;
	private int curLevelNumber = 0;
	public Transform map;
	[Space(10)]
	[Header("Level elements")]
	public GameObject floor;
	public GameObject enemy;
	public GameObject obstacle;
	public GameObject player;
	public GameObject door;
	public GameObject border;
	public GameObject borderEdge;
	
	List<LevelElements> levelElements;
	void Awake() {
		LevelStart();
	}
	public void LevelStart() {
		Clear();
		Init();
		Generate();
		LevelBridge.Instance.Init();
	}
	public void Clear() {
		if (map) Destroy(map.gameObject);
		map = new GameObject().transform;
		map.name = "map";
	}
	void Init() {
		var floorClass = new Floor();
		var enemyClass = new Enemies();
		var obstacleClass = new Obstacles();
		var playerClass = new MainPlayer();
		var doorClass = new Doors();
		var borderClass = new Borders();
		var borderEdgeClass = new BordersEdge();
		floorClass.Init(floor, elementsDistance, map);
		enemyClass.Init(enemy, elementsDistance, map);
		obstacleClass.Init(obstacle, elementsDistance, map);
		playerClass.Init(player, elementsDistance, map);
		doorClass.Init(door, elementsDistance, map);
		borderClass.Init(border, elementsDistance, map);
		borderEdgeClass.Init(borderEdge, elementsDistance, map);
		levelElements = new List<LevelElements>() { floorClass, doorClass, borderClass, borderEdgeClass, playerClass, enemyClass, obstacleClass };
	}
	public void Generate() {
		level = new LevelElement[height, width];
		foreach (var element in levelElements) {
			element.SetMap(level, height, width, curLevelNumber);
			level = element.SetElements();
		}
		foreach (var element in levelElements) {
			element.SetMap(level, height, width, curLevelNumber);
			element.PlaceElements();
		}
	}
	
}
