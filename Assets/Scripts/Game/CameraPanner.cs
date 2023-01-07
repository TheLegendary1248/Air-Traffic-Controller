using UnityEngine;

public class CameraPanner : MonoBehaviour
{
    public Vector2 focus = Vector2.zero;
    public float speed = 100f;
    public float lerp = 0.02f;
    public float zoom = 10f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) //Right Click
        {
            focus = _.WorldPoint();
        }
        focus += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * speed;
        zoom -= (Input.mouseScrollDelta.y * zoom) / 10;
        
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, focus, lerp);
        Camera.main.orthographicSize =  Mathf.Lerp(Camera.main.orthographicSize, zoom, lerp);
    }
}
