using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenController : MonoBehaviour {

    public AudioClip clickSound;
    public AudioClip openingSound;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        source.clip = openingSound;
        source.Play();
    }

    public void LoadGame()
    {
        source.PlayOneShot(clickSound, 1f);
        StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ChooseSubject");
        GameObject[] objs = GameObject.FindGameObjectsWithTag("level");

        if (objs.Length > 0)
        {
            Destroy(objs[0]);
        }
    }

    public void QuitGame()
    {
        source.PlayOneShot(clickSound, 1f);
        Application.Quit();
    }
}
