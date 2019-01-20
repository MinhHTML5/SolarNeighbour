using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class SCR_UIMainInfoPanel : MonoBehaviour {
	public static SCR_UIMainInfoPanel instance;
	
	public GameObject UI_WaitingForPlayers;
	public GameObject UI_ChooseAPlanet;
	
	public void Awake () {
		instance = this;
	}
	
    public void ShowPanel () {
		GetComponent<RectTransform>().DOAnchorPosY (-10, 0.5f, true);
	}
	public void HidePanel () {
		GetComponent<RectTransform>().DOAnchorPosY (200, 0.5f, true);
	}
	
	public void HideAllMessage () {
		UI_WaitingForPlayers.SetActive (false);
		UI_ChooseAPlanet.SetActive (false);
	}
	
	public void ShowWaitingForPlayers () {
		HideAllMessage();
		UI_WaitingForPlayers.SetActive (true);
	}
	public void ShowChooseAPlanet () {
		HideAllMessage();
		UI_ChooseAPlanet.SetActive (true);
	}
}

