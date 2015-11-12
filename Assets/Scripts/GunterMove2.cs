using UnityEngine;
using System.Collections;

public class GunterMove2 : MonoBehaviour {
	public Transform transform;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x <= 394.81)
			transform.position = new Vector3(transform.position.x + Time.deltaTime * 2, transform.position.y + Time.deltaTime * 3, transform.position.z);
	}
}