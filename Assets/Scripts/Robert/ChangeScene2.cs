using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene2 : MonoBehaviour
{
    private float timer2 = 30f;
    // Start is called before the first frame update
    void Start()
    {
        timer2 = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer2 < 8)
            SceneManager.LoadScene("Scenes/Robert/BoatFlip");

        timer2 -= 1 * Time.deltaTime;
    }
}
