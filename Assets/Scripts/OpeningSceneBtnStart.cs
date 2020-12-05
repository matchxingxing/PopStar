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
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            //loading需要读取的场景
            PlayerPrefs.SetInt(Constant.NextSceneIndex, Constant.MainScene);
            btnStart.enabled = false;
            Invoke("LoadScene", 1f);
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

}
