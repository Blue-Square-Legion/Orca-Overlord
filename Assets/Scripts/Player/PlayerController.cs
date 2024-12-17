using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private bool canDie;
   
   
   //Basic Movement
   private CharacterController _controller;
   private Vector3 _playerVelocity;
   private bool _isInWater;
   private bool _isAtSurface;
   private float _speed;
   
   public bool IsInWater => _isInWater;
   public bool IsAtSurface => _isAtSurface;
   
   
   //Health
   private Health _health;
   
   public Health Health => _health;
   
   
   //Combat
   [SerializeField] private int damage = 10;
   private GameObject _currentEnemy;
   private Boat _boat;
   [SerializeField] private float _knockBackPower = 10.0f;
   public int Damage => damage;
   

   //Knockback
   [SerializeField] private GameObject damageEffect;
   private Vector3 _velocity;
   private bool _isKnockedBack;
   private float _knockBackDuration = 0.2f;
   private float _knockBackTimer;
   
   [SerializeField] private float playerSpeed = .5f;
   [SerializeField] private float boostMultiplier = 1.5f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityBoost = 1.0f;
   
   
   //Animation
   private Animator _animator;
   
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
      
      if (!TryGetComponent(out _animator))
      {
         Debug.LogError("Animator Component is Missing.");
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

      if (other.CompareTag("Enemy"))
      {
         _currentEnemy = other.gameObject;
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
      
      if (other.CompareTag("Enemy"))
      {
         _currentEnemy = null;
      }
   }

   void Update()
   {
      if (_health.IsDead && canDie)
      {
         return;
      }
      
      if (_isKnockedBack) 
      {
         _knockBackTimer -= Time.deltaTime;
         if (_knockBackTimer <= 0) 
         {
            damageEffect.SetActive(false);
            
            _isKnockedBack = false;
            _velocity = Vector3.zero;
         }
      }
      
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
      
         _controller.Move(move);
      }
      
      _controller.Move(_velocity * Time.deltaTime);

      if (_currentEnemy && Input.GetMouseButtonDown(0))
      {
         _animator.SetTrigger("Attack");
      }
      
   }

   public Vector3 Move()
   {
      return transform.right * Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime +
             transform.forward * Input.GetAxisRaw("Vertical") * _speed * Time.deltaTime;
   }
   
   
   public void ApplyKnockBack(Vector3 direction, float attackPower, int damage) 
   {
      _isKnockedBack = true;
      _knockBackTimer = _knockBackDuration;
      _velocity = direction * attackPower;
      
      damageEffect.SetActive(true);
      
      _health.TakeDamage(damage);
   }
   
   public void PerformAttack() 
   {
      if (_currentEnemy)
      {
         Vector3 knockbackDirection = (_currentEnemy.transform.position - transform.position).normalized;
         //knockbackDirection.y = 0;

         _currentEnemy.GetComponent<Boat>()?.ApplyKnockBack(knockbackDirection, _knockBackPower, damage);
      }
   }
}
