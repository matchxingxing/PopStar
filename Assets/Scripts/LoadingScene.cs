using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private AsyncOperation async;
    private uint nowProcess;
    private Text btnStartText;

    private IEnumerator LoadScene(int scene)
    {
        async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;

        yield return async;
    }

    // Start is called before the first frame update
    void Start()
    {
        nowProcess = 0;

        GameObject btnStart = GameObject.Find("LoadingMsg");
        btnStartText = btnStart.GetComponent<Text>();

        int next_scene_index = PlayerPrefs.GetInt(Constant.NextSceneIndex, Constant.MainScene);
        StartCoroutine(LoadScene(next_scene_index));
    }

    // Update is called once per frame
    void Update()
    {
        //异步loading
        if (async == null)
        {
            return;
        }

        uint to_process;

        //Debug.Log(async.progress * 100);

        if (async.progress < 0.9f) //坑爹的progress，最多到0.9f
        {
            to_process = (uint)(async.progress * 100);
        }
        else
        {
            to_process = 100;
        }

        if (nowProcess < to_process)
        {
            nowProcess++;
        }

        //显示部分
        btnStartText.text = "已加载：" + nowProcess + "%";
        //Debug.Log(btnStartText.text);

        if (nowProcess == 100) //async.isDone应该是在场景被激活时才为true
        {
            async.allowSceneActivation = true;
        }

        //异步loading end
    }
}
