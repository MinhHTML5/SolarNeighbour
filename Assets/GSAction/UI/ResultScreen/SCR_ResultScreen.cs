using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SCR_ResultScreen : MonoBehaviour {
    public static SCR_ResultScreen instance;
	
	public GameObject IMG_Victory;
	public GameObject IMG_Defeat;
	public GameObject BTN_Continue;
	public GameObject BTN_Quit;
	
	private float alpha = 0;
	private bool showing = false;
	
    private void Start() {
        
	}

    private void Update() {
		if (showing == true) {
			alpha += Time.deltaTime * 3;
			if (alpha > 1) alpha = 1;
		}
		else {
			alpha -= Time.deltaTime * 3;
			if (alpha <= 0) {
				gameObject.SetActive (false);
				alpha = 0;
			}
		}
		ApplyAlpha();
    }
	
	public void ShowVictory() {
		showing = true;
		gameObject.SetActive (true);
		IMG_Defeat.SetActive (false);
		IMG_Victory.SetActive (true);
		BTN_Continue.GetComponent<Button>().interactable = false;
		ApplyAlpha();
	}
	
	public void ShowDefeat() {
		showing = true;
		gameObject.SetActive (true);
		IMG_Defeat.SetActive (true);
		IMG_Victory.SetActive (false);
		BTN_Continue.GetComponent<Button>().interactable = true;
		ApplyAlpha();
	}
	
	public void Hide() {
		showing = false;
	}
	
	public void ApplyAlpha() {
		gameObject.GetComponent<CanvasGroup>().alpha = alpha;
	}
	
	public void Quit() {
		SCR_Loading.LoadScene ("GSMenu/SCN_Menu");
	}
}
