using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GestureRecognizer;

using UnityEngine;

public class Spells : MonoBehaviour {
	static Dictionary<string, Spell> spells = new Dictionary<string, Spell>();

	public void Start() {
		spells.Add("up", new Directional());
		spells.Add("W", new Fireball());
		spells.Add("bolt", new Directional.Bolt());
	}
	public void Cast(RecognitionResult result) {
		if (result != RecognitionResult.Empty)
			spells[result.gesture.id].Cast();
	}
	public abstract class Spell {
		public abstract void Cast();
	}
	
	public class Fireball : Spell {
		public float shellForce = 400;

		public override void Cast() {
			var fireball = Resources.Load<GameObject>("fireball");
			var playerPos = FindObjectOfType<Player>().transform.position; //op
			var enemies = FindObjectsOfType<EnemyLogic>().Where(x => x.isAlive).ToArray();
			if (enemies.Length == 0) return;
			var target = enemies.OrderBy(x => Vector2.Distance(x.gameObject.transform.position, playerPos)).First()
				.gameObject;
			fireball = Instantiate(fireball, playerPos, Quaternion.identity);
			fireball.transform.right = target.transform.position - fireball.transform.position;
			var direction = target.transform.position - fireball.transform.position;
			direction.Normalize();
			fireball.GetComponent<Rigidbody2D>().AddForce(direction * shellForce, ForceMode2D.Force);
		}
	}
	public class Directional : Spell {
		public int shellForce = 400;
		public override void Cast() {
			Direction direction = PlayerInput.PlayerDirection;
			var playerPos = FindObjectOfType<Player>().transform.position; //op
			var dir = Vector3.zero;
			switch (direction) {
				case Direction.Left:
					dir = Vector3.left;
					break;
				case Direction.Right:
					dir = Vector3.right;
					break;
				case Direction.Down:
					dir = Vector3.down;
					break;
				case Direction.Up:
					dir = Vector3.up;
					break;
			}
			var fireball = Resources.Load<GameObject>("tornado");
			fireball = Instantiate(fireball, playerPos, Quaternion.identity);
			fireball.GetComponent<Rigidbody2D>().AddForce(dir * shellForce, ForceMode2D.Force);
		}
		public class Bolt : Spell {
			public override void Cast() {
				var enemies = FindObjectsOfType<EnemyLogic>().Where(x => x.isAlive).ToArray();
				var playerPos = FindObjectOfType<Player>().transform.position; //op
				if (enemies.Length == 0) return;
				var random = Random.Range(0, enemies.Length);
				var target = enemies[random];
				target.ForceKill();
			}
		}
	}
}
