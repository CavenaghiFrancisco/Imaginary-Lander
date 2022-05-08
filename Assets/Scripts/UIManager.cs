using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image fullGasolineImage;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    private float offset = 0.3f;


    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.OnPropulsorUse += UpdateGasoline;
        PlayerMovement.OnDamage += ShowLoseScreen;
        Base.OnSuccesfulLanding += UpdateScore;
    }

    private void Update()
    {
        fullGasolineImage.material.SetTextureOffset("_MainTex", new Vector2(offset += 0.1f * Time.deltaTime, 0));
    }

    private void UpdateGasoline(float gasoline)
    {
        fullGasolineImage.fillAmount = gasoline/100;
    }

    private void UpdateScore()
    {
        
    }

    private void ShowLoseScreen()
    {
        
    }

    private void CheckWinCondition()
    {
            
    }

    public void ChangeScene(int scene)
    {
        PlayerMovement.OnPropulsorUse -= UpdateGasoline;
        PlayerMovement.OnDamage -= ShowLoseScreen;
        Base.OnSuccesfulLanding -= UpdateScore;
        SceneManager.LoadScene(scene);
    }
}
