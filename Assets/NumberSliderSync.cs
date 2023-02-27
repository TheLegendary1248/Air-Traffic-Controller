using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// A neat utility script to sync input field with slider
/// </summary>
public class NumberSliderSync : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField input;
    public System.Action<float> onValueChange;
    float value;
    private void Awake()
    {
        slider.onValueChanged.AddListener(SliderValueChanged);
        input.onValueChanged.AddListener(InputValueChanged);
    }
    
    void SliderValueChanged(float val)
    {
        value = val;
        input.text = value.ToString();
        onValueChange?.Invoke(value);
    }
    void InputValueChanged(string str)
    {
        float parsed = float.Parse(str);
        value = parsed;
        slider.value = value;
        onValueChange?.Invoke(value);
    }
}
