using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class AutoObjectPlacement : MonoBehaviour
{
    
    #if UNITY_EDITOR
    
    // Start is called before the first frame update

    [SerializeField] private string parentName;
    
    [SerializeField] GameObject objectToInstantiate;

    [SerializeField] Transform[] refernces;

    [SerializeField]private bool spawnData;
    
    
    
    void Start()
    {
        
    }


    private void Update()
    {
        SpawnData();
    }

    void SpawnData()
    {
        if(!spawnData)
            return;

        spawnData = false;

        GameObject parent = new GameObject
        {
            name = parentName
        };


        foreach (var t in refernces)
        {
            GameObject g = PrefabUtility.InstantiatePrefab(objectToInstantiate) as GameObject;

            g.transform.position = t.transform.position;

            g.transform.rotation = t.transform.rotation;

            g.transform.localScale = t.transform.lossyScale;

            g.transform.parent = parent.transform;
            
            
            DestroyImmediate(t.gameObject);
            

        }
        
    }
    
    #endif
    
}
