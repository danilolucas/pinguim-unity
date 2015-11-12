using UnityEngine;
using System.Collections;

public class GameSounds : MonoBehaviour {

	public bool isGrounded;
	public bool isWalled;
	public bool isStaired;

	public bool outGround;

	public Transform bottom;
	public Transform side;
	public Transform top;

	public AudioClip walkSound;
	public AudioClip jumpSound;
	public AudioClip groundSound;
	public AudioClip lowDashSound;

	public bool playedSound;
	public float soundTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//seta na variavel se o player esta no chao
		isGrounded = Physics2D.Linecast(this.transform.position, bottom.position, 1 << LayerMask.NameToLayer("Plataforma"));
		//seta na variavel se o player esta tocando na parede
		isWalled = Physics2D.Linecast(side.position, this.transform.position, 1 << LayerMask.NameToLayer("Plataforma"));
		//seta na variavel se o player esta na escada
		isStaired = Physics2D.Linecast(top.position, top.position, 1 << LayerMask.NameToLayer("Stair"));

		if(!isGrounded && !isStaired){
			outGround = true;
		}
		else{
			if(outGround && isGrounded){
				GetComponent<AudioSource>().PlayOneShot(groundSound);
				outGround = false;
				playedSound = true;
				soundTime = 0.3f;
			}
		}

		if(Input.GetAxisRaw("Horizontal")>0 && !playedSound && isGrounded){
			GetComponent<AudioSource>().PlayOneShot(walkSound);
			playedSound = true;
			soundTime = 0.4f;
		}
		if(Input.GetAxisRaw("Horizontal")<0 && !playedSound && isGrounded){
			GetComponent<AudioSource>().PlayOneShot(walkSound);
			playedSound = true;
			soundTime = 0.4f;
		}

		if(Input.GetButtonDown("Jump") && (isGrounded || isWalled)){
			GetComponent<AudioSource>().PlayOneShot(jumpSound);
			playedSound = true;
		}
		
		soundTime -= Time.deltaTime;
		
		if(soundTime <= 0 && playedSound){
			playedSound = false;
		}
	
	}
}
