using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Cursor : MonoBehaviour
{
    public Canvas parentCanvas;
    public TextMeshProUGUI text;
    //Thanks to https://stackoverflow.com/questions/43802207/position-ui-to-mouse-position-make-tooltip-panel-follow-cursor#answer-43804848
    //For following mouse position as canvas element
    public void Start()
    {
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, Input.mousePosition,
            parentCanvas.worldCamera,
            out pos);
    }
    public void Update()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);
        transform.position = parentCanvas.transform.TransformPoint(movePos);
        //Calculate terrain height position
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(pos);
        text.text = $"X: {pos.x}\nY: {pos.y}\n{ShaderFunctions.ClassicNoise(pos)}";
        
    }
}
