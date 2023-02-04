using UnityEngine;

/// <summary>
/// タイトルの操作関係処理
/// </summary>
public class TitleController : MonoBehaviour
{
    [SerializeField, Tooltip("フェイドタイム")] private float _fadeTime = 0.5f;
    
    /// <summary>
    /// タッチされたらゲーム画面にうつる
    /// </summary>
    public void GoToGameScene()
    {
        if (ScreenTouch.GetPhase() == Phase.Began)
        {
            // シーンを変更
            FadeController.Instance.LoadScene(_fadeTime, GameScene.GameScene);
        }
    }
}
