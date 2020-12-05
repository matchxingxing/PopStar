using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainSceneBtnSys : MonoBehaviour
{
    private bool isOpenBtnMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        //DOTween.Init();

        Button btnSys = GetComponent<Button>();
        btnSys.onClick.AddListener(delegate ()
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            GameObject btnInfo = GameObject.Find("BtnInfo");
            GameObject btnOff = GameObject.Find("BtnOff");

            Sequence btnSequence = DOTween.Sequence();
            if (isOpenBtnMenu)
            {
                btnSequence.Append(btnOff.transform.DOLocalMoveY(btnSys.gameObject.transform.localPosition.y - 110f, 0.1f)
                    .OnStart(() => OnBtnMoveStart(btnOff, new GameObject[] { }))
                    .OnComplete(() => OnBtnMoveComplete(btnOff, new GameObject[] { })));
                btnSequence.Append(btnInfo.transform.DOLocalMoveY(btnSys.gameObject.transform.localPosition.y, 0.1f)
                    .OnStart(() => OnBtnMoveStart(btnInfo, new GameObject[] { btnOff }))
                    .OnComplete(() => OnBtnMoveComplete(btnInfo, new GameObject[] { btnOff })));
                isOpenBtnMenu = false;
            }
            else
            {
                btnSequence.Append(btnInfo.transform.DOLocalMoveY(btnSys.gameObject.transform.localPosition.y - 110f, 0.1f)
                    .OnStart(() => OnBtnMoveStart(btnInfo, new GameObject[] { btnOff }))
                    .OnComplete(() => OnBtnMoveComplete(btnInfo, new GameObject[] { btnOff })));
                btnSequence.Append(btnOff.transform.DOLocalMoveY(btnSys.gameObject.transform.localPosition.y - 220f, 0.1f)
                    .OnStart(() => OnBtnMoveStart(btnOff, new GameObject[] { }))
                    .OnComplete(() => OnBtnMoveComplete(btnOff, new GameObject[] { })));
                isOpenBtnMenu = true;
            }
            btnSequence.Play();

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBtnMoveStart(GameObject sender, GameObject[] targets)
    {
        foreach (var item in targets)
        {
            item.transform.localScale = new Vector3(0, 0, 1);
        }
    }

    void OnBtnMoveComplete(GameObject sender, GameObject[] targets)
    {
        foreach (var item in targets)
        {
            item.transform.localScale = new Vector3(1, 1, 1);
            item.transform.position = sender.transform.position;
        }
    }

}
