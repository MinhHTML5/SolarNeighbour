using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class SCR_UIRightControl : MonoBehaviour {
	public static SCR_UIRightControl instance;
	
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
}
