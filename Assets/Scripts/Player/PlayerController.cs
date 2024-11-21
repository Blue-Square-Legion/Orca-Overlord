using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   //Basic Movement
   private CharacterController _controller;
   private Vector3 _playerVelocity;
   private bool _isInWater;
   private bool _isAtSurface;
   
   public bool IsInWater => _isInWater;
   public bool IsAtSurface => _isAtSurface;
   
   [SerializeField] private float playerSpeed = .5f;
   [SerializeField] private float boostMultiplier = 1.5f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityBoost = 1.0f;

   private float _speed;
   private void Start()
   {
      if (!TryGetComponent<CharacterController>(out _controller))
      {
         Debug.LogError("Character Controller Component is Missing.");
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Water"))
      {
         _isInWater = true;
      }
      
      if (other.CompareTag("WaterSurface"))
      {
         _isAtSurface = true;
      }
   }

   private void OnTriggerExit(Collider other)
   { 
      if (other.CompareTag("Water"))
      {
         _isInWater = false;
      }

      if (other.CompareTag("WaterSurface"))
      {
         _isAtSurface = false;
      }
   }

   void Update()
   {
      if (_isInWater)
      {
         _speed = playerSpeed;
      
         if (Input.GetKey(KeyCode.LeftShift))
         {
            _speed = playerSpeed * boostMultiplier;
         }
         _controller.Move(Move());   
      }
      else
      {
         Vector3 move = Move();

         move.y += -9.18f * gravityBoost * Time.deltaTime;
         
         transform.Translate(move);
         
      }
   }

   public Vector3 Move()
   {
      return transform.right * Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime +
             transform.forward * Input.GetAxisRaw("Vertical") * _speed * Time.deltaTime;
   }
}

      
