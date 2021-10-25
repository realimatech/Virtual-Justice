using UnityEngine;
using System.Collections;

public class ScriptMenu : MonoBehaviour {
	
	private bool sair = false;
	void OnMouseEnter(){
		GetComponent<Renderer>().material.color = Color.red;	
	}
	
	void OnMouseExit(){
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseDown(){
		if(sair){
			Application.Quit();
		}else{
			Application.LoadLevel(1);
		}
	}
}
