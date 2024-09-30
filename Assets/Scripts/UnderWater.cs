
using UnityEngine.Rendering;
using UnityEngine;

public class UnderWater : MonoBehaviour
{
    [SerializeField] int depth = -15;
    [SerializeField] Transform mainCamera;
    [SerializeField] Volume postProcessingVolume;
    [SerializeField] VolumeProfile surfacePostProcessing;
    [SerializeField] VolumeProfile underwaterPostProcessing;

    void Update()
    {
        if (mainCamera.position.y < depth)
        {
            EnableEffects(true);
        }
        else
        {
            EnableEffects(false);
        }
    }

    void EnableEffects(bool active)
    {
        if (active)
        {
            postProcessingVolume.profile = underwaterPostProcessing;
        }
        else
        {
            postProcessingVolume.profile = surfacePostProcessing;
        }
    }







}