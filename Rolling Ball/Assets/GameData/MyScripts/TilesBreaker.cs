using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesBreaker : MonoBehaviour
{
    public int playerLevel;    
    public StickManController stickMan;
    public GameObject confetti;
    // Start is called before the first frame update

    private void Start()
    {
       
        if (playerLevel >= 2048) {
            stickMan.win = true;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("files");
        if (collision.gameObject.TryGetComponent<PowerTile>(out PowerTile tile))
        {
            if (tile.powerLevel <= stickMan.playerLevel)
            {
                tile.shatterGlass();
                
            }
            else {
                stickMan.stopApplyingForce = true;
                stickMan.PerformLastStunt();               
                GetComponent<Rigidbody>().isKinematic = true;
                Invoke(nameof(PlayConfetii),1f);
            }
        }
    }
    public void PlayConfetii() {
        confetti.SetActive(true);
    }
}