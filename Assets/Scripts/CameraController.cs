using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private float xOffset;
	private Vector3 pos;

	// Use this for initialization
	void Start () {
		xOffset = transform.position.x - player.transform.position.x;
		pos = new Vector3(xOffset,transform.position.y,transform.position.z);
	}
	
	//Runs as the last update lines
	void LateUpdate () {
		pos.x = xOffset + player.transform.position.x;
		transform.position = pos;
	}
}
