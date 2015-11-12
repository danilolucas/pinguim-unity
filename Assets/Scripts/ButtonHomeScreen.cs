using UnityEngine;
using System.Collections;

public class ButtonHomeScreen : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Jogar(){
		Application.LoadLevel ("JogadorScreen");
	}

	public void Pontuacao(){
		Application.LoadLevel ("ScoreScreen");
	}

	public void Sair(){
		Application.Quit();
	}

	public void Vai(){
		Application.LoadLevel ("LevelTest");
	}

	public void Voltar(){
		Application.LoadLevel ("HomeScreen");
	}

	public void Pause(){
		Application.LoadLevel ("HomeScreen");
	}
}
