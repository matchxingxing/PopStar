using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneBtnInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Button btnInfo = GetComponent<Button>();
        btnInfo.onClick.AddListener(delegate ()
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            Debug.Log("点击了info按钮");

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
