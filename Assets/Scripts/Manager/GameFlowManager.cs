using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    /// <summary>
    /// シーン遷移関数
    /// </summary>
    /// <param name="sceneName">切り替え先のシーン名</param>
    public void SceneLoad(string sceneName)
    {
        Debug.Log($"{sceneName}に遷移開始");
        SceneManager.LoadScene(sceneName);  //シーン遷移
    }

    /// <summary>
    /// ゲーム終了関数
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(); //アプリケーションの終了
        Debug.Log("ゲーム終了");
    }

    public void ResetGame()
    {
        GameManager.Instance.Money = 0;
        GameManager.Instance.EnemyKillCounter = 0;
        GameManager.Instance.IsGameStopped = false;
    }
}
