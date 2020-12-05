using System.Collections.Generic;
using UnityEngine;

public class Constant
{
    //场景索引
    public static readonly int OpeningScene = 0;
    public static readonly int LoadingScene = 1;
    public static readonly int MainScene = 2;

    //同样颜色的星星，超过n个就消除
    public static readonly uint PopSameStarCount = 4;

    //消除了1个星星的分数
    public static readonly ulong PopStarScore = 10;

    //同色星星消除加成的基数
    public static readonly uint PopSameScoreMult = 2;

    //最大分数
    public static readonly ulong MaxScore = 999999;

    //最大关卡
    public static readonly uint MaxStage = 99;

    //一行显示多少个星星
    public static readonly int StarXCount = 10;

    //共多少行星星
    public static readonly int StarYCount = 10;

    public static readonly string NextSceneIndex = "next_scene_index";

    //当前分数
    public static ulong CurrScore = 0;

    //当前关卡
    public static uint CurrStage = 1;

    //星星的数据
    private static IList<IList<SpriteRenderer>> StarDataInstance;
    private static object StarDataInstanceLock = new object();
    public static IList<IList<SpriteRenderer>> GetStarDataInstance()
    {
        if (StarDataInstance == null)
        {
            lock (StarDataInstanceLock)
            {
                if (StarDataInstance == null)
                {
                    StarDataInstance = new List<IList<SpriteRenderer>>();
                }
            }
        }
        return StarDataInstance;
    }

    //消了星星的处理列队
    private static Queue<string> PopStarDataInstance;
    private static object PopStarDataInstanceLock = new object();
    public static Queue<string> GetPopStarDataInstance()
    {
        if (PopStarDataInstance == null)
        {
            lock (PopStarDataInstanceLock)
            {
                if (PopStarDataInstance == null)
                {
                    PopStarDataInstance = new Queue<string>();
                }
            }
        }
        return PopStarDataInstance;
    }

    //同样颜色的星星列队，超过PopSameStarCount个就消除
    private static Queue<SpriteRenderer> SameStarDataInstance;
    private static object SameStarDataInstanceLock = new object();
    public static Queue<SpriteRenderer> GetSameStarDataInstance()
    {
        if (SameStarDataInstance == null)
        {
            lock (SameStarDataInstanceLock)
            {
                if (SameStarDataInstance == null)
                {
                    SameStarDataInstance = new Queue<SpriteRenderer>();
                }
            }
        }
        return SameStarDataInstance;
    }

}
