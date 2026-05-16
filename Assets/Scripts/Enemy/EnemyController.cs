using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyController : MonoBehaviour
{
    [Header("パラメーター")]
    [Tooltip("移動速度")][SerializeField] private float moveSpeed = 0.1f;
    [Tooltip("体力")][SerializeField] private int hp = 10;
    [Tooltip("攻撃力")][SerializeField] private int damage = 1;
    [Tooltip("報酬金")][SerializeField] private int rewardMoney = 10;

    private CreateEnemy createEnemy;

    //タワー座標
    private Transform towerPos;

    //自身の座標
    private Vector2 selfPos;

    //移動停止フラグ
    private bool isMove = true;

    //死亡フラグ
    private bool isDead = false;

    private void OnValidate()
    {
        //０より大きい値に強制する
        moveSpeed = Mathf.Clamp(moveSpeed, 0, moveSpeed);
        hp = Mathf.Clamp(hp, 0, hp);
        damage = Mathf.Clamp(damage, 0, damage);
    }

    private void Start()
    {
        //参照忘れを防ぐために定義
        towerPos = GameObject.FindWithTag("Tower").transform;
        createEnemy = FindFirstObjectByType<CreateEnemy>();

        //フラグを初期化
        isMove = true;
    }

    void Update()
    { 
        //移動中か？
        if (isMove) MoveToTower();
    }

    /// <summary>
    /// タワーの位置まで移動
    /// </summary>
    private void MoveToTower()
    {
        //Vector3.MoveTowardsで自身の位置から目標位置までどのように移動するか指定可能
        transform.position = Vector3.MoveTowards(transform.position, towerPos.position, moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Towerタグが付いたオブジェクトに触れたか？
        if (collision.gameObject.CompareTag("Tower"))
        {
            //移動不可にする
            isMove = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            //タワーにダメージを与える
            collision.gameObject.GetComponent<TowerController>().TakeDamage(damage);

            //敵を倒す
            TakeDamage(hp);
        }
    }

    /// <summary>
    /// 敵にダメージを与える処理
    /// </summary>
    /// <param name="dealDamage">ダメージ量</param>
    /// <returns></returns>
    public void TakeDamage(int dealDamage)
    {
        if (isDead) return; //すでに死亡している場合は処理しない

        hp -= dealDamage;

        //体力が０以下なら
        if (hp <= 0)
        {
            isDead = true; //死亡フラグを立てる
            hp = 0;
            GameManager.Instance.AddMoney(rewardMoney); //敵を倒した報酬金を追加
            Debug.Log("死亡");
            createEnemy.enemies.Remove(this.gameObject);   //敵のリストから削除
            Debug.Log($"敵の出現数：{createEnemy.enemies.Count}");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log($"敵の残りの体力: {hp}");
        }
    }
}
