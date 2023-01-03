using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject plane;
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
        timer = StartCoroutine(Wave(3f));
    }
    void SpawnPlane()
    {
        Vector2 v = Random.insideUnitCircle;
        v.Normalize(); v *= 100;
        float angle = Vector2.SignedAngle(Vector2.down, v);
        GameObject gb = Instantiate(plane, new Vector3(v.x,70f + v.y,-4.5f), Quaternion.Euler(0,0,angle));
    }
}
