using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private AsyncOperation Async = null;
    private Text BtnStartText;

    private IEnumerator LoadScene(int scene)
    {
        Async = SceneManager.LoadSceneAsync(scene);
        Async.allowSceneActivation = false;

        yield return Async;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject btnStart = GameObject.Find("LoadingMsg");
        BtnStartText = btnStart.GetComponent<Text>();

        int next_scene_index = PlayerPrefs.GetInt(Constant.NextSceneIndex, Constant.MainScene);
        StartCoroutine(LoadScene(next_scene_index));
    }

    // Update is called once per frame
    void Update()
    {
        //异步loading
        if (Async != null)
        {
            int to_process;

            //Debug.Log(Async.progress * 100);

            if (Async.progress < 0.9f) //坑爹的progress，最多到0.9f
            {
                to_process = (int)(Async.progress * 100);
            }
            else
            {
                to_process = 100;
            }

            //显示部分
            BtnStartText.text = "已加载：" + to_process + "%";

            if (to_process == 100) //Async.isDone应该是在场景被激活时才为true
            {
                Async.allowSceneActivation = true;
            }
        }
        //异步loading end
    }
}
