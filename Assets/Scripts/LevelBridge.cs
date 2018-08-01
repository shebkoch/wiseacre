using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
	Left, Right, Down, Up
};
public class LevelBridge : Singleton<LevelBridge> {
	public bool isDamageRecieving;
	
	public int manaToLevel;
	private Direction playerDirection;
	private bool isBridgeBuild = false;
	private GameObject player;
	private int mapHeight;
	private int mapWidth;
	public int mapOffset;
	private bool isInitialized = false;
	private List<GameObject> enemies;
	private List<GameObject> doors;
	private bool isDoorOpen = false;
	public Direction trapFree;
	void TeleportPlayer() {  //Reverse player position
		Vector3 playerPos = DirToVector(playerDirection);
		playerPos.z = player.transform.position.z;
		player.transform.position = playerPos;
	}
	public Vector2 DirToVector(Direction dir) {
		Vector2 result = new Vector2();
		switch (dir) {
			case Direction.Left:	result = new Vector3(mapWidth - 1, mapHeight / 2);	break;
			case Direction.Right:	result = new Vector3(0, mapHeight / 2); break;
			case Direction.Down:	result = new Vector3(mapWidth / 2, mapHeight); break;
			case Direction.Up:		result = new Vector3(mapWidth / 2, 0); break;
		}
		return result;
	}
	public void Init() {
		mapHeight = LevelGenerator.Instance.height;
		mapWidth = LevelGenerator.Instance.width;
		player = GameObject.FindGameObjectWithTag("Player");
		doors = new List<GameObject>();
		enemies = new List<GameObject>();
		doors.AddRange(GameObject.FindGameObjectsWithTag("Door"));
		enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
		isInitialized = true;
	}
	void SetPlayerDirection() {
		var playerPos = player.transform.position;
		if (playerPos.y > mapHeight + mapOffset) {
			playerDirection = Direction.Up;
			isBridgeBuild = true;
		}
		if (playerPos.y < 0 - mapOffset) {
			playerDirection = Direction.Down;
			isBridgeBuild = true;
		}
		if (playerPos.x > mapWidth + mapOffset) {
			playerDirection = Direction.Right;
			isBridgeBuild = true;
		}
		if (playerPos.x < 0 - mapOffset) {
			playerDirection = Direction.Left;
			isBridgeBuild = true;
		}
	}
	Direction OppositeDirection(Direction dir) {
		if (dir == Direction.Left) return Direction.Right;
		if (dir == Direction.Right) return Direction.Left;
		if (dir == Direction.Down) return Direction.Up;
		if (dir == Direction.Up) return Direction.Down;
		throw new System.ArgumentException();
	}
	public List<Vector2> GetPossiblePath() {
		List<Vector2> result = new List<Vector2>();
		List<Direction> possibleDirection = new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
		possibleDirection.Remove(playerDirection);
		foreach (var dir in possibleDirection) {
			result.Add(DirToVector(dir));
		}
		return result;
	}
	void SetTraps() {
		List<Direction> possibleDirection = new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up };
		possibleDirection.Remove(OppositeDirection(playerDirection));
		trapFree = possibleDirection[Random.Range(0, possibleDirection.Count)];
	}
	void TrapDamage() {
		bool isDamaged = playerDirection != trapFree;
		if (isDamaged) {
			PlayerParametersController.Instance.Health--;//todo
		}
		else PlayerParametersController.Instance.Mana += manaToLevel;

		TipsController.Instance.Study(isDamaged);
	}
	public void OpenDoors() {
		if (enemies.Count > 0) {
			foreach (var enemy in enemies) {
				if (enemy.GetComponent<EnemyLogic>().IsAlive()) return;
			}
			isDoorOpen = true;
			TipsController.Instance.SetTips(GetPossiblePath());
			foreach (var door in doors) {
				door.SetActive(false);
			}
		}
	}
	void Update () {
		if (!player) {
			Init();
			TeleportPlayer();
		}
		if (isInitialized) {
			if (!isDoorOpen) {
				OpenDoors();
			}
			SetPlayerDirection();
		}
		if (isBridgeBuild) {
			isBridgeBuild = false;
			LevelGenerator.Instance.LevelStart();
			UIController.Instance.StartFade();
			TipsController.Instance.Clear();//TODO
			isDoorOpen = false;
			TeleportPlayer();
			if (isDamageRecieving)
				TrapDamage();
			SetTraps();
			
		}

	}
}
