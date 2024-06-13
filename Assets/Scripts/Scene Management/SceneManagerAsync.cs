using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerAsync : MonoBehaviour
{

    [SerializeField] Slider _loadingBar;
    [SerializeField] GameObject _menuUI, _loadingUI;



    public void LoadSceneAsync(string sceneToLoad)
    {

        StartCoroutine(LoadSceneAsyncCou(sceneToLoad));
        
    }

    IEnumerator LoadSceneAsyncCou(string sceneToLoad)
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        _menuUI.SetActive(false);
        _loadingUI.SetActive(true);
        float progress = 0;
        while (!operation.isDone) 
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            _loadingBar.value = progress;
            Debug.Log($"operation progress: {progress}");

            yield return null;
        }


        yield return null;
    }


    public void QuitGame()
    {
        Debug.Log("Quitting app");
        Application.Quit();
    }

   
}
