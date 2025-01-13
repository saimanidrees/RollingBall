using GameData.MyScripts;
using UnityEngine;
public class TriggerToNextPatch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerPrefsHandler.Player)) {
            InfinityManager.instance.SetPatchesCounter();
            InfinityManager.instance.InstantiateNewPatch();
            //InfinityManager.instance.GeneratePatch();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerPrefsHandler.Player))
        {
            Invoke(nameof(DestroyThisPatch),4f);
        }
    }
    public void DestroyThisPatch()
    {
        var patch = gameObject.GetComponentInParent<PatchHandler>();
        if (patch != null) {
            Destroy(patch.gameObject);
            //patch.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Starting Patch");
        }

    }
}