using UnityEngine;

/// <summary>
/// ゲームオーバーのマネージャークラス
/// </summary>
public class GameClearManager : MonoBehaviour
{
    [SerializeField, Tooltip("文字フェードのスクリプト")]
    private NamePlateFade _namePlate;
    
    [SerializeField, Tooltip("ゲームオーバーコントローラークラス")]
    private GameClearController _gameClearController;

    private void Update()
    {
        _namePlate.ChangeColor();
        _gameClearController.GoToTitleScene();
    }
}
