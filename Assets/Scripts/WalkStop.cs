using UnityEngine;

public class WalkStop : MonoBehaviour
{
    private Animator animator;
    bool IsWalking;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator != null)
        {
           
                IsWalking = true;
        }

    }
}