using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float walkSpeed;
	public float dashUpForce;
	public float dashDownForce;
	public Transform player;
	private Animator animator;

	private bool sitDown;

	public bool isGrounded;
	public bool isWalled;
	public bool isBoxSide;
	public bool isBoxTop;
	public bool isStaired;
	public bool isGroundInStair;
	public bool inAir;

	public float force;
	private float jumpTime;
	public float jumpDelay;
	public bool jumped;
	public bool wallJump;
	public Transform bottom;
	public Transform side;
	public Transform top;

	public bool walled;
	public bool staired;

	private float dashUpTime;
	public float dashUpDelay;
	private float dashDownTime;
	public float dashDownDelay;
	public bool dashedUp;
	public bool dashedDown;

	private bool rightDirection = true;
	private bool leftDirection = true;

	public CircleCollider2D playerBody;
	public BoxCollider2D playerHead;

	public bool isKinematic;

	private bool touchRight = false;
	private bool touchLeft = false;
	private bool touchJump = false; 
	private bool touchAttack = false; 
	private bool touchUp = false;
	private bool touchDown = false;

	// Use this for initialization
	void Start () {
		animator = player.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		physics();
		move();
		jump();
		dash();
		stair();

		if (transform.position.y <= 100) {
			Application.LoadLevel ("secreteLevel");
		}
	}

	void physics(){
		//seta na variavel se o player esta no chao
		isGrounded = Physics2D.Linecast(this.transform.position, bottom.position, 1 << LayerMask.NameToLayer("Plataforma"));
		//seta na variavel se o player esta tocando na parede
		isWalled = Physics2D.Linecast(side.position, this.transform.position, 1 << LayerMask.NameToLayer("Plataforma"));
		//seta na variavel se o player esta tocando na lateral da caixa
		isBoxSide = Physics2D.Linecast(side.position, this.transform.position, 1 << LayerMask.NameToLayer("Box"));
		//seta na variavel se o palyer esta tocando em cima da caixa
		isBoxTop = Physics2D.Linecast(this.transform.position, bottom.position, 1 << LayerMask.NameToLayer("Box"));
		//seta na variavel se o player esta na escada
		isStaired = Physics2D.Linecast(top.position, top.position, 1 << LayerMask.NameToLayer("Stair"));
		//sete na variavel se a tag  esta na escada
		isGroundInStair = Physics2D.Linecast(bottom.position, bottom.position, 1 << LayerMask.NameToLayer("Stair"));

		if(isBoxTop) //faz com que se o personagem estiver em cima da caixa seja como se ele estivesse no chao
			isGrounded = true;

		//air
		if(!jumped && (!isGrounded && !isStaired) || !jumped && (!isGrounded && isStaired)){
			animator.ResetTrigger("nonAir");
			animator.SetTrigger("air");
			inAir = true;
		}
		else{
			if(inAir && isGrounded){
				animator.ResetTrigger("air");
				animator.SetTrigger("nonAir");
				inAir = false;
			}
		}

		//wallHold
		if((Input.GetAxisRaw("Horizontal")!=0 || touchRight || touchLeft)&& isWalled && !isGrounded && !wallJump){
			if(!isKinematic){
				GetComponent<Rigidbody2D>().isKinematic = true;
				isKinematic = true;
			}
			else{
				GetComponent<Rigidbody2D>().drag = 6; //faz a gravidade contraria para gerar o atrito com a parede
				animator.ResetTrigger("nonWall");
				animator.SetTrigger("wall");
				walled = true;
				jumped = false;
				rightDirection = false;
				leftDirection = false;
				GetComponent<Rigidbody2D>().isKinematic = false;
			}
		}
		else{
			if(walled){
				GetComponent<Rigidbody2D>().drag = 0;
				animator.ResetTrigger("wall");
				animator.SetTrigger("nonWall");
				walled = false;
				isKinematic = false;
			}
		}
	}

	//touch walk methods
	public void moveRight(){
		touchRight = true;
		if(isGrounded){
			animator.SetFloat("walk", 1);
		}
	}
	public void moveLeft(){
		touchLeft = true;
		if(isGrounded){
			animator.SetFloat("walk", 1);
		}
	}
	public void jumpTouch(){
		touchJump = true;
	}
	public void upTouch(){
		touchUp = true;
	}
	public void downTouch(){
		touchDown = true;
		animator.SetTrigger("down");
	}

	public void resetMove(){
		touchRight = false;
		touchLeft = false;
		animator.SetFloat("walk", 0);
	}
	public void resetJump(){
		touchJump = false;
	}
	public void resetUp(){
		touchUp = false;
	}
	public void resetDown(){
		touchDown = false;
	}

	void move(){

		//walk
	    	animator.SetFloat("walk", Mathf.Abs(Input.GetAxis("Horizontal")));

		if(rightDirection){
			if(Input.GetAxisRaw("Horizontal")>0){
				if(!isWalled){
					transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);
				}
				transform.eulerAngles = new Vector2(0,0);
			}
		}
		if(leftDirection){
			if(Input.GetAxisRaw("Horizontal")<0){
				if(!isWalled){
					transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);
				}
				transform.eulerAngles = new Vector2(0,180);
			}
		}

		//down(abaixar)
		if(Input.GetAxisRaw("Vertical")<0){
			if((isStaired && !isGroundInStair) || (!isStaired && !isGroundInStair)){
				animator.SetTrigger("down");
			}
			else{
				staired = true;
			}
			sitDown = true;
		}
		else{
			if(sitDown){
				animator.ResetTrigger("down");
				animator.SetTrigger("up");
				sitDown = false;
				rightDirection = true;
				leftDirection = true;
			}
		}
	}

	void jump(){
		//WallJump
		if((Input.GetButtonDown("Jump") || touchJump)&& (Input.GetAxisRaw("Horizontal")!=0 || touchRight || touchLeft) && !isGrounded && !jumped && isWalled){
			GetComponent<Rigidbody2D>().drag = 0;
			GetComponent<Rigidbody2D>().AddForce(transform.right * (-force/4));
			GetComponent<Rigidbody2D>().AddForce(transform.up * (force + 100));
			jumpTime = 0.4f;
			animator.ResetTrigger("air");
			animator.SetTrigger("jump");
			jumped = true;
			wallJump = true;
		}
		
		//jump
		if((Input.GetButtonDown("Jump") || touchJump)&& isGrounded && !jumped && !sitDown){
			GetComponent<Rigidbody2D>().AddForce(transform.up * force);
			jumpTime = jumpDelay;
			animator.SetTrigger("jump");
			jumped = true;
		}
		
		jumpTime -= Time.deltaTime;
		
		if(jumpTime <= 0.1){
			rightDirection = true;
			leftDirection = true;
		}
		if(jumpTime <= 0 && jumped){
			animator.SetTrigger("air");
			animator.ResetTrigger("nonDashUp");
			jumped = false;
			wallJump = false;
		}
	}

	void dash(){
		//dashUp
		if(Input.GetButton("Fire2") && !dashedUp && !dashedDown && !sitDown){
			GetComponent<Rigidbody2D>().AddForce(transform.right * dashUpForce); //gera uma força para frente, criando o dash
			dashUpTime = dashUpDelay;
			animator.SetTrigger("dashUp");
			dashedUp = true;
			rightDirection = false;
			leftDirection = false;
		}
		
		//Timer dashe up
		dashUpTime -= Time.deltaTime;
		
		if(dashUpTime <= 0 && dashedUp){
			animator.SetTrigger("nonDashUp");
			dashedUp = false;
			rightDirection = true;
			leftDirection = true;
		}

		//dashDown
		if(sitDown && Input.GetButton("Jump") && !dashedDown && !dashedUp && isGrounded){
			GetComponent<Rigidbody2D>().AddForce(transform.right * dashDownForce); //gera uma força para frente, criando o dash
			dashDownTime = dashDownDelay;
			animator.SetTrigger("dashDown");
			dashedDown = true;
			rightDirection = false;
			leftDirection = false;
		}
		
		//Timer dashe down
		dashDownTime -= Time.deltaTime;
		
		if(dashDownTime <= 0 && dashedDown){
			animator.SetTrigger("nonDashDown");
			dashedDown = false;
			rightDirection = true;
			leftDirection = true;
		}
	}

	void stair(){
		//Stair (subir escada)
		if((Input.GetAxisRaw("Vertical")>0 || touchUp)&& isStaired){
			GetComponent<Rigidbody2D>().isKinematic = true;
			transform.Translate(Vector2.up * walkSpeed * Time.deltaTime);
			animator.enabled = true;
			animator.SetTrigger("upStair");
			animator.ResetTrigger("nonUpStair");
			staired = true;
			playerBody.isTrigger = true;
			playerHead.isTrigger = true;
		}
		else{
			if(staired){
				animator.enabled = false;
				if(!isStaired)
					staired = false;
				//descer escada
				if((Input.GetAxisRaw("Vertical")<0  || touchDown)&& ((isStaired && isGroundInStair) || (!isStaired && isGroundInStair))){
					GetComponent<Rigidbody2D>().isKinematic = true;
					transform.Translate(Vector2.up * (-walkSpeed) * Time.deltaTime);
					animator.enabled = true;
					animator.SetTrigger("upStair");
					animator.ResetTrigger("nonUpStair");
					staired = true;
					playerBody.isTrigger = true;
					playerHead.isTrigger = true;
				}
			}
			else{
				if(!isStaired && !isGroundInStair && !isWalled){
					GetComponent<Rigidbody2D>().isKinematic = false;
					animator.enabled = true;
					animator.SetTrigger("nonUpStair");
					animator.ResetTrigger("upStair");
					playerBody.isTrigger = false;
					playerHead.isTrigger = false;
				}
			}
		}
	}
}
