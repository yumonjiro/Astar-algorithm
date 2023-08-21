using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public Transform raycastStartPoint;
    public int rayNumber;
    public LineRenderer _renderer;
    public Material irMat;
    // public Material lm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float theta = 360/rayNumber;
        for(int i = 0; i < rayNumber; i++)
        {
            float rotate = i*Mathf.PI*2/rayNumber;
            Vector3 rotateVector = new Vector3(Mathf.Cos(rotate), 0, Mathf.Sin(rotate));
            Ray ray = new Ray(raycastStartPoint.position, rotateVector);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                DrawLine(raycastStartPoint.position, hit.point, Color.red);
            }
            else
            {
                DrawLine(raycastStartPoint.position, raycastStartPoint.position + rotateVector*20, Color.black);
            }
            
            
            // Debug.DrawRay(raycastStartPoint.position, rotateVector* 10, Color.red, 1/60f, true);
        }
    }
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = irMat;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, Time.deltaTime+0.05f);
    }
}
