using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject timelineObject;
    [SerializeField] private GameObject pause;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject panel;

    private void Start()
    {
        Time.timeScale = 1;
        timelineObject = GameObject.FindGameObjectWithTag("Timeline");
    }

    public void ChangeScene(string scene)
    {
        timeline.gameObject.SetActive(true);
        StartCoroutine(StartCinematich(scene));
    }

    public void ResumeGame(string scene)
    {
        pause.SetActive(false);
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void QuitGame(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private IEnumerator StartCinematich(string scene)
    {
        while(timeline.state == PlayState.Playing)
        {
            yield return null;
        }
        if(panel)
        panel.SetActive(true);
        SceneManager.LoadScene(scene);
    }
}
