using UnityEngine;
using System.Collections;

public class ShootPhysics : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.GetComponent<Rigidbody2D>().AddForce(transform.right * speed);
	}

	void OnCollisionEnter2D(Collision2D collider){
		if(collider.gameObject.tag == "Plataforma" || collider.gameObject.tag == "Rope" || collider.gameObject.tag == "Box"){
			Destroy(gameObject);
		}
	}
}
