using UnityEngine;


/// <summary>
/// Add all the game objects you want to assign the don't destroy on load property to.
/// </summary>

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToNotDestroy;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        foreach (GameObject go in gameObjectsToNotDestroy)
        {
            DontDestroyOnLoad(go);
        }
    }
}
