using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトル画面の文字をfadeさせる関数
/// </summary>
public class NamePlateFade : MonoBehaviour
{
    [SerializeField, Tooltip("点滅させるオブジェクト")]
    private Text _modeSelectTextArray;

    [SerializeField, Header("点滅させるスピード")] private float _blinkSpeed = 0.0f;

    [SerializeField, Tooltip("白か黒か")] private bool isBlack = false;

    // 時間計測変数
    private float _sceneTime = 0.0f;
    
    private void Start()
    {
        // 値を初期化
        _modeSelectTextArray.color = GetAlphaColor(_modeSelectTextArray.color);
        
        if (isBlack)
            _modeSelectTextArray.color = Color.black;
        else
            _modeSelectTextArray.color = Color.white;
    }

    /// <summary>
    /// 色を変える関数
    /// </summary>
    public void ChangeColor()
    {
        _modeSelectTextArray.color = GetAlphaColor(_modeSelectTextArray.color);
    }

    //Alpha値を更新してColorを返す
    private Color GetAlphaColor(Color color)
    {
        _sceneTime += Time.deltaTime * 5.0f * _blinkSpeed;
        color.a = Mathf.Sin(_sceneTime) * 0.5f + 0.5f;

        return color;
    }
}