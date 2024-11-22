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

    void Start()
    {
        _currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth); // Ensure health doesn't go below 0 or above max
        UpdateHealthBar();
    }

    public void Heal(float healAmount)
    {
        _currentHealth += healAmount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
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
