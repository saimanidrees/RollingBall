using System;
using System.Collections.Generic;
using UnityEngine;
public class StackLayout : MonoBehaviour
{
    [SerializeField] private float xStart, yStart;
    [SerializeField] private int columnLength, rowLength;
    [SerializeField] private List<Layer> layers;
    [SerializeField] private float xSpace, ySpace;
    [Range(0f, 4f)][SerializeField] private float xGap;
    [Range(0f, 4f)][SerializeField] private float yGap;
    [SerializeField] private GameObject prefab;
    public List<GameObject> balls;
    private void Start()
    {
        /*for (var i = 0; i < columnLength * rowLength; i++)
        {
            var ob = Instantiate(prefab, transform);
            ob.transform.localPosition = new Vector3(xStart + (xSpace * (i % columnLength)),
                // ReSharper disable once PossibleLossOfFraction
                yStart + (ySpace * (i / columnLength)), 0f);
            ob.transform.localRotation = Quaternion.identity;
            /*Instantiate(prefab,
                // ReSharper disable once PossibleLossOfFraction
                new Vector3(xStart + (xSpace * (i % columnLength)), yStart + (ySpace * (i / columnLength)), 0f),
                Quaternion.identity);#1#
        }*/
        for (var i = 0; i < layers.Count; i++)
        {
            for (var j = 0; j < layers[i].objectsCount; j++)
            {
                var ob = Instantiate(prefab, transform);
                ob.transform.localPosition = new Vector3(xGap + (xSpace * (i % layers.Count)),
                    yGap + (ySpace * (j % layers.Count)), 0f);
                ob.transform.localRotation = Quaternion.identity;
                balls.Add(ob);
            }
        }
    }

    private void Update()
    {
        for (var i = 0; i < balls.Count; i++)
        {
            balls[i].transform.localPosition = new Vector3(xGap + (xSpace * (i % layers.Count)),
                yGap + (ySpace * (i % layers.Count)), 0f);
            balls[i].transform.localRotation = Quaternion.identity;
        }
    }

    [Serializable]
    public class Layer
    {
        public int layerIndex;
        public int objectsCount;
    }
}