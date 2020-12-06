using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneBtnOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btnOff = GetComponent<Button>();
        btnOff.onClick.AddListener(delegate ()
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            //loading需要读取的场景
            PlayerPrefs.SetInt(Constant.NextSceneIndex, Constant.OpeningScene);
            btnOff.enabled = false;
            Invoke("LoadScene", 1f);
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
        Constant.GetSameStarDataInstance().Clear();
        Constant.CurrScore = 0;
        Constant.CurrStage = 1;
        //打扫战场，清理场景用的临时数据 end

        SceneManager.LoadScene(Constant.LoadingScene);
    }

}
