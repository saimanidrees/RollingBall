using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GameData.MyScripts;

public class FallOnTiles : MonoBehaviour
{
    public int playerValue;
    int[] valuesArr = { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
    public GameObject[] tiles;
    public GameObject StickFigure;
    public float duration;
    public Material playerMatt;
    public SkinnedMeshRenderer skin;
    private void OnEnable()
    {
        if(playerMatt!=null)
        skin.material = playerMatt;
        if (playerValue >= 2 && playerValue <= 4096)
        {
            StickFigure.GetComponent<TileDetector>().tileHandler = this;
            //BreakOnTile();
        }
        else {
            Debug.LogError("Player Value not instantiated");
        }
    }
    
    GameObject tile;
    Transform stickManPos;
    CinemachineVirtualCamera cam;
    public void BreakOnTile() {
        StickFigure.GetComponent<StickManTextureSetting>().SetTextureOffset(PlayerPrefsHandler.BallNumbers.IndexOf(playerValue));
        StickFigure.SetActive(true);
        int index=0;
        for (int i=0;i< valuesArr.Length;i++) {
            if (valuesArr[i] == playerValue) {
                index = i;
                break;
            }
        }
         tile = tiles[index];
        stickManPos = tile.GetComponent<TileProperties>().playerPosition;
        StickFigure.transform.position = stickManPos.position;
        StickFigure.transform.rotation = stickManPos.rotation;
        cam = tile.GetComponent<TileProperties>().cam;
        if (cam!=null){
            cam.Priority += 3;
        }

    }
    public void StartVibrating() {
        StartCoroutine(Vibrating());
    }
    IEnumerator Vibrating() {
        
        
        float t = 0;
        while (t < 0.8) {
            var speed = 5.0f;
            var intensity = 0.1f;

           tile.GetComponent<TileProperties>().tile.transform.localPosition= intensity * new Vector3(
                    Mathf.PerlinNoise(speed * Time.time, 1),
                    Mathf.PerlinNoise(speed * Time.time, 2),
                 Mathf.PerlinNoise(speed * Time.time, 3));
            t += Time.deltaTime;
            yield return null;
        }
        tile.GetComponent<TileProperties>().tile.transform.localPosition = Vector3.zero;
        yield return null;

    }

    /* var speed = 5.0f;
     var intensity = 0.4f;

     transform.localPosition = intensity* new Vector3(
        Mathf.PerlinNoise(speed* Time.time, 1),
             Mathf.PerlinNoise(speed* Time.time, 2),
             Mathf.PerlinNoise(speed* Time.time, 3));*/
}
