using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_UI : MonoBehaviour {
	public static SCR_UI instance;
	
	public Text TXT_Population;
	public Text TXT_Resource;
	
	
    private void Start() {
        instance = this;
    }

    private void Update() {
        
    }
	
	
	
	public void SetResourceNumber (int population, int resource) {
		TXT_Population.text = "" + population + " M";
		TXT_Resource.text = "" + resource + " T";
	}
}
