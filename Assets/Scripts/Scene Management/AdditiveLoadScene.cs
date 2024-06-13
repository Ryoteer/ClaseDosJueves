using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveLoadScene : MonoBehaviour
{
    [SerializeField] string _sceneToLoad;
    [SerializeField] Animation openDoorAnimation;

    [SerializeField] string sceneToUnload;


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) UnloadScene(sceneToUnload);


    }



    private void OnTriggerEnter(Collider other)
    {
        
        if(other.TryGetComponent<Player>(out Player player))
        {
            LoadSceneAdditive(_sceneToLoad);
            

        }


    }


    public void LoadSceneAdditive(string sceneToLoad)
    {
        StartCoroutine(AddScene(sceneToLoad));

    }

    IEnumerator AddScene(string sceneToAdd)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToAdd, LoadSceneMode.Additive); ;
        operation.completed += PlayAnimation;


        yield return null;
    }

    public void PlayAnimation(AsyncOperation operation)
    {
        openDoorAnimation.Play();
        gameObject.SetActive(false);
    }


    public void UnloadScene(string sceneToUnload)
    {

        SceneManager.UnloadSceneAsync(sceneToUnload);


    }




}
