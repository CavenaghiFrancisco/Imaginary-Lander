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
    [SerializeField] private TMP_Text volumeTxt;
    [SerializeField] private GameObject muteImage;
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject destroyedAudio;
    private bool choosed;

    private void Awake()
    {
        choosed = false;
        if (PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume",PlayerPrefs.GetFloat("Volume"));
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", 0.3f);
        }
        PlayerMovement.OnDamage += BackToMenu;
    }

    private void Start()
    {
        Time.timeScale = 1;
        timelineObject = GameObject.FindGameObjectWithTag("Timeline");
    }

    private void Update()
    {
        if(muteImage && music)
        {
            muteImage.SetActive(PlayerPrefs.GetFloat("Volume") == 0);
            music.volume = PlayerPrefs.GetFloat("Volume");
        }
        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] != "")
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
                volumeTxt.text = "X - VOLUME";
                if (Input.GetButtonDown("Cancel") && !choosed)
                {
                    Application.Quit();
                    choosed = true;
                }
                else if (Input.GetButtonDown("Submit") && !choosed)
                {
                    ChangeScene("SampleScene");
                    choosed = true;
                }
                else if (Input.GetButtonDown("Volume"))
                {
                    MuteAudio();
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
                volumeTxt.text = "VOLUME";
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
        Application.Quit();
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

    public void MuteAudio()
    {
        if(PlayerPrefs.GetFloat("Volume") == 0)
        {
            PlayerPrefs.SetFloat("Volume", 0.3f);
            muteImage.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", 0);
            muteImage.SetActive(false);
        }
    }

    private void BackToMenu()
    {
        timeline.gameObject.SetActive(true);
        //destroyedAudio.SetActive(true);
        StartCoroutine(StartCinematich("Menu"));
    }

    private void OnDestroy()
    {
        PlayerMovement.OnDamage -= BackToMenu;
    }
}
