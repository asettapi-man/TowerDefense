using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("タワーのステータス")]
    [Tooltip("タワーの体力")]
    [SerializeField] private int hp = 10;


    private void OnValidate()
    {
        //０より大きい値に強制する
        hp = Mathf.Clamp(hp, 0, hp);
    }

    void Start()
    {
        
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
