using UnityEngine;

/// <summary>
/// ゲームオーバーのマネージャークラス
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [SerializeField, Tooltip("文字フェードのスクリプト")]
    private NamePlateFade _namePlate;
    
    [SerializeField, Tooltip("ゲームオーバーコントローラークラス")]
    private GameOverController _gameOverController;

    private void Update()
    {
        _namePlate.ChangeColor();
        _gameOverController.GoToTitleScene();
    }
}
