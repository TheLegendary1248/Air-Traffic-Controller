using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NumberSliderSync : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField input;
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
    }
    void InputValueChanged(string str)
    {
        float parsed = float.Parse(str);
        value = parsed;
        slider.value = value;
    }
}
