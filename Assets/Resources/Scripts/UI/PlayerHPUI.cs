using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのHPを表示するクラス
/// </summary>
public class PlayerHPUI : MonoBehaviour
{
    [Header("スライダー各種のインポート")]
    [SerializeField, Tooltip("スライダー青")]
    private Slider _bulkSlider;

    private float currentHP = 0f;
    private void Start()
    {
        _bulkSlider.value = 1;
    }

    /// <summary>
    /// プレイヤーのスライダーHPを表示する関数
    /// </summary>
    public void PlayerSliderHPViewUI()
    {
        // 割合を求める
        _bulkSlider.value = PlayerManager.playerHP / (float)PlayerManager.playerDefaultHP;
    }
}
