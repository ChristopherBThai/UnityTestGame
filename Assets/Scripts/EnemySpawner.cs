using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemy;
	public GameObject target;
	public Camera cam;
	public int spawnTime;
	public int enemyCount;
	private int time;
	private float width;
	private List<GameObject> enemies;
	private EnemyBehavior behavior;

	// Use this for initialization
	void Start () {
		enemies = new List<GameObject>();
		time = spawnTime;
		float height = 2f * cam.orthographicSize;
		width = height * cam.aspect;
		width /= 2;
		width += enemy.GetComponent<SpriteRenderer> ().bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(time<=0&&enemies.Count<enemyCount){
			time = spawnTime;
			float tempWidth;
			if (Random.value >= .5f)
				tempWidth = width;
			else
				tempWidth = -width;
			GameObject temp = Instantiate (enemy, new Vector2 (cam.transform.position.x+tempWidth,cam.transform.position.y), new Quaternion ());
			temp.GetComponent<EnemyBehavior> ().setTarget (target);
			enemies.Add (temp);
		}
		time--;

		checkDeadEnemies ();
	}

	void checkDeadEnemies(){
		foreach (GameObject o in enemies) {
			behavior = o.GetComponent<EnemyBehavior> ();
			if (behavior.isDead()) {
				enemies.Remove (o);
				Destroy (o);
			}
		}
	}

	public List<GameObject> getEnemies(){
		return enemies;
	}
}
