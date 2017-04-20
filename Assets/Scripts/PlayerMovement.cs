using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public EnemySpawner es;

	public float speed;

	public float attackRange;

	public int health;

	private Animator animator;
	private SpriteRenderer sr;
	private Rigidbody2D rb;
	private float blockTime;

	void Start () {
		animator = GetComponent<Animator> ();
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		if (checkBlock ())
			return;
		playerControls ();
	}

	bool checkBlock(){
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
			return true;
		}
		return false;
	}

	void playerControls(){
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

		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("PlayerAttack")) {
			moving = false;
			vel.x = 0;
		}else if (Input.GetKey (KeyCode.E)) {
			moving = false;
			animator.SetTrigger ("PlayerAttack");
			vel.x = 0;
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

	public void hit(int dir){
		if (isBlocking()) {

		} else {
			animator.SetTrigger ("PlayerHit");
			if(dir<0)
				rb.AddForce(new Vector2(rb.mass*100,rb.mass*80));
			if(dir>0)
				rb.AddForce(new Vector2(-rb.mass*100,rb.mass*80));
		}

	}
	
}
