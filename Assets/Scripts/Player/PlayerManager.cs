using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    private static PlayerController _playerController;

    private Health _health;
    
    public PlayerController PlayerController => _playerController;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        if (!TryGetComponent(out _health))
        {
            Debug.LogError("Health component not found!");
        }
        
        if (!TryGetComponent(out _playerController))
        {
            Debug.LogError("Character Controller not found!");
        }
    }

    public static Vector3 GetPlayerPosition()
    {
        return _playerController.transform.position;
    }

    public static bool IsPlayerInWater()
    {
        return _playerController.IsInWater;
    }

    public static bool IsPlayerAtSurface()
    {
        return _playerController.IsAtSurface;
    }
}
