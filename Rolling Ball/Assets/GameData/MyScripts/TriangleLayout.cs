using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleLayout : MonoBehaviour
{
    /*public List<Transform> children;

    public float radius = 5f;
    public float increment = 30f;

    void Start() {
        //children = new List<Transform>();
    }

    void Update() {

        if(children.Count > 0) {
            PositionChild(children[0], 0); 
        }

        for(int i = 1; i < children.Count; i++) {
            float angle = i * increment;  
            PositionChild(children[i], angle);
        }
    }

    void PositionChild(Transform child, float angle) {

        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        child.position = new Vector3(x, 0, z);
    }

    public void AddChild(Transform child) {
        children.Add(child);
    }*/
    public List<GameObject> objects;

    public float baseWidth;
    public float heightIncrement;

    float baseHeight;

    void Start() {
        //objects = new List<GameObject>();
        baseHeight = baseWidth / 2f * Mathf.Sqrt(3);
    }

    void Update() {

        float xOffset = 0;
        float zOffset = 0;
        float yPos = 0;

        foreach(GameObject obj in objects) {

            obj.transform.position = new Vector3(
                xOffset, 
                yPos,
                zOffset
            );

            yPos += heightIncrement;

            if(objects.IndexOf(obj) % 2 == 0) {
                xOffset += baseWidth / 2f;  
            } else {
                xOffset -= baseWidth / 2f;
                zOffset += baseHeight;
            }
        }
    }

    public void AddObject(GameObject obj) {
        objects.Add(obj);
    }
}
