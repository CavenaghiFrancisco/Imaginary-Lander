using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject pause;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text menuTxt;
    [SerializeField] private TMP_Text resumeTxt;
    [SerializeField] private TMP_Text subtitleTxt;
    [SerializeField] private TMP_Text quitTxt;
    [SerializeField] private TMP_Text playTxt;
    [SerializeField] private TMP_Text volumeTxt;
    [SerializeField] private GameObject muteImage;
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject destroyedAudio;
    [SerializeField] private AudioSource[] audios;
    private bool choosed;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Language"))
        {
            PlayerPrefs.SetString("Language", "ENG");
        }
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
    }

    private void Update()
    {
        if (muteImage && music)
        {
            muteImage.SetActive(PlayerPrefs.GetFloat("Volume") == 0);
            music.volume = PlayerPrefs.GetFloat("Volume");
        }
        if (Input.GetJoystickNames().Length > 0 && JoystickConnected(Input.GetJoystickNames()))
        {
            if (menuTxt && resumeTxt)
            {
                menuTxt.text = "B - MENU";
                resumeTxt.text = "A - RESUME";
                if (pause.activeSelf)
                {
                    if (Input.GetButtonDown("Cancel"))
                    {
                        ChangeScene("Menu");
                    }
                    else if (Input.GetButtonDown("Submit"))
                    {
                        ResumeGame();
                        foreach (AudioSource audio in audios)
                        {
                            audio.Play();
                        }
                    }
                }
            }
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                if (playTxt && quitTxt && volumeTxt && subtitleTxt)
                {
                    quitTxt.text = "B - QUIT";
                    playTxt.text = "A - PLAY";
                    volumeTxt.text = "X - VOLUME";
                    subtitleTxt.text = "Y - SUBTITLE: \n" + PlayerPrefs.GetString("Language", "ENG");
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
                    else if (Input.GetButtonDown("Language"))
                    {
                        ChangeLanguage();
                    }
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
                subtitleTxt.text = "SUBTITLE: \n" + PlayerPrefs.GetString("Language", "ENG");
            }
        }
    }

    private bool JoystickConnected(string[] joystciks)
    {
        foreach(string joystick in joystciks)
        {
            if(joystick != "")
            {
                return true;
            }
        }
        return false;
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
        if (timeline.state != PlayState.Playing)
        {
            if (PlayerPrefs.GetFloat("Volume") == 0)
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
    }

    public void ChangeLanguage()
    {
        if (timeline.state != PlayState.Playing)
        {
            if (PlayerPrefs.GetString("Language") == "ENG")
            {
                PlayerPrefs.SetString("Language", "ESP");
            }
            else
            {
                PlayerPrefs.SetString("Language", "ENG");
            }
        }
    }

    private void BackToMenu()
    {
        timeline.gameObject.SetActive(true);
        destroyedAudio.SetActive(true);
        StartCoroutine(StartCinematich("Menu"));
    }

    private void OnDestroy()
    {
        PlayerMovement.OnDamage -= BackToMenu;
    }
}
