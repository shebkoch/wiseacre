using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelBridge : Singleton<LevelBridge>
{
	public enum Dir
	{
		left, right, down, up
	};
	public Dir playerDirection;
	public bool isBridgeBuild = false;
	private GameObject player;
	private int mapHeight;
	private int mapWidth;
	public int mapOffset;
	private bool isInitialized = false;
	private List<GameObject> enemies;
	private List<GameObject> doors;

	public Dir trapFree;
	void TeleportPlayer() {	 //Reverse player position
		Vector3 playerPos = Vector3.zero;
		switch (playerDirection) {
			case Dir.left:	playerPos = new Vector3(mapWidth , mapHeight / 2); break;
			case Dir.right:	playerPos = new Vector3(0 , mapHeight / 2); break;
			case Dir.down:	playerPos = new Vector3(mapWidth / 2, mapHeight); break;
			case Dir.up:	playerPos = new Vector3(mapWidth / 2, 0); break;
		}
		playerPos.z = player.transform.position.z;
		player.transform.position = playerPos;
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
			playerDirection = Dir.up;
			isBridgeBuild = true;
		}
		if (playerPos.y < 0 - mapOffset) {
			playerDirection = Dir.down;
			isBridgeBuild = true;
		}
		if (playerPos.x > mapWidth + mapOffset) {
			playerDirection = Dir.right;
			isBridgeBuild = true;
		}
		if (playerPos.x < 0 - mapOffset) {
			playerDirection = Dir.left;
			isBridgeBuild = true;
		}
	}
	Dir OppositeDirection(Dir dir) {
		if (dir == Dir.left) return Dir.right;
		if (dir == Dir.right) return Dir.left;
		if (dir == Dir.down) return Dir.up;
		if (dir == Dir.up) return Dir.down;
		throw new System.ArgumentException();
	}
	void SetTraps() {
		List<Dir> possibleDirection = new List<Dir>() { Dir.down, Dir.left, Dir.right, Dir.up };
		possibleDirection.Remove(OppositeDirection(playerDirection));
		trapFree = possibleDirection[Random.Range(0, possibleDirection.Count)];
	}
	void TrapDamage() {
		if (playerDirection != trapFree) PlayerParametersController.Instance.Health--;//todo
	}
	public void OpenDoors() {
		if (enemies.Count > 0) {
			foreach (var enemy in enemies) {
				if (enemy.GetComponent<EnemyLogic>().IsAlive()) return;
			}
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
			OpenDoors();
			SetPlayerDirection();
		}
		if (isBridgeBuild) {
			isBridgeBuild = false;
			LevelGenerator.Instance.LevelStart();
			UIController.Instance.StartFade();
			TipsController.Instance.SetTips(Vector2.zero); //TODO:
			TeleportPlayer();
			TrapDamage();
			SetTraps();
		}

	}
}
