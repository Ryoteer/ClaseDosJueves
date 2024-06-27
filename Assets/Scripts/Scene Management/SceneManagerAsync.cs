using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerAsync : MonoBehaviour
{
    [Header("<color=orange>UI</color>")]
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

        operation.allowSceneActivation = false;

        float progress = 0;

        while (operation.progress < .9f) 
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            _loadingBar.value = progress;
            Debug.Log($"operation progress: {progress}");

            yield return null;
        }

        _loadingBar.value = 1f;

        UpdateManager.Instance.Clear();

        operation.allowSceneActivation = true;
    }


    public void QuitGame()
    {
        UpdateManager.Instance.Clear();

        Debug.Log("Quitting app");
        Application.Quit();
    }   
}
