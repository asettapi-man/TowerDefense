using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int enemyKillCounter = 0;   //敵を倒した数

    // == プロパティ ==
    public int money { get; private set; }  //所持金
    public int EnemyKillCounter { get => enemyKillCounter; set => enemyKillCounter = value; }   //敵を倒した数

    //所持金が変化したときに呼び出されるイベント
    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        //シングルトンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 敵を倒した時にお金を追加する
    /// </summary>
    /// <param name="amount">報酬金</param>
    public void AddMoney(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(GameManager.Instance.money); //所持金が変化したことを通知
        Debug.Log($"所持金：{money}");
    }
}
