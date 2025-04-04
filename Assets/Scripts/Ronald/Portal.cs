using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //For loading next level

public class Portal : MonoBehaviour
{

    public float shrinkDuration = 3.0f; //Time taken to shrink
    public Vector3 shrinkScale = new Vector3(1.0f, 1.0f, 1.0f); //Final size before teleport
    private bool isTeleporting = false; //Prevent multiple triggers

    private Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            isTeleporting = true;
            StartCoroutine(ShrinkAndTeleport(other.gameObject));
        }
    }

    private System.Collections.IEnumerator ShrinkAndTeleport(GameObject player)
    {
        Vector3 originalScale = player.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            player.transform.localScale = Vector3.Lerp(originalScale, shrinkScale, elapsedTime / shrinkDuration);
            yield return null;
        }

        player.transform.localScale = shrinkScale; //Ensure final size is set

        //Load next level
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels to load!");
        }
    }
}
