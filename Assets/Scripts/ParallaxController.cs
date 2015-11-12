using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	private Material currentMaterial;
	public float offSet;
	public Transform player;

	// Use this for initialization
	void Start () {
	
		currentMaterial = GetComponent<Renderer>().material;

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetAxisRaw("Horizontal")>0){
			offSet -= 0.0001f;
			currentMaterial.SetTextureOffset("_MainTex", new Vector2(offSet, 0));
		}

		if(Input.GetAxisRaw("Horizontal")<0){
			offSet += 0.0001f;
			currentMaterial.SetTextureOffset("_MainTex", new Vector2(offSet, 0));
		}

	}
}
