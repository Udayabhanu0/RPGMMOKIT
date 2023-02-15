using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTargetFollow : MonoBehaviour
{
    public GameObject[] target;
    public Vector3 Offset;
    private Vector3 velocity;
    public float smoothTime = .5f;
    public float maxZoom = 40f;
    public float minZoom = 80f;
    public float ZoomLimiter = 80f;
    private Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        target = GameObject.FindGameObjectsWithTag("Player");
        if (target.Length == 0)
        {
            return;
        }
        move();
        Zoom();

    }
    void move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

    }
    void Zoom(){
     float newZoom = Mathf.Lerp(maxZoom,minZoom,GetGreatestDistance()/ZoomLimiter);
     cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,newZoom,Time.deltaTime);

    }
    float GetGreatestDistance(){
        var bounds = new Bounds(target[0].transform.position, Vector3.zero);
        for (int i = 0; i < target.Length; i++)
        {
            bounds.Encapsulate(target[i].transform.position);
        }
        return bounds.size.x;
    }
    Vector3 GetCenterPoint()
    {
        if (target.Length == 1)
        {
            return target[0].transform.position;

        }
        var bounds = new Bounds(target[0].transform.position, Vector3.zero);
        for (int i = 0; i < target.Length; i++)
        {
            bounds.Encapsulate(target[i].transform.position);
        }
        return bounds.center;
    }
}
