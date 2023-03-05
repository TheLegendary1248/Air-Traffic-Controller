using UnityEngine;
using TMPro;
///<summary>
///A quick script I wrote for debugging. Will be used in game at some point, give me a moment 
/// </summary>
public class Cursor : MonoBehaviour
{
    public Canvas parentCanvas;
    public TextMeshProUGUI text;
    public Gradient textColoring;
    public float terraMax;
    public float terraMin;
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
        Vector2 pos = _.WorldPoint();
        float noiseValAtPoint = 0f;
        World.Main.GetTerrainHeight(pos, out noiseValAtPoint);
        //Display text
        text.text = 
            $"X: {string.Format("{0:0.00}",pos.x)}\n" +
            $"Z: {string.Format("{0:0.00}",pos.y)}\n" +
            string.Format("{0:0.00}",noiseValAtPoint);
        text.color = textColoring.Evaluate(Mathf.LerpUnclamped(0f, 0.5f, noiseValAtPoint));
    }
}
