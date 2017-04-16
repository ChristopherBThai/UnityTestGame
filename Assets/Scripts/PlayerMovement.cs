using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public Animator animator;
	public EnemySpawner es;
	public SpriteRenderer sr;
	public Rigidbody2D rb;
	public float speed;
	public float attackRange;
	public float attackDelay;

	private float attackTime;
	private float blockTime;

	void Start () {
		//animator = GetComponent<Animator> ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftShift)) {
			animator.SetBool ("PlayerMove", false);
			animator.SetBool ("PlayerBlock", true);
			blockTime = .6f;
		} else if (animator.GetBool ("PlayerBlock")) {
			animator.SetBool ("PlayerBlock", false);
			blockTime = .6f;
		}
		if (blockTime > 0||animator.GetBool("PlayerBlock")) {
			blockTime -= Time.deltaTime;
			return;
		}


		bool moving = false;
		Vector3 vel = new Vector3 ();
		if(animator.GetBool("PlayerAttack")){
		}else if (Input.GetKey (KeyCode.D)) {
			sr.flipX = false;
			vel.x = speed;
			moving = true;
		} else if (Input.GetKey (KeyCode.A)) {
			sr.flipX = true;
			moving = true;
			vel.x = -speed;
		}

		if (Input.GetKey (KeyCode.E)&&!animator.GetBool("PlayerAttack")) {
			moving = false;
			animator.SetTrigger ("PlayerAttack");
			attackTime = attackDelay;
			vel.x = 0;
		}

		if (attackTime > 0) {
			attackTime -= Time.deltaTime;
			if (attackTime <= 0)
				attack ();
		}

		animator.SetBool ("PlayerMove",moving);
		transform.Translate (vel);
	}

	void attack(){
		foreach (GameObject o in es.getEnemies()) {
			float diff = o.transform.position.x - transform.position.x;
			if (!sr.flipX && diff > 0 && diff < attackRange) {
				o.GetComponent<EnemyBehavior> ().hit (-1);
			} else if (sr.flipX && diff < 0 && diff > -attackRange) {
				o.GetComponent<EnemyBehavior> ().hit (1);
			}
		}
	}

	public bool isBlocking(){
		return blockTime <= 0 && animator.GetBool ("PlayerBlock");
	}
	
}
