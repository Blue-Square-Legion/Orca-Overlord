using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    private static PlayerController _playerController;
    
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        _playerController = GetComponent<PlayerController>();
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
