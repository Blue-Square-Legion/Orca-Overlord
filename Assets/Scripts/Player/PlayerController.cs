using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   //Basic Movement
   private CharacterController _controller;
   private Vector3 _playerVelocity;
   private bool _isInWater;
   private bool _isAtSurface;
   private float _speed;
   
   
   //Health
   private Health _health;
   
   //Knockback
   private Vector3 _velocity;
   private bool _isKnockedBack;
   private float _knockbackDuration = 0.5f;
   private float _knockbackTimer;
   
   [SerializeField] private float playerSpeed = .5f;
   [SerializeField] private float boostMultiplier = 1.5f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityBoost = 1.0f;
   
   public CharacterController Controller => _controller;
   public bool IsInWater => _isInWater;
   public bool IsAtSurface => _isAtSurface;

   private void Awake()
   {
      if (!TryGetComponent(out _health))
      {
         Debug.LogError("Health component not found!");
      }
      
      if (!TryGetComponent(out _controller))
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
      if (_isKnockedBack) {
         _knockbackTimer -= Time.deltaTime;
         if (_knockbackTimer <= 0) {
            _isKnockedBack = false;
            _velocity = Vector3.zero; // Stop knockback
         }
      }
      else
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
      _controller.Move(_velocity * Time.deltaTime);
   }

   public Vector3 Move()
   {
      return transform.right * Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime +
             transform.forward * Input.GetAxisRaw("Vertical") * _speed * Time.deltaTime;
   }
   
   
   public void ApplyKnockback(Vector3 direction, float attackPower, float damage) {
      _isKnockedBack = true;
      _knockbackTimer = _knockbackDuration;
      _velocity = direction * attackPower; // Set knockback velocity
      
      _health.TakeDamage(damage);
   }
}

      
