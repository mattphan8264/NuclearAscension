using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    GameObject loadingScreen;
    BlackFade blackFade;
    bool loadingScene = false;

    void Awake()
    {
        loadingScreen = transform.GetChild(0).gameObject;
        blackFade = GetComponentInChildren<BlackFade>();
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int temp;

    public void loadScene(int input)
    {
        if (!loadingScene)
        {
            loadingScreenOn();
            loadingScene = true;

            temp = input;

            startLoading();
        }
        
    }

    void startLoading()
    {   
        StartCoroutine(loadNewScene(temp));
    }

    IEnumerator loadNewScene(int input)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(input);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2);
        StartCoroutine(fadeIn(async));
    }

    IEnumerator fadeIn(AsyncOperation async)
    {
        StartCoroutine(blackFade.blackFadeIn());
        yield return new WaitForSeconds(1.4f);
        loadingScreenOff();
        yield return new WaitForSeconds(.5f);
        async.allowSceneActivation = true;

        yield return new WaitForSeconds(2);

        StartCoroutine(blackFade.blackFadeOut());
    }

    void loadingScreenOn()
    {
        loadingScreen.SetActive(true);
    }

    void loadingScreenOff()
    {
        loadingScreen.SetActive(false);
    }
}
