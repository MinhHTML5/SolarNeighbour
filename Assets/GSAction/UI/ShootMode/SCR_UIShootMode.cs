using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class SCR_UIShootMode : MonoBehaviour {
	public static SCR_UIShootMode instance;
	
	public GameObject[]	UI_PlanetIndicator;
	public GameObject 	UI_ShootMaskLeft;
	public GameObject 	UI_ShootMaskRight;
	public GameObject 	UI_AimIndicator;
	public GameObject 	UI_ForceIndicator;
	
	
	public RectTransform canvasRect;
	
	public float	aimAngle = 0;
	public float	aimForce = 0;
	
	private bool 	showing = false;
	private float 	alpha = 0;
	private float 	aimX = 0;
	private float 	aimY = 0;
	private float 	homeX = 0;
	private float 	homeY = 0;
	
	
	
    private void Start() {
        instance = this;
		UI_ShootMaskLeft.GetComponent<RectTransform>().anchoredPosition = new Vector2 (-960, 0);
		UI_ShootMaskRight.GetComponent<RectTransform>().anchoredPosition = new Vector2 (960, 0);
		for (int i=0; i<UI_PlanetIndicator.Length; i++) {
			UI_PlanetIndicator[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
		}
		UI_AimIndicator.GetComponent<Image>().color = new Color(1, 1, 1, 0);
		UI_ForceIndicator.GetComponent<Image>().color = new Color(1, 1, 1, 0);
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
				UI_ForceIndicator.SetActive (false);
				UI_AimIndicator.SetActive (false);
				
				for (int i=0; i<UI_PlanetIndicator.Length; i++) {
					UI_PlanetIndicator[i].SetActive (false);
				}
			}
		}
		
		if (alpha > 0) {
			for (int i=0; i<UI_PlanetIndicator.Length; i++) {
				if (SCR_Action.instance.gameState == GameState.ACTION) {
					if (SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().playerID == -1) {
						UI_PlanetIndicator[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, alpha * 0.5f);
					}
					else if (SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().playerID == SCR_Action.instance.playerID) {
						UI_PlanetIndicator[i].GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.2f, alpha);
					}
					else {
						UI_PlanetIndicator[i].GetComponent<Image>().color = new Color(1.0f, 0.2f, 0.2f, alpha);
					}
					
					Vector2 viewportPos = Camera.main.WorldToViewportPoint (SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().GetPosition());
					Vector2 uiPos = new Vector2 ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f), (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
					
					UI_PlanetIndicator[i].GetComponent<RectTransform>().anchoredPosition = uiPos;
					if (SCR_Action.instance.planets[i].GetComponent<SCR_Planet>().playerID == SCR_Action.instance.playerID) {
						homeX = uiPos.x;
						homeY = uiPos.y;
					}
				}
			}
		
			aimAngle = SCR_Helper.AngleBetweenTwoPoint (homeX, homeY, aimX, aimY);
			
			UI_ForceIndicator.transform.localEulerAngles = new Vector3(0, 0, 90 - aimAngle);
			UI_ForceIndicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(homeX, homeY);
			UI_ForceIndicator.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
			UI_AimIndicator.transform.localEulerAngles = new Vector3(0, 0, 90 - aimAngle);
			UI_AimIndicator.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
			UI_AimIndicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(homeX, homeY);
			
			float distance = SCR_Helper.DistanceBetweenTwoPoint (homeX, homeY, aimX, aimY);
			aimForce = distance / 400;
			if (aimForce > 1) aimForce = 1;
			else if (aimForce < 0.2f) aimForce = 0.2f;
			UI_ForceIndicator.GetComponent<Image>().fillAmount = aimForce;
			UI_AimIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(distance, 4);
		}
		else {
			
		}
    }
	
	public void Show() {
		UI_ShootMaskLeft.GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
		UI_ShootMaskRight.GetComponent<RectTransform>().DOAnchorPosX (0, 0.5f, true);
		showing = true;
		
		UI_ForceIndicator.SetActive (true);
		UI_AimIndicator.SetActive (true);
		
		for (int i=0; i<UI_PlanetIndicator.Length; i++) {
			UI_PlanetIndicator[i].SetActive (true);
		}
	}
	
	public void Hide() {
		UI_ShootMaskLeft.GetComponent<RectTransform>().DOAnchorPosX (-960, 0.5f, true);
		UI_ShootMaskRight.GetComponent<RectTransform>().DOAnchorPosX (960, 0.5f, true);
		showing = false;
	}
	
	public void MouseHover(Vector2 mousePos) {
		Vector2 aimPoint = new Vector2 (0, 0);
		RectTransformUtility.ScreenPointToLocalPointInRectangle (canvasRect, mousePos, null, out aimPoint);

		aimX = aimPoint.x;
		aimY = aimPoint.y;
	}
}