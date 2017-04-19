using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	public GameObject target;
	public float speed;
	public float attackRange;
	public float attackDelay;
	public int health;

	private Animator animator;
	private SpriteRenderer sr;
	private Rigidbody2D rb;

	private Vector3 vel;

	private float attackTimer;

	private bool dead;
	private float deadTimer;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		setTarget (target);
		dead = false;
	}

	// Update is called once per frame
	void Update () {
		if(!dead&&!animator.GetBool("EnemyHit")&&!checkForTarget())
			transform.Translate (vel);
		checkDead ();
		attackIntervalCheck ();
	}

	public void setTarget(GameObject o){
		if (o == null)
			return;
		sr = GetComponent<SpriteRenderer> ();
		target = o;
		float tempSpeed = speed;
		if (target.transform.position.x > transform.position.x) {
			sr.flipX = false;
		} else {
			tempSpeed *= -1;
			sr.flipX = true;
		}
		vel = new Vector3 (tempSpeed, 0, 0);
	}

	bool checkForTarget(){
		if (animator.GetBool ("EnemyAttack"))
			return true;
		if (Mathf.Abs (target.transform.position.x - transform.position.x) > attackRange)
			return false;
		attack ();
		return true;
	}

	void attack(){
		animator.SetTrigger ("EnemyAttack");
		attackTimer = attackDelay;
	}

	void attackIntervalCheck(){
		if (dead || animator.GetBool ("EnemyHit"))
			attackTimer = 0f;
		else if (attackTimer > 0) {
			attackTimer -= Time.deltaTime;
			if (attackTimer <= 0) {
				if (sr.flipX)
					target.GetComponent<PlayerMovement> ().hit (1);
				else
					target.GetComponent<PlayerMovement> ().hit (-1);
			}
		}
	}

	public void hit(int dir){
		if(dir<0)
			rb.AddForce(new Vector2(rb.mass*200,rb.mass*80));
		if(dir>0)
			rb.AddForce(new Vector2(-rb.mass*200,rb.mass*80));
		health--;
		animator.SetTrigger ("EnemyHit");
		if(health<=0)
			die ();
	}

	void die(){
		animator.SetTrigger ("EnemyDie");
		deadTimer = .7f;
		dead = true;
	}

	public bool isDead(){
		return !enabled;
	}

	void checkDead(){
		if (dead) {
			deadTimer -= Time.deltaTime;
			if (deadTimer <= 0)
				enabled = false;
		}
		if (transform.position.y < -100)
			enabled = false;
	}
}
