using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour {
	public GameObject right,left;
	public Camera cam;
	public GameObject player;
	public float moveSpeed;

	private float prevX;
	private float width;
	private Vector3 speed;
	private float camOffset;

	// Use this for initialization
	void Start () {
		width = right.transform.position.x - left.transform.position.x;
		speed = new Vector3 (0, 0, 0);
		camOffset = left.transform.position.x - cam.transform.position.x;
		prevX = player.transform.position.x;
	}

	// Update is called once per frame
	void Update () {
		moveBackground ();
		checkBounds ();
	}

	void moveBackground(){
		float dst = player.transform.position.x - prevX;
		prevX = player.transform.position.x;
		speed.x = dst*moveSpeed;
		left.transform.Translate (speed);
		right.transform.Translate (speed);
	}

	void checkBounds(){
		if (left.transform.position.x - cam.transform.position.x > camOffset) {
			right.transform.position = new Vector2 (left.transform.position.x-width, left.transform.position.y);
			GameObject temp = right;
			right = left;
			left = temp;
		} else if (right.transform.position.x - cam.transform.position.x < camOffset) {
			left.transform.position = new Vector2 (right.transform.position.x+width, right.transform.position.y);
			GameObject temp = right;
			right = left;
			left = temp;
		}
	}
}
