using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_UITimer : MonoBehaviour {
    public static SCR_UITimer instance;
	
	public Text UI_TimerText;
	
	public float time;
	public bool enable;
	
	public void Awake () {
		instance = this;
		time = 0;
		enable = false;
	}
	
	public void Show () {
		GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
	}
	public void Hide () {
		GetComponent<RectTransform>().DOAnchorPosX (-250, 0.5f, true);
	}
	
	public void SetTime (float t) {
		time = t;
	}
	public void Start () {
		enable = true;
	}
	public void Pause () {
		enable = false;
	}

	private void Update() {
        if (enable) {
			time -= Time.deltaTime;
			if (time < 0) time = 0;
		}
		UI_TimerText.text = "" + Mathf.RoundToInt(time);
    }
}

