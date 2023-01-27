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
        float height = 0f;
        //Check terrain at spot
        for (int i = 0; i < 10; i++)
        {
            World.Main.GetTerrainHeight((Vector3)v + new Vector3(0, 0, -10f), out height);
            //Land is low enough that we can assume that it's mostly free of obstruction
            if (height < 1) break;
        }
        //Test spot
        float angle = Vector2.SignedAngle(Vector2.down, v);
        GameObject gb = Instantiate(plane, new Vector3(v.x,70f + v.y,-3f), Quaternion.Euler(0,0,angle));
    }
    //Extract this as an extension method to Rects later
    Vector2 GetSpwnPt()
    {
        //TODO Center on rect
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
