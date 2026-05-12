using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class TowerController : MonoBehaviour
{
    [Header("パラメーター")]
    [Tooltip("タワーの体力")][SerializeField] private int hp = 10;

    private CreateEnemy createEnemy;

    private void OnValidate()
    {
        //０より大きい値に強制する
        hp = Mathf.Clamp(hp, 0, hp);
    }

    void Start()
    {
        //参照忘れを防ぐために参照先を登録
        createEnemy = GameObject.FindFirstObjectByType<CreateEnemy>();
    }


    void Update()
    {

    }

    /// <summary>
    /// タワーにダメージを与える処理
    /// </summary>
    /// <param name="dealDamage"></param>
    public void TakeDamage(int dealDamage)
    {
        hp -= dealDamage;

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        Debug.Log($"タワーの残りの体力: {hp}");
    }
}
