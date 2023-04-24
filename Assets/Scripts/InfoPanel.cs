using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviourSingleton<InfoPanel>
{
    [SerializeField] TextMeshProUGUI _TextInfo;


    public void Open(string data)
    {
        _TextInfo.text = data;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
