using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_UILeftControl : MonoBehaviour {
	public static SCR_UILeftControl instance;
	
	public GameObject[] UI_PlayerName;
	
    private void Start() {
        instance = this;
		GetComponent<RectTransform>().anchoredPosition = new Vector2 (-400, 0);
    }
    private void Update() {
        
    }
	
	public void Show() {
		GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
	}
	
	public void Hide() {
		GetComponent<RectTransform>().DOAnchorPosX (-400, 0.5f, true);
	}
	
	public void SetName(int i, string name) {
		UI_PlayerName[i].GetComponent<Text>().text = "P" + (i + 1) + ": " + name;
	}
	public void HideNameOnly() {
		for (int i=0; i<UI_PlayerName.Length; i++) {
			UI_PlayerName[i].GetComponent<RectTransform>().DOAnchorPosX (-400, 0.5f, true);
		}
	}
	public void ShowNameOnly() {
		for (int i=0; i<UI_PlayerName.Length; i++) {
			UI_PlayerName[i].GetComponent<RectTransform>().DOAnchorPosX (10, 0.5f, true);
		}
	}
}
