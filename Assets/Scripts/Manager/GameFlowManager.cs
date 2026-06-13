using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    //シングルトン
    public static GameFlowManager Instance;

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
}
