﻿using UnityEngine;
using System.Collections;

public class snowRotation : MonoBehaviour {
	public Transform transform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.back * Time.deltaTime * 50);
	}
}
