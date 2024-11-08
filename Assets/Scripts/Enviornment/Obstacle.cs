using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private int damage;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && collider.gameObject.GetComponent<IHealth>() != null)
        {
            collider.gameObject.GetComponent<IHealth>().OnGetDamage(damage);
        }
    }
}
