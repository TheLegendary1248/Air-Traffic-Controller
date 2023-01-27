using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimeKeep : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public float timeStamp = 0f;
    
    public void FixedUpdate()
    {
        float time = (Time.fixedTime - timeStamp);
        uiText.text = $"{(int)(time / 60)} : {((int)(time % 60)).ToString().PadLeft(2, '0')}";
    }
}
