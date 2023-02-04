using UnityEngine;

/// <summary>
/// タイトルマネージャー
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField, Tooltip("タイトル点滅用クラス")]
    private NamePlateFade namePlateFade;
    [SerializeField, Tooltip("タイトルコントローラークラス")]
    private TitleController _titleController;

    private void Update()
    {
        namePlateFade.ChangeColor();
        
        _titleController.GoToGameScene();
    }
}
