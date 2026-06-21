using UnityEngine;
using UnityEngine.UI;

public class CastleController : MonoBehaviour
{
    [Tooltip("体力"), SerializeField] private int hp;
    [Tooltip("体力表示テキスト"), SerializeField] private Text hpText;

    private ResultManager resultManager;

    void Awake()
    {
        resultManager = GameObject.FindFirstObjectByType<ResultManager>();
        if (hpText != null)
        {
            hpText.text = hp.ToString();
        }
    }

    /// <summary>
    /// 城にダメージを与える処理
    /// </summary>
    /// <param name="dealDamage"></param>
    public void TakeDamage(int dealDamage)
    {
        hp -= dealDamage;

        if (hp <= 0)
        {
            GameManager.Instance?.StopGame();
            resultManager?.ResultTextDisplay();
            //体力が０になったら破棄
            //Destroy(this.gameObject);
        }
        else
        {
            if (hpText != null)
            {
                //HPテキストの更新
                hpText.text = hp.ToString();
            }
        }
        Debug.Log($"城の残りの体力: {hp}");
    }
}
