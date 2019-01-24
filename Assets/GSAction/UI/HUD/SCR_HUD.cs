using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_HUD : MonoBehaviour {
	public static SCR_HUD instance;
	
	public GameObject[]	UI_PlayerIndicator;
	public Image[]		UI_PlayerHP;
	public RectTransform canvasRect;
	
	private bool 	showing = false;
	private float 	alpha = 0;
	
    private void Awake() {
        instance = this;
    }

    private void Update() {
        float dt = Time.deltaTime;
        if (showing == true) {
			alpha += dt * 2;
			if (alpha > 1) alpha = 1;
		}
		else {
			alpha -= dt * 2;
			if (alpha < 0) {
				alpha = 0;
				for (int i=0; i<UI_PlayerIndicator.Length; i++) {
					UI_PlayerIndicator[i].SetActive (false);
				}
			}
		}
		
		for (int i=0; i<UI_PlayerIndicator.Length; i++) {
			UI_PlayerIndicator[i].GetComponent<Image>().color = new Color(1, 1, 1, alpha);
		}
		
		if (alpha > 0) {
			for (int i=0; i<SCR_Action.instance.planets.Length; i++) {
				if (SCR_Action.instance.gameState == GameState.ACTION) {
					if (SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().playerID == -1) {
						
					}
					else {
						Vector2 viewportPos = Camera.main.WorldToViewportPoint (SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().GetPosition());
						Vector2 uiPos = new Vector2 ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f), (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
						uiPos.y += 20;
						UI_PlayerIndicator[SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().playerID].GetComponent<RectTransform>().anchoredPosition = uiPos;
						
						UI_PlayerHP[SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().playerID].fillAmount = 0.15f + SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().GetHPPercentage() * 0.75f;
					}
				}
			}
		}
    }
	
	
	public void Show() {
		showing = true;
		for (int i=0; i<UI_PlayerIndicator.Length; i++) {
			UI_PlayerIndicator[i].SetActive (true);
		}
	}
	public void Hide() {
		showing = false;
	}
}
