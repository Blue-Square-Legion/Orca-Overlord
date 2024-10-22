using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Scene currentScene;
    public ChangeScene3 sceneChanger;
    
    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return)){
            if(currentScene.name == "Title"){
                sceneChanger.OpenStory();
            }
            else if(currentScene.name == "Movie"){
                SceneManager.LoadScene("Tutorial");
            }
            else if(currentScene.name == "GameOver"){
                sceneChanger.OnRetry();
            }
            else if(currentScene.name == "End"){
                sceneChanger.OnRetry();
            }
        }
    }
}
