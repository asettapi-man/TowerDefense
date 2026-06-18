using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int money = 0;              //所持金
    private int enemyKillCounter = 0;   //敵を倒した数
    private bool isGameStopped = false; //ゲームの進行フラグ

    // == プロパティ ==
    public int Money { get => money; set => money = value; }
    public int EnemyKillCounter { get => enemyKillCounter; set => enemyKillCounter = value; }

    public bool IsGameStopped { get => isGameStopped; set => isGameStopped = value; }

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

    /// <summary>
    /// ゲーム進行停止
    /// </summary>
    public void StopGame()
    {
        isGameStopped = true;
    }

    /// <summary>
    /// ゲーム進行再開
    /// </summary>
    public void ResumeGame()
    {
        isGameStopped = false;
    }
}
