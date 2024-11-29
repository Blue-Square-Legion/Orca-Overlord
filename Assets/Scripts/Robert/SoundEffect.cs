using UnityEngine;


/// <summary>
/// 
/// </summary>
public class SoundEffect : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip;
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        source.PlayOneShot(clip);
    }
}
