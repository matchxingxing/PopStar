using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpeningSceneBtnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btnStart = GetComponent<Button>();
        btnStart.onClick.AddListener(delegate ()
        {
            PlayAudioCallback(GetComponent<AudioSource>(), LoadScene);

            //loading需要读取的场景
            PlayerPrefs.SetInt(Constant.NextSceneIndex, Constant.MainScene);
            btnStart.enabled = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadScene()
    {
        SceneManager.LoadScene(Constant.LoadingScene);
    }

    //声音播放的回调
    delegate void AudioCallBack();
    void PlayAudioCallback(AudioSource audio, AudioCallBack callback)
    {
        audio.Play();
        StartCoroutine(DelayedCallback(audio.clip.length, callback));
    }
    IEnumerator DelayedCallback(float time, AudioCallBack callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
    //声音播放的回调 end

}
