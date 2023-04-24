using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene: MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "Scene";
    static int count = 1; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        count++;
        SceneManager.LoadScene(sceneToLoad + count.ToString());
    }
}
