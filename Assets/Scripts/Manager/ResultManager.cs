using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [Tooltip("リザルトCanvas"), SerializeField] private Canvas resultCanvas;
    [Tooltip("討伐数表示テキスト"), SerializeField] private Text killEnemyCounterText;

    //== プロパティ ==
    public Canvas ResultCanvas { get => resultCanvas; private set => resultCanvas = value; }

    void Start()
    {
        resultCanvas.gameObject.SetActive(false);   //非表示
    }

    /// <summary>
    /// リザルト画面で表示する内容設定
    /// </summary>
    public void ResultTextDisplay()
    {
        //テキストに設定
        killEnemyCounterText.text = GameManager.Instance.EnemyKillCounter.ToString();   //敵を倒した数
    }
}
