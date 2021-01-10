using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        CanvasScaler canvas_scaler = canvas.GetComponent<CanvasScaler>();
        canvas_scaler.referenceResolution = new Vector2(Constant.WindowWidth, Constant.WindowHeight);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
