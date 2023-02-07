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
    
    [Header("HPの数値表示用コンポーネント")]
    [SerializeField, Tooltip("初期HP表示用テキスト")]
    private Text _defaultHPText;
    [SerializeField, Tooltip("現在のHP表示用テキスト")]
    private Text _nowHPText;
    
    [Header("ダメージ表示用コンポーネント")] 
    [SerializeField, Tooltip("与えられたダメージ")]
    private GameObject _hitDamage;
    [SerializeField, Tooltip("生成するキャンバス")]
    private Canvas _canvas;
    
    private float currentHP = 0f;
    private void Start()
    {
        _bulkSlider.value = 1;
    }

    public void InitShowHP()
    {
        // 初期化
        _defaultHPText.text = EnemyManager.enemyDefaultHP.ToString();
        _nowHPText.text = EnemyManager.enemyDefaultHP.ToString();
    }
    
    /// <summary>
    /// HPの表示を行う関数
    /// </summary>
    public void ShowHP()
    {
        _nowHPText.text = EnemyManager.enemyHP.ToString();
    }

    /// <summary>
    /// プレイヤーのスライダーHPを表示する関数
    /// </summary>
    public void EnemySliderHPViewUI()
    {
        // 割合を求める
        _bulkSlider.value = EnemyManager.enemyHP / (float)EnemyManager.enemyDefaultHP;
    }
    
    /// <summary>
    /// ダメージを食らった時に表示する関数
    /// </summary>
    /// <param name="damage"> 食らったダメージ </param>>
    public void ShowDamageUI(int damage)
    {
        _hitDamage.GetComponent<Text>().text = damage.ToString();
        
        // 生成をキャンバスで行う
        GameObject createdObj = Instantiate(_hitDamage, _canvas.transform, true);
    }
}
