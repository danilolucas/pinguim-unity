using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public Transform player;
	private Animator animator;

	private float attackTime;
	public float attackDelay;
	public bool attacked;

	public Transform side;

	public GameObject[] shootImage;

	public float shootSpeed;

	private bool touchFire = false;

	// Use this for initialization
	void Start () {
		animator = player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		basicAttack();
	
	}

	void basicAttack(){
		//attack
		if((Input.GetButtonDown("Fire1") || touchFire) && !attacked){
			attackTime = attackDelay;
			animator.ResetTrigger("nonAttack");
			animator.SetTrigger("attack");
			attacked = true;
			Instantiate(shootImage[0], side.position, transform.rotation);
		}
		
		attackTime -= Time.deltaTime;
		
		if(attackTime <= 0 && attacked){
			animator.ResetTrigger("attack");
			animator.SetTrigger("nonAttack");
			attacked = false;
		}
	}

	public void fireTouch(){
		touchFire = true;
	}
	public void resetFire(){
		touchFire = false;
	}
}
