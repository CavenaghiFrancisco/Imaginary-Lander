using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveAndLoad
{
    public static string LoadFromFile(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename);
        string stringData = textAsset.text;
        return stringData;
    }
}
