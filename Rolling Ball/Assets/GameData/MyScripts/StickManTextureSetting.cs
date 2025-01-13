using UnityEngine;
public class StickManTextureSetting : MonoBehaviour
{
    [SerializeField] private bool setTextureOffset = false;
    [SerializeField] private int index = 0;
    [SerializeField] private SkinnedMeshRenderer body, head; 
    [SerializeField] private Vector2[] textureOffsets;
    private void Start()
    {
        if(setTextureOffset)
            SetTextureOffset();
    }
    private void SetTextureOffset()
    {
        body.material.SetTextureOffset("_MainTex", textureOffsets[index]);
        head.material.SetTextureOffset("_MainTex", textureOffsets[index]);
    }
    public void SetTextureOffset(int index)
    {
        body.material.SetTextureOffset("_MainTex", textureOffsets[index]);
        head.material.SetTextureOffset("_MainTex", textureOffsets[index]);
    }
}