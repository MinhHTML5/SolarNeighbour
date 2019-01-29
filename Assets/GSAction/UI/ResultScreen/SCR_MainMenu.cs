using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_MainMenu : MonoBehaviour {
	public static SCR_MainMenu instance;
    
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
	
	public void Show() {
		ApplyAlpha();
		gameObject.SetActive (true);
		showing = true;
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
