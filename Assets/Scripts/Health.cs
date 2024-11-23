using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages Health and Damage Logic for Characters.
/// </summary>

public class Health : MonoBehaviour
{
    
    [SerializeField] private Slider healthBar; // Reference to the Slider
    [SerializeField] private float maxHealth = 100f;
    
    private float _currentHealth;
    private bool _isDead;

    public bool IsDead => _isDead;
    
    void Start()
    {
        _currentHealth = maxHealth;
        _isDead = false;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0.0f, maxHealth); // Ensure health doesn't go below 0 or above max
        UpdateHealthBar();

        _isDead = _currentHealth == 0.0f;
    }

    public void Heal(float healAmount)
    {
        _currentHealth += healAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0.0f, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar)
        {
            healthBar.value = _currentHealth / maxHealth; // Normalize between 0 and 1
        }
    }
}
