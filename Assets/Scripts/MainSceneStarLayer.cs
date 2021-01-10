using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneStarLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //本游戏的x坐标固定在-5.607到-5.607，y坐标固定在-10到10

        Canvas canvas = FindObjectOfType<Canvas>();
        canvas.transform.Find("StageN").GetComponent<Text>().text = Constant.CurrStage.ToString();
        canvas.transform.Find("ScoreN").GetComponent<Text>().text = Constant.CurrScore.ToString();

        //获取camera设置的size值，本游戏计算的是高度
        float camera_size = FindObjectOfType<Camera>().orthographicSize;

        //计算每个像素对应的unity单位
        float unit = camera_size * 2f / Mathf.Max(Screen.width, Screen.height);

        //计算屏幕宽度（unity单位）
        float camera_size2 = unit * Mathf.Min(Screen.width, Screen.height);

        /*Debug.Log("操作系统：" + SystemInfo.operatingSystem);
        Debug.Log("设备唯一标识符：" + SystemInfo.deviceUniqueIdentifier);
        Debug.Log("设备名称：" + SystemInfo.deviceName);
        Debug.Log("显卡设备名称：" + SystemInfo.graphicsDeviceName);
        Debug.Log("显卡支持版本：" + SystemInfo.graphicsDeviceVersion);
        Debug.Log("显存大小：" + SystemInfo.graphicsMemorySize);
        Debug.Log("屏幕像素：" + Screen.width + "x" + Screen.height);
        Debug.Log("unity单位：" + unit);
        Debug.Log("宽度的size：" + camera_size2);*/

        SpriteRenderer[] StarListSrc = GameObject.Find("StarLayerSrc").GetComponentsInChildren<SpriteRenderer>();

        float star_scale_x = unit * Screen.width / Constant.StarXCount / (StarListSrc[0].sprite.rect.width / 100f);
        float star_scale_y = StarListSrc[0].sprite.rect.height / StarListSrc[0].sprite.rect.width * star_scale_x; //高的放缩还未知，所以用高和宽的比例来计算
        //Debug.Log(star_scale_x);
        //Debug.Log(star_scale_y);

        for (int x = 0; x < Constant.StarXCount; x++)
        {
            IList<SpriteRenderer> StarList = new List<SpriteRenderer>();

            for (int y = 0; y < Constant.StarYCount; y++)
            {
                //随机加载星星对象
                int star_idx = Random.Range(0, StarListSrc.Length);
                SpriteRenderer star = Instantiate(StarListSrc[star_idx]);
                star.transform.SetParent(this.transform);
                star.name = "Star_" + x.ToString() + "_" + y.ToString();

                star.transform.localScale = new Vector3(star_scale_x, star_scale_y, 1f);

                //计算星星的大小
                Vector2 star_size = star.sprite.rect.size / star.sprite.pixelsPerUnit * star.transform.localScale.y;

                //计算星星的位置
                float pos_x = -(camera_size2 / 2f) + (star_size.x / 2f) + (x * star_size.x);
                float pos_y = -camera_size + (star_size.y / 2f) + (y * star_size.y);
                star.transform.position = new Vector3(pos_x, pos_y, -8.0f); //-8只是为了把星星显示在背景图的上面

                StarList.Add(star);

                //break;
            }

            Constant.GetStarDataInstance().Add(StarList);
            //break;
        }

        //test
        /*foreach (var item in Constant.GetStarDataInstance())
        {
            foreach (var item2 in item)
            {
                Debug.Log(item2.name);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //延迟检测同颜色的星星的相邻
    public void DelayCheckStarData()
    {
        Invoke("DelayCheckStarDataTimer", 0.75f);
    }

    void DelayCheckStarDataTimer()
    {
        //检测是否已经过关
        bool is_clear = true;
        foreach (var StarYList in Constant.GetStarDataInstance())
        {
            if (StarYList.Count > 0)
            {
                is_clear = false;
            }
        }
        if (is_clear)
        {
            AudioSource[] AudioList = FindObjectOfType<Camera>().GetComponents<AudioSource>();
            AudioList[0].Stop();

            Transform transform_root = FindObjectOfType<Canvas>().transform;
            transform_root.Find("BtnSys").GetComponent<Button>().enabled = false;
            transform_root.Find("BtnInfo").GetComponent<Button>().enabled = false;
            transform_root.Find("BtnOff").GetComponent<Button>().enabled = false;
            PlayAudioCallback(AudioList[3], NextStage);
            return;
        }
        //检测是否已经过关 end

        while (Constant.GetPopStarDataInstance().Count > 0)
        {
            string curr_pop_name = Constant.GetPopStarDataInstance().Dequeue();
            string[] s = curr_pop_name.Split('_');
            int curr_x = int.Parse(s[1]);
            int curr_y = int.Parse(s[2]);

            for (int i = 0; i < Constant.GetStarDataInstance()[curr_x].Count; i++)
            {
                SpriteRenderer star = Constant.GetStarDataInstance()[curr_x][i];
                string[] s2 = star.name.Split('_');
                int curr_y2 = int.Parse(s2[2]);
                if (curr_y2 > curr_y)
                {
                    //递归检测同颜色的星星
                    CheckStar(star);

                    //消除同色的星星
                    PopSameStar();
                }
            }
        }
    }

    void CheckStar(SpriteRenderer star)
    {
        //Debug.Log("当前：" + star.name);
        //加入同样颜色的星星列队，递归完后超过Constant.PopSameStarCount个就消除
        /*foreach (var item in Constant.GetSameStarDataInstance()[star.tag])
        {
            if (star.name == item.name)
            {
                return;
            }
        }*/
        if (Constant.GetSameStarDataInstance()[star.tag].Contains(star))
        {
            return;
        }
        Constant.GetSameStarDataInstance()[star.tag].Enqueue(star);

        SpriteRenderer StarUp = null,
            StarDown = null,
            StarLeft = null,
            StarRight = null;

        string[] s = star.name.Split('_');
        int curr_x = int.Parse(s[1]);
        int curr_y = int.Parse(s[2]);

        //上
        for (int i = 0; i < Constant.GetStarDataInstance()[curr_x].Count; i++)
        {
            SpriteRenderer star2 = Constant.GetStarDataInstance()[curr_x][i];
            string[] s2 = star2.name.Split('_');
            int curr_y2 = int.Parse(s2[2]);
            if (StarUp == null && curr_y2 > curr_y)
            {
                StarUp = star2;
                break;
            }
        }

        //下
        for (int i = Constant.GetStarDataInstance()[curr_x].Count - 1; i >= 0; i--)
        {
            SpriteRenderer star2 = Constant.GetStarDataInstance()[curr_x][i];
            string[] s2 = star2.name.Split('_');
            int curr_y2 = int.Parse(s2[2]);
            if (StarDown == null && curr_y2 < curr_y)
            {
                StarDown = star2;
                break;
            }
        }

        //左右
        for (int i = 0; i < Constant.GetStarDataInstance()[curr_x].Count; i++)
        {
            SpriteRenderer star2 = Constant.GetStarDataInstance()[curr_x][i];
            string[] s2 = star2.name.Split('_');
            int curr_y2 = int.Parse(s2[2]);
            if (StarLeft == null && curr_x > 0 && curr_y2 == curr_y)
            {
                IList<SpriteRenderer> StarYListLeft = Constant.GetStarDataInstance()[curr_x - 1];
                if (i + 1 <= StarYListLeft.Count)
                {
                    StarLeft = StarYListLeft[i];
                }
            }
            if (StarRight == null && curr_x + 1 < Constant.StarXCount && curr_y2 == curr_y)
            {
                IList<SpriteRenderer> StarYListRight = Constant.GetStarDataInstance()[curr_x + 1];
                if (i + 1 <= StarYListRight.Count)
                {
                    StarRight = StarYListRight[i];
                }
            }
        }
        
        //相同颜色就进入递归处理
        if (StarUp != null && StarUp.tag == star.tag)
        {
            CheckStar(StarUp);
            //Debug.Log("上：" + StarUp.name + "====tag:" + StarUp.tag);
        }
        if (StarDown != null && StarDown.tag == star.tag)
        {
            CheckStar(StarDown);
            //Debug.Log("下：" + StarDown.name + "====tag:" + StarDown.tag);
        }
        if (StarLeft != null && StarLeft.tag == star.tag)
        {
            CheckStar(StarLeft);
            //Debug.Log("左：" + StarLeft.name + "====tag:" + StarLeft.tag);
        }
        if (StarRight != null && StarRight.tag == star.tag)
        {
            CheckStar(StarRight);
            //Debug.Log("右：" + StarRight.name + "====tag:" + StarRight.tag);
        }
    }

    void PopSameStar()
    {
        ulong score = 0;
        foreach (var item in Constant.GetSameStarDataInstance()) {
            if (item.Value.Count >= Constant.PopSameStarCount)
            {
                uint star_count = (uint)item.Value.Count;
                uint mult = star_count / Constant.PopSameStarCount;

                while (item.Value.Count > 0)
                {
                    SpriteRenderer star = item.Value.Dequeue();
                    //Debug.Log("消除了同色的星星：" + star.name + "=====" + star.tag);
                    star.GetComponent<Star>().Pop();
                }

                //分数加成计算
                score += mult * star_count * Constant.PopSameScoreMult * Constant.PopStarScore;
            }
            else
            {
                item.Value.Clear();
            }
        }

        if (score > 0)
        {
            //播放效果音
            AudioSource[] AudioList = FindObjectOfType<Camera>().GetComponents<AudioSource>();
            AudioList[2].Play();

            UpdateScore(score);
        }
    }

    void NextStage()
    {
        Constant.CurrStage++;
        if (Constant.CurrStage > Constant.MaxStage)
        {
            Constant.CurrStage = Constant.MaxStage;
        }

        PlayerPrefs.SetInt(Constant.NextSceneIndex, Constant.MainScene);

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
        //打扫战场，清理场景用的临时数据 end

        SceneManager.LoadScene(Constant.LoadingScene);
    }

    //防止暴力测试出现的星星卡死在中间的问题
    public void DelayRestoreEnabledClickEvent()
    {
        Invoke("DelayRestoreEnabledClickEventTimer", 1f);
    }

    void DelayRestoreEnabledClickEventTimer() 
    {
        SpriteRenderer[] StarList = GameObject.Find("StarLayer").GetComponentsInChildren<SpriteRenderer>();
        foreach (var star in StarList)
        {
            star.GetComponent<Star>().enabledClickEvent = true;
        }
    }
    //防止暴力测试出现的星星卡死在中间的问题 end

    public void UpdateScore(ulong score)
    {
        Constant.CurrScore += score;
        if (Constant.CurrScore > Constant.MaxScore)
        {
            Constant.CurrScore = Constant.MaxScore;
        }
        FindObjectOfType<Canvas>().transform.Find("ScoreN").GetComponent<Text>().text = Constant.CurrScore.ToString();
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
