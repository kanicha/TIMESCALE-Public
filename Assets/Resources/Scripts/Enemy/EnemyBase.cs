using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの基底クラス
/// </summary>
public class EnemyBase : MonoBehaviour
{
    public int EnemyID; // エネミーのIDとモデル
    
    public string EnemyName; // スキルの名前
    public int EnemyHP;
    public List<List<float>> AttackTimes; // 全ての攻撃時間を入れる
    
    public List<float> AttackTimePatternA; // パターンAの攻撃
    public List<float> AttackTimePatternB; // パターンBの攻撃
    public List<float> AttackTimePatternC; // パターンCの攻撃
    public List<float> AttackTimePatternD; // パターンDの攻撃

    public Timer.Timers[] EnemyTimer; // このモンスターが出現している間のタイマー制限
    
    /// <summary>
    /// エネミーの攻撃 Aパターン
    /// </summary>
    public virtual void OnAttackPatternA()
    {
    }

    /// <summary>
    /// エネミーの攻撃 Aパターン2
    /// </summary>
    public virtual void OnAttackPatternA2()
    {
    }
    
    /// <summary>
    /// エネミーの攻撃 Bパターン
    /// </summary>
    public virtual void OnAttackPatternB()
    {
        
    }
    
    /// <summary>
    /// エネミーの攻撃 Bパターン
    /// </summary>
    public virtual void OnAttackPatternB2()
    {
        
    }
    
    /// <summary>
    /// エネミーの攻撃 Cパターン
    /// </summary>
    public virtual void OnAttackPatternC()
    {
        
    }
    
    /// <summary>
    /// エネミーの攻撃 Cパターン
    /// </summary>
    public virtual void OnAttackPatternC2()
    {
        
    }

    /// <summary>
    /// エネミーの攻撃 Dパターン
    /// </summary>
    public virtual void OnAttackPatternD()
    {
        
    }
    
    /// <summary>
    /// エネミーの攻撃 Dパターン
    /// </summary>
    public virtual void OnAttackPatternD2()
    {
        
    }
}
