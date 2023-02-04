using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイマーの値を習得してきてUIとして表示を行うクラス
/// </summary>
public class TimerUI : MonoBehaviour
{
    [Header("タイマー表示用変数群")] [SerializeField, Tooltip("現在のタイマー表示用変数")]
    private Text _nowTimerText = null;

    [SerializeField, Tooltip("次のタイマー表示用変数")]
    private Text _nextTimerText = null;
    
    /// <summary>
    /// TimerのUIを表示する関数
    /// </summary>
    public void TimerUIDraw()
    {
        _nowTimerText.text = Timer._intNowTimerLength.ToString();
        _nextTimerText.text = Timer._intNextTimer.ToString();
    }
}
