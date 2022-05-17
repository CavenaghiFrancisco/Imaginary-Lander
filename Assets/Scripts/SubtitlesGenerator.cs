using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitlesGenerator : MonoBehaviour
{
    private const string subtitleESPath = "Subtitles/ESP";
    private const string subtitleENGPath = "Subtitles/ENG"; 
    [SerializeField] private TMPro.TextMeshProUGUI subtitleText;
    private int currentSubtitle;
    private string language;
    private string subtitles;
    private string[] subtitlesSplitted;

    // Start is called before the first frame update
    void Awake()
    {
        currentSubtitle = -1;
        language = PlayerPrefs.GetString("Language", "ENG");
        if (language == "ESP")
        {
            subtitles = DataSaveAndLoad.LoadFromFile(subtitleESPath);
        }
        else
        {
            subtitles = DataSaveAndLoad.LoadFromFile(subtitleENGPath);
        }
        subtitlesSplitted = subtitles.Split('\n');

    }

    private void OnEnable()
    {
        Debug.Log("Hola");
        currentSubtitle++;
        subtitleText.text = subtitlesSplitted[currentSubtitle];
    }

    

}
