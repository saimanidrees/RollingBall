using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    Animator anim;
   public FallOnTiles tileHandler;
    bool onceDetected;
    public GameObject bodySlamParticles;
    public GameObject smokeEffect;
    public GameObject smack;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
    }
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tile"&&!onceDetected) {
            anim.SetTrigger("impact");

            bodySlamParticles.SetActive(true);

            tileHandler.StartVibrating();
            onceDetected = true;
            smokeEffect.SetActive(true);
            smack.SetActive(true);
            iTween.ScaleTo(smack, new Vector3(0.2f, 0.26f, 0), 0.5f);
            Invoke("FadeAway",0.5f);
        }
    }

    public void FadeAway() {
        iTween.FadeTo(smack, 0, 0.5f);
    }
    
}
