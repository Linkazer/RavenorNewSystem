using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RVN_SceneManager : Singleton<RVN_SceneManager>
{
    [SerializeField] private CanvasGroup loadingScreen;

    [SerializeField] private Animator loadingAnimation;

    [SerializeField] private float minimumLoadTime = 1f;

    public static Action ToDoAfterLoad;

    private void Start()
    {
        TimerManager.CreateRealTimer(Time.deltaTime, () => OnSceneLoaded(null));
    }

    public void ReloadScene()
    {
        instance.LaunchSceneLoading(SceneManager.GetActiveScene().buildIndex, null);
    }

    public void LoadScene(int sceneIndex)
    {
        instance.LaunchSceneLoading(sceneIndex, null);
    }

    private void LaunchSceneLoading(int sceneIndex, Action callback)
    {
        StartCoroutine(LoadAsyncScene(sceneIndex, callback));
    }

    private IEnumerator LoadAsyncScene(int sceneIndex, Action callback)
    {
        loadingScreen.alpha = 1;
        loadingAnimation.enabled = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(minimumLoadTime);

        OnSceneLoaded(callback);
    }

    private void OnSceneLoaded(Action callback)
    {
        ToDoAfterLoad?.Invoke();
        ToDoAfterLoad = null;

        callback?.Invoke();

        loadingScreen.alpha = 0;
        loadingAnimation.enabled = false;
    }
}
