using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PowerTile : MonoBehaviour
{

    public float powerLevel;
    public GameObject Shrads;
    public GameObject tempGlass;
    public StickManController level;
    public TextMeshPro numberTxt;
    private void Start()
    {
        numberTxt.text = powerLevel.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
       /* if (other.gameObject.tag == PlayerPrefsHandler.Player) {
            if (level.playerLevel >= powerLevel)
            {
               
            }
            else {

                StartCoroutine(Explode());
            }
        
        }*/
    }
    public void shatterGlass() {
        Shrads.SetActive(true);
        tempGlass.SetActive(false);
        /*GlassPiece[] pieces = Shrads.GetComponentsInChildren<GlassPiece>();
        foreach (GlassPiece piece in pieces)
        {
           
            piece.gameObject.GetComponent<Rigidbody>().useGravity = true;

        }*/
    }
    IEnumerator Explode() {
        float t = 0;
        Debug.Log("explode");
        while (t < 2)
        {
            level.mesh1.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, t / 2));
            t += Time.deltaTime;
            yield return null;

        }
        Destroy(level.StickManOpen);

    }
}
