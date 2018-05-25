using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LevelElement { Floor, Enemy, Obstacle, Player, Door, Border, BorderEdge, Trap, Reserved }

public abstract class LevelElements: MonoBehaviour {
	protected LevelElement[,] map;
	protected float elementsDistance;
	protected GameObject element;
	protected int height;
	protected int width;
	protected int currentLevel;
	protected Transform parent;
	public void Init(GameObject element, float elementsDistance, Transform globalParent) {
		this.element = element;
		this.elementsDistance = elementsDistance;
		parent = new GameObject().transform;
		parent.gameObject.name = GetType().Name;
		parent.transform.parent = globalParent;
	}
	public void SetMap(LevelElement[,] map, int height, int width, int currentLevel) {
		this.map = map;
		this.height = height;
		this.width = width;
		this.currentLevel = currentLevel;
	}
	protected bool IsEmpty(int y, int x) {
		return map[y, x] == LevelElement.Floor;
	}
	public abstract LevelElement[,] SetElements();
	protected abstract void SetElement(int y, int x);
	public abstract void PlaceElements();
	protected abstract void PlaceElement(int y, int x);
	}

public class Floor: LevelElements
{
	public override LevelElement[,] SetElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				SetElement(i, j);
			}
		}
		return this.map;
		}
	protected override void SetElement(int y,int x) {
		map[y, x] = LevelElement.Floor;
	}
	public override void PlaceElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				PlaceElement(i, j);
			}
		}
	}
	protected override void PlaceElement(int y, int x) {
		Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z), 
					Quaternion.identity, parent);
	}
}

public class Enemies : LevelElements
{
	int enemyCount = 3;
	public override LevelElement[,] SetElements() {
		enemyCount += currentLevel;
		for (int i = 0; i < enemyCount; i++) {
			var y = 0;
			var x = 0;
			while (!IsEmpty(y, x)) {
				y = Random.Range(0, height);
				x = Random.Range(0, width);
			}
			SetElement(y, x);
		}
		return this.map;
	}
	protected override void SetElement(int y, int x) {
		map[y, x] = LevelElement.Enemy;
	}
	public override void PlaceElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if(map[i,j] == LevelElement.Enemy) PlaceElement(i, j);
			}
		}
	}
	protected override void PlaceElement(int y, int x) {
		var enemy = Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z),
					Quaternion.identity, parent);
		enemy.tag = "Enemy";
	}
}

public class Obstacles : LevelElements
{
	int padding = 2;
	int obstacleCount = 5;
	public override LevelElement[,] SetElements() {
		obstacleCount += currentLevel * 2;
		for (int i = 0; i < obstacleCount; i++) {
			var y = 0;
			var x = 0;
			while (!IsEmpty(y, x)) {
				y = Random.Range(0, height - padding);
				x = Random.Range(0, width - padding);
			}
			SetElement(y, x);
		}
		return this.map;
	}
	protected override void SetElement(int y, int x) {
		map[y, x] = LevelElement.Obstacle;
	}
	public override void PlaceElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (map[i, j] == LevelElement.Obstacle)  PlaceElement(i, j);
			}
		}
	}
	protected override void PlaceElement(int y, int x) {
		Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z),
					Quaternion.identity, parent);

	}
}

public class MainPlayer : LevelElements
{
	int xPos;
	int yPos;
	public override LevelElement[,] SetElements() {
		xPos = width / 2;
		yPos = height / 2;
		SetElement(yPos,xPos);
		return this.map;
	}
	protected override void SetElement(int y, int x) {
		map[y, x] = LevelElement.Player;
	}
	public override void PlaceElements() {
		PlaceElement(yPos, xPos);
	}
	protected override void PlaceElement(int y, int x) {
		var player = Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z),
			Quaternion.identity, parent);
		player.transform.tag = "Player";
	}
}

public class Doors : LevelElements
{
	int doorHeight = 3;
	int doorWidth = 4;
	public override LevelElement[,] SetElements() {
		SetElement(height / 2, 0);
		SetElement(height / 2, width - 1);
		SetElement(0, width / 2);
		SetElement(height - 1, width / 2);
		return this.map;
	}
	protected override void SetElement(int y, int x) {
		for (int i = 0; i < doorHeight; i++) {
			for (int j = 0; j < doorWidth; j++) {
				int yPos = y - doorHeight/2 + i;
				int xPos = x - doorWidth/2 + j;
				if (yPos < height && xPos < width && yPos >= 0 && xPos >= 0)
					map[yPos, xPos] = LevelElement.Reserved;
			}
		}
		map[y, x] = LevelElement.Door;
	}
	public override void PlaceElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (map[i, j] == LevelElement.Door) PlaceElement(i,j);
			}
		}
	}

	protected override void PlaceElement(int y, int x) {
		Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z), 
					Rotation(y,x), parent);
	}
	private Quaternion Rotation(int y, int x) {
		if (y == height / 2 && x == 0) return Quaternion.Euler(0, 0, 90);
		else if (y == height / 2 && x == width - 1) return Quaternion.Euler(0, 0, -90);
		else if (y == 0 && x == width / 2) return Quaternion.Euler(0, 0, 180);
		else if (y == height - 1 && x == width / 2) return Quaternion.Euler(0, 0, 0);
		else throw new System.ArgumentException("try rotate not door");
	}
}

public class Borders : LevelElements
{
	public override LevelElement[,] SetElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (IsEmpty(i, j) && (i == 0 || j == 0 || i == height - 1 || j == width - 1))
					SetElement(i, j);
			}
		}
		return this.map;
	}
	protected override void SetElement(int y, int x) {
		map[y, x] = LevelElement.Border;
	}
	public override void PlaceElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (map[i, j] == LevelElement.Border) PlaceElement(i, j);
			}
		}
	}
	protected override void PlaceElement(int y, int x) {
		Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z), 
					Rotation(y, x), parent);
	}
	private Quaternion Rotation(int y, int x) {
		if (y == 0) return Quaternion.Euler(0, 0, -90);
		else if (y == height - 1) return Quaternion.Euler(0, 0, 90);
		else if (x == 0) return Quaternion.Euler(0, 0, 180);
		else if (x == width - 1) return Quaternion.Euler(0, 0, 0);
		else throw new System.ArgumentException("try rotate not border");
	}
}

public class BordersEdge : LevelElements
{
	public override LevelElement[,] SetElements() {
		SetElement(0,0);
		SetElement(0, width - 1);
		SetElement(height - 1, width -1);
		SetElement(height -1, 0);
		return this.map;
	}
	protected override void SetElement(int y, int x) {
		map[y, x] = LevelElement.BorderEdge;
	}
	public override void PlaceElements() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (map[i, j] == LevelElement.BorderEdge) PlaceElement(i,j);
			}
		}
	}
	protected override void PlaceElement(int y, int x) {
		Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z),
					Rotation(y, x), parent);
	}
	private Quaternion Rotation(int y, int x) {
		if (y == 0 && x == 0) return Quaternion.Euler(0, 0, 0);
		else if ((y == height - 1) && (x == width - 1)) return Quaternion.Euler(0, 0, 180);
		else if ((x == 0) && (y == height - 1)) return Quaternion.Euler(0, 0, -90);
		else if (x == width - 1 && y == 0) return Quaternion.Euler(0, 0, 90);
		else throw new System.ArgumentException("try rotate not edge");
	}
}