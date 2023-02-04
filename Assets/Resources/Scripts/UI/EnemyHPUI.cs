using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーのHPを表示するクラス
/// </summary>
public class EnemyHPUI : MonoBehaviour
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
    public void EnemySliderHPViewUI()
    {
        // 割合を求める
        _bulkSlider.value = EnemyManager.enemyHP / (float)EnemyManager.enemyDefaultHP;
    }
}
