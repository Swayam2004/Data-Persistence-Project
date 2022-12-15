using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveAndGetData : MonoBehaviour
{
    public static SaveAndGetData Instance;

    [SerializeField] TMP_InputField input;

    public string PlayerName { get; private set; }
    public string PreviewName { get; private set; }

    string data;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveData()
    {
        data = input.text;
    }

    public void ShowData()
    {
        PreviewName = data;
    }
}
