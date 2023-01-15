using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject plane;
    public float spawnTime;
    public Coroutine timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = StartCoroutine(Wave(3f));
    }
    IEnumerator Wave(float time)
    {
        SpawnPlane();
        yield return new WaitForSeconds(time);
        timer = StartCoroutine(Wave(spawnTime));
    }
    void SpawnPlane()
    {
        Vector2 v = World.Main.AlignToWorld(GetSpwnPt());
        float angle = Vector2.SignedAngle(Vector2.down, v);
        GameObject gb = Instantiate(plane, new Vector3(v.x,70f + v.y,-3f), Quaternion.Euler(0,0,angle));
    }
    Vector2 GetSpwnPt()
    {
        Rect border = World.Main.worldBorder;
        int axis = Random.value > 0.5f ? 1 : 0 ; //X or Y side
        int side = Random.value > 0.5f ? 1 : -1; //This or opposing side
        float mul = Random.value * 2f - 1f; //Interpolation
        Vector2 l = Vector2.zero;
        float w = border.width / 2f;
        float h = border.height / 2f;
        l[axis] = side * (axis == 1 ? w : h);
        l[(axis - 1) * -1] = mul * (axis == 1 ? h : w);
        return l;
    }
}
