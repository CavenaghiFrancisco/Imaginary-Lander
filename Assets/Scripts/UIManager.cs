using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image fullGasolineImage;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private AudioSource[] audios;
    [SerializeField] private PlayableDirector[] playableDirectors;
    private float offset = 0.3f;
    private bool alive = true;


    void Start()
    {
        PlayerMovement.OnPropulsorUse += UpdateGasoline;
        PlayerMovement.OnDamage += UpdateAlive;
    }

    private void Update()
    {
        fullGasolineImage.material.SetTextureOffset("_MainTex", new Vector2(offset += 0.1f * Time.deltaTime, 0));
        if (alive)
        {
            if (!pausePanel.activeSelf)
            {
                foreach (PlayableDirector playableDirector in playableDirectors)
                {
                    if (playableDirector.state == PlayState.Playing && Input.GetButton("Submit"))
                    {
                        Time.timeScale = 10;
                        AudioListener.volume = 0;
                        break;
                    }
                    else
                    {
                        Time.timeScale = 1;
                        AudioListener.volume = 1;
                    }
                }
            }
            if (Input.GetButtonDown("Start")|| Input.GetKeyDown(KeyCode.Escape))
            {
                pausePanel.SetActive(!pausePanel.activeSelf);
                foreach (AudioSource audio in audios)
                {
                    if (pausePanel.activeSelf)
                    {
                        audio.Pause();
                    }
                    else
                    {
                        audio.Play();
                    }
                        
                }
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }
            
            

        }
    }

    private void UpdateGasoline(float gasoline)
    {
        fullGasolineImage.fillAmount = gasoline/100;
    }

    private void UpdateAlive()
    {
        alive = false;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnPropulsorUse -= UpdateGasoline;
        PlayerMovement.OnDamage -= UpdateAlive;
    }
}
