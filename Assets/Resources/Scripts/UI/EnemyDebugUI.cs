using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミー関連のUI
/// </summary>
public class EnemyDebugUI : MonoBehaviour
{
    [SerializeField, Tooltip("タイミング時間表示用Text")]
    private Text _timingText;
    [SerializeField, Tooltip("エネミーの名前")]
    private Text _enemyNameText;
    
    public void ShowEnemyAttackTiming(EnemyBase enemyBase, int choiceNum)
    {
        /*_timingText.text = enemyBase.EnemyAttackTime[choiceNum].ToString(CultureInfo.CurrentCulture);*/
    }

    public void ShowEnemyName()
    {
        _enemyNameText.text = EnemyManager.Enemy.EnemyName;
    }
}
