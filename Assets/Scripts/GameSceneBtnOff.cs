using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneBtnOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btnOff = GetComponent<Button>();
        btnOff.onClick.AddListener(delegate ()
        {
            PlayAudioCallback(GetComponent<AudioSource>(), LoadScene);

            //loading需要读取的场景
            PlayerPrefs.SetInt(Constant.NextSceneIndex, Constant.MainScene);
            btnOff.enabled = false;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadScene()
    {
        //打扫战场，清理场景用的临时数据
        foreach (var item in Constant.GetStarDataInstance())
        {
            item.Clear();
        }
        foreach (var item in Constant.GetSameStarDataInstance())
        {
            item.Value.Clear();
        }
        Constant.GetStarDataInstance().Clear();
        Constant.GetPopStarDataInstance().Clear();
        Constant.CurrScore = 0;
        Constant.CurrStage = 1;
        //打扫战场，清理场景用的临时数据 end

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
