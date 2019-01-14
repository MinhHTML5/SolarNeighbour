using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCR_Loading : MonoBehaviour {
	// ======================================================================
	// CONST
	
	// ======================================================================
	// PREFAB
	
	// ======================================================================
	// PUBLIC
	public static bool	firstTimeRun = true;
	// ======================================================================
	// PRIVATE
	private static System.String nextScene = "";
	// ======================================================================
	// INSTANCE
	public static SCR_Loading instance = null;
	// ======================================================================
	
	
	// =====================================================
	// MY STUFF, DON'T TOUCH THIS
	// =====================================================
	private void Start () {
		// Setting
		Application.targetFrameRate = 60;

		instance = this;
		
		firstTimeRun = false;
	}
	
	private void Update () {
		if (nextScene != "") {
			instance.StartCoroutine(LoadSceneAsync(nextScene));
			nextScene = "";
		}
	}
	
	private static IEnumerator LoadSceneAsync(System.String path) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(path);
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
	// =====================================================
	
	
	
	// =====================================================
	// THIS IS ALL YOU HAVE TO DO TO USE THIS
	// =====================================================
	public static void LoadScene (System.String path) {
		nextScene = path;
		SceneManager.LoadScene("SCN_Loading");
	}
	// =====================================================
	
}
