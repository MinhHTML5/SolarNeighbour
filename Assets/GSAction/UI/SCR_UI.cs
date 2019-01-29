using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UI : MonoBehaviour {
	public static SCR_UI instance;
	
	public Text TXT_Population;
	public Text TXT_Resource;
	
	public GameObject UI_ResultScreen;
	public GameObject UI_MainMenu;
	
	
    private void Start() {
        instance = this;
		SCR_ResultScreen.instance = UI_ResultScreen.GetComponent<SCR_ResultScreen>();
		SCR_MainMenu.instance = UI_MainMenu.GetComponent<SCR_MainMenu>();
    }

    private void Update() {
        
    }
	
	public void ShowMainMenu() {
		SCR_MainMenu.instance.Show();
	}
	
	public void SetResourceNumber (int population, int resource) {
		TXT_Population.text = "" + population + " M";
		TXT_Resource.text = "" + resource + " T";
	}
	
	
}
