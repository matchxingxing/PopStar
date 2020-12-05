using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public bool enabledClickEvent = true;
    public GameObject StarLayer = null;
    public GameObject StarEffect = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //冻结碰撞后的反弹
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //冻结碰撞后的反弹
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnMouseDown()
    {
        //防止暴力测试出现的星星卡死在中间的问题
        if (!enabledClickEvent)
        {
            return;
        }

        SpriteRenderer[] StarList = StarLayer.GetComponentsInChildren<SpriteRenderer>();
        foreach (var Star in StarList)
        {
            Star.GetComponent<Star>().enabledClickEvent = false;
        }
        StarLayer.GetComponent<MainSceneStarLayer>().DelayRestoreEnabledClickEvent();
        //防止暴力测试出现的星星卡死在中间的问题 end

        //播放效果音
        AudioSource[] AudioList = FindObjectOfType<Camera>().GetComponents<AudioSource>();
        AudioList[1].Play();

        Pop();

        //增加分数
        StarLayer.GetComponent<MainSceneStarLayer>().UpdateScore(Constant.PopStarScore);

        //延迟检测星星的相邻
        StarLayer.GetComponent<MainSceneStarLayer>().DelayCheckStarData();

    }

    public void Pop()
    {
        //星星消失的效果
        GameObject StarEffect = Instantiate(this.StarEffect);
        StarEffect.transform.SetParent(StarLayer.transform);
        StarEffect.transform.localPosition = transform.localPosition;
        StarEffect.GetComponent<ParticleSystem>().Play();

        //删除对应的星星数据
        foreach (var StarList in Constant.GetStarDataInstance())
        {
            bool do_remove = false;
            foreach (var Star in StarList)
            {
                if (name == Star.name)
                {
                    StarList.Remove(Star);
                    do_remove = true;
                    break;
                }
            }
            if (do_remove)
            {
                break;
            }
        }
        //Debug.Log(name);
        if (!Constant.GetPopStarDataInstance().Contains(name))
        {
            Constant.GetPopStarDataInstance().Enqueue(name);
        }
        
        Destroy(gameObject);
    }

}
