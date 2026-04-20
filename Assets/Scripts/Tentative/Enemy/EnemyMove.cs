using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Tooltip("タワーの座標")]
    [SerializeField] private Transform towerPos;

    [Header("ステータス")]
    [Tooltip("敵の移動速度")]
    [SerializeField] private float moveSpeed = 0.1f;

    [Tooltip("敵の体力")]
    [SerializeField] private uint hp = 10;

    //自身の座標
    private Vector2 selfPos;

    //移動停止フラグ
    private bool isMove = true;

    private void Start()
    {
        //参照忘れを防ぐために定義
        if (towerPos == null) towerPos = GameObject.FindWithTag("Tower").transform;

        //フラグを初期化
        isMove = true;
    }

    void Update()
    { 
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
            isMove = false;
        }
    }

    /// <summary>
    /// マウスカーソルによる受けるダメージ処理
    /// </summary>
    /// <param name="dealDamage">与えるダメージ量</param>
    /// <returns></returns>
    public void MouseDamage(uint dealDamage)
    {
        hp -= dealDamage;

        //体力が０以下なら
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        Debug.Log($"敵の残りの体力: {hp}");
    }
}
