using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Text text;
    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.OnPropulsorUse += UpdateGasoline;
        CratersBase.OnSuccesfulLanding += UpdateScore;
        text = transform.GetChild(4).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateGasoline(float gasoline)
    {
        transform.GetChild(0).GetComponent<Image>().fillAmount = gasoline/100;
    }

    private void UpdateScore()
    {
        score += 3;
        text.text = score.ToString();
    }
}
