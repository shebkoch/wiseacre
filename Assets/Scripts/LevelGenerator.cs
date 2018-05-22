using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelGenerator : MonoBehaviour {
	private enum Element { Floor, Enemy, Obstacle, Player, Door, Border, BorderEdge }
	public int width;
	public int height;
	public int enemyCount;
	public int obstacleCount;
	public float elementsDistance;
	public List<GameObject> elements = new List<GameObject>();
	private Element[,] level;
	public LevelGenerator Instance { get; private set; }
	void Awake() {
		Instance = this;
		Generate();
	}
	void SetBasic() {       //Set floor and edge
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (i == 0 || j == 0 || i == height - 1 || j == width - 1)
					level[i, j] = Element.Border;
				else level[i, j] = Element.Floor;
				if (i == 0 && j == 0 ||
					i == 0 && j == width- 1 ||
					i == height - 1 && j == width -1 ||
					i == height - 1 && j == 0)
					level[i, j] = Element.BorderEdge;
			}
		}
	}
	void SetDoor() {
		level[height / 2, 0] = Element.Door;
		level[height / 2, width-1] = Element.Door;
		level[0, width / 2] = Element.Door;
		level[height-1, width / 2] = Element.Door;
	}
	bool isEmpty(int y,int x) {
		return level[y, x] == Element.Floor;
	}
	void SetEnemy() {
		for (int i = 0; i < enemyCount; i++) {
			var y = 0;
			var x = 0;
			while (!isEmpty(y, x)) {
				y = Random.Range(0, height);
				x = Random.Range(0, width);
			}
			level[y, x] = Element.Enemy;
		}
	}
	void SetPlayer() {
		var x = 0;
		var y = 0;
		while(!isEmpty(y,x)) {
			y = Random.Range(0, height);
			x = Random.Range(0, width);
		}
		level[y, x] = Element.Player;
	}
	void SetObstacle() {
		for (int i = 0; i < obstacleCount; i++) {
			var y = 0;
			var x = 0;
			while (!isEmpty(y, x)) {
				y = Random.Range(0, height);
				x = Random.Range(0, width);
			}
			level[y, x] = Element.Obstacle;
		}
	}
	void PlaceBorder(int y,int x, GameObject element) {
		Quaternion rotation = Quaternion.identity;
		if (y == 0) rotation = Quaternion.Euler(0, 0, -90);
		if (y == height-1) rotation = Quaternion.Euler(0, 0, 90);
		if (x == 0) rotation = Quaternion.Euler(0, 0, 180);
		if (x == width -1) rotation = Quaternion.Euler(0, 0, 0);
		Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z), rotation);
	}

	void PlaceBorderEdge(int y, int x, GameObject element) {
		Quaternion rotation = Quaternion.identity;
		if (y == 0 && x == 0) rotation = Quaternion.Euler(0, 0, 0) ;
		if ((y == height - 1) && (x == width -1)) rotation = Quaternion.Euler(0, 0, 180);
		if ((x == 0) && (y == height - 1)) rotation = Quaternion.Euler(0, 0, -90);
		if (x == width - 1 && y == 0) rotation = Quaternion.Euler(0, 0, 90);
		Instantiate(element, new Vector3(x * elementsDistance, y * elementsDistance, element.transform.position.z), rotation);
	}
	void PlaceAll() {
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				var element = elements[(int)level[i, j]];
				if (level[i, j] == Element.Border) PlaceBorder(i, j, element);
				else if (level[i, j] == Element.BorderEdge) PlaceBorderEdge(i,j,element);
				else
					Instantiate(element, new Vector3(i * elementsDistance, j * elementsDistance, element.transform.position.z), Quaternion.identity);
			}
		}
	}
	void Generate() {
		level = new Element[height, width];
		SetBasic();
		SetDoor();
		SetEnemy();
		SetPlayer();
		SetObstacle();
		PlaceAll();
	}
}
