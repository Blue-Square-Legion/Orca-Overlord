using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehaviour : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player") && col.gameObject.GetComponent<IHealth>() != null)
        {
            gameObject.GetComponent<IHealth>().OnGetDamage(damage);
        }
        if (col.CompareTag("Water"))
        {
            StartCoroutine(StartDestroy());
        }
    }

    IEnumerator StartDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
