using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TMP_Text menuTxt;
    [SerializeField] private TMP_Text resumeTxt;
    [SerializeField] private TMP_Text quitTxt;
    [SerializeField] private TMP_Text playTxt;

    private void Start()
    {
        Time.timeScale = 1;
        timelineObject = GameObject.FindGameObjectWithTag("Timeline");
    }

    private void Update()
    {
        if (Input.GetJoystickNames()[0] != "")
        {
            if (menuTxt && resumeTxt)
            {
                menuTxt.text = "B - MENU";
                resumeTxt.text = "A - RESUME";
                if (pause)
                {
                    if (Input.GetButtonDown("Cancel"))
                    {
                        ChangeScene("Menu");
                    }
                    else if (Input.GetButtonDown("Submit"))
                    {
                        ResumeGame();
                    }
                }
            }
            if (playTxt && quitTxt)
            {
                quitTxt.text = "B - QUIT";
                playTxt.text = "A - PLAY";
                if (Input.GetButtonDown("Cancel"))
                {
                    Application.Quit();
                }
                else if (Input.GetButtonDown("Submit"))
                {
                    ChangeScene("SampleScene");
                }
            }
        }
        else
        {
            if (menuTxt && resumeTxt)
            {
                menuTxt.text = "MENU";
                resumeTxt.text = "RESUME";
            }
            if (playTxt && quitTxt)
            {
                quitTxt.text = "QUIT";
                playTxt.text = " PLAY";
            }
        }
    }

    public void ChangeScene(string scene)
    {
        timeline.gameObject.SetActive(true);
        StartCoroutine(StartCinematich(scene));
    }

    public void ResumeGame()
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
