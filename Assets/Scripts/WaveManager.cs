using UnityEngine;
public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public float amplitude = .3f;
    public float length = .4f;
    public float speed = .2f;
    public float offset = 0f;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    void Update()
    {
        offset += Time.deltaTime * speed;
    }
    public float GetWaveHeight(float _x)
    {
        return (amplitude * Mathf.Sin(_x / length + offset))/2;
    }
}