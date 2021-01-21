using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneBtnExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btnExit = GetComponent<Button>();
        btnExit.onClick.AddListener(delegate ()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
