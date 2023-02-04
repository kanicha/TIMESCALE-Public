using UnityEngine;

/// <summary>
/// ゲームクリアコントローラークラス
/// </summary>
public class GameClearController : MonoBehaviour
{
    [SerializeField, Tooltip("フェイドタイム")] private float _fadeTime = 0.5f;
    
    /// <summary>
    /// タッチされたらタイトル画面にうつる
    /// </summary>
    public void GoToTitleScene()
    {
        if (ScreenTouch.GetPhase() == Phase.Began)
        {
            // シーンを変更
            FadeController.Instance.LoadScene(_fadeTime, GameScene.TitleScene);
        }
    }
}
