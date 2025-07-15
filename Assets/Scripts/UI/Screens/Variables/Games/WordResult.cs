using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordResult : MonoBehaviour
{
    public TMP_Text wordText;
    public Image icon;
    public Sprite correct;
    public Sprite wrong;
    public void SetResult(bool result, string word)
    {
        wordText.text = word;
        if(result)
        {
            icon.sprite = correct;

            Color color = icon.color;
            color.a = Mathf.Clamp01(1);
            icon.color = color;
        }
        else
        {
            icon.sprite = wrong;

            Color color = icon.color;
            color.a = Mathf.Clamp01(0.5f);
            icon.color = color;
        }
    }
}
