using UnityEngine;

public class CameraCulling : MonoBehaviour
{
    [SerializeField] private int[] culledDistances;
    [SerializeField] private LayerMask layersForCulling;
    private void Start()
    {
        ApplyCulling();
    }
    private void ApplyCulling()
    {
        var camera = GetComponent<Camera>();
        var distances = new float[32];
        var distanceIndex = 0;
        for (var i = 0; i < 32; i++)
        {
            if (layersForCulling == (layersForCulling | (1 << i)))
            {
                distances[i] = culledDistances[distanceIndex];
                distanceIndex++;
            }
        }
        camera.layerCullDistances = distances;
    }
}
