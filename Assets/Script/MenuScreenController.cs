using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenController : MonoBehaviour {

	public void StartGame()
    {
        SceneManager.LoadScene("ChooseSubject");
        GameObject[] objs = GameObject.FindGameObjectsWithTag("level");

        if (objs.Length > 0)
        {
            Destroy(objs[0]);
        }
    }
}
