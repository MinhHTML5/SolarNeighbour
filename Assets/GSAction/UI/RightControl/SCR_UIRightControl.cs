using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_UIRightControl : MonoBehaviour {
	public static SCR_UIRightControl instance;
	
	public GameObject BTN_Shoot;
	public GameObject PNL_ShootButtonMask;
	
    private void Start() {
        instance = this;
		GetComponent<RectTransform>().anchoredPosition = new Vector2 (400, 0);
    }
    private void Update() {
        
    }
	
	public void Show() {
		GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
	}
	
	public void Hide() {
		GetComponent<RectTransform>().DOAnchorPosX (400, 0.5f, true);
	}
	
	public void SetShootCooldown(float cd) {
		if (cd > 0) {
			BTN_Shoot.GetComponent<Button>().interactable = false;
		}
		else {
			BTN_Shoot.GetComponent<Button>().interactable = true;
		}
		PNL_ShootButtonMask.GetComponent<RectTransform>().sizeDelta = new Vector2 (155, cd * 155);
	}
}
