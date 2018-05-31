using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMenu : MonoBehaviour {

	// Use this for initialization
	public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
