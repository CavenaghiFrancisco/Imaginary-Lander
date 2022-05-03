using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Text txtMissions;
    private Text text;
    private int score = 0;
    private float offset = 0.3f;
    private int groupsSaved = 0;
    [SerializeField] private Material UIMaterial;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.OnPropulsorUse += UpdateGasoline;
        PlayerMovement.OnDamage += ShowLoseScreen;
        CratersBase.OnSuccesfulLanding += UpdateScore;
        MainBase.OnCheckWinCondition += CheckWinCondition;
        text = transform.GetChild(4).GetComponent<Text>();
        txtMissions = transform.GetChild(5).transform.GetChild(0).gameObject.GetComponent<Text>();
        transform.GetChild(0).GetComponent<Image>().material = UIMaterial;
    }

    private void Update()
    {
        transform.GetChild(0).GetComponent<Image>().material.SetTextureOffset("_MainTex", new Vector2(offset += 0.1f * Time.deltaTime, 0));
    }

    private void UpdateGasoline(float gasoline)
    {
        transform.GetChild(0).GetComponent<Image>().fillAmount = gasoline/100;
    }

    private void UpdateScore()
    {
        score += 3;
        groupsSaved++;
        text.text = score.ToString();
        txtMissions.text = "Rescue the groups\n of astronauts\n" + groupsSaved + " / 4";
        if(groupsSaved == 4)
        {
            txtMissions.color = Color.green;
        }
    }

    private void ShowLoseScreen()
    {
        transform.GetChild(6).gameObject.SetActive(true);
    }

    private void CheckWinCondition()
    {
        if(score >=12 || groupsSaved >= 4)
        {
            transform.GetChild(7).gameObject.SetActive(true);
        }
    }

    public void ChangeScene(int scene)
    {
        PlayerMovement.OnPropulsorUse -= UpdateGasoline;
        PlayerMovement.OnDamage -= ShowLoseScreen;
        CratersBase.OnSuccesfulLanding -= UpdateScore;
        SceneManager.LoadScene(scene);
    }
}
