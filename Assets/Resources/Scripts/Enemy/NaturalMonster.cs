using System.Collections.Generic;
using UnityEngine;

public class NaturalMonster : EnemyBase
{
    public NaturalMonster()
    {
        EnemyID = 1;
        
        EnemyName = "何の変哲もないモンスター";
        // 仮に200
        EnemyHP = 200;
        
        /*
        AttackTimePatternA.Add(8);
        AttackTimePatternB.Add(10);
        AttackTimePatternC.Add(5);
        AttackTimePatternD.Add(8);
        AttackTimePatternD.Add(12);*/
    }
    
    private void Awake()
    {
        // 初期化
        AttackTimes = new List<List<float>>
        {
            AttackTimePatternA,
            AttackTimePatternB,
            AttackTimePatternC,
            AttackTimePatternD
        };
    }
    
    /// <summary>
    /// エネミーの攻撃 Aパターン
    /// </summary>
    public override void OnAttackPatternA()
    {
        // 8秒）12ダメージ
        Debug.Log("Aアタック");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 12, 0, 0));
    }

    /// <summary>
    /// エネミーの攻撃 Bパターン
    /// </summary>
    public override void OnAttackPatternB()
    {
        // 10秒）15ダメージ
        Debug.Log("Bアタック");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 15, 0, 0));
    }

    /// <summary>
    /// エネミーの攻撃 Cパターン
    /// </summary>
    public override void OnAttackPatternC()
    {
        // 5秒）強撃30％を3回得る
        Debug.Log("Cアタック");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.HardBlow, 0, 30, 3));
    }

    /// <summary>
    /// エネミーの攻撃 Dパターン
    /// </summary>
    public override void OnAttackPatternD()
    {
        // 8秒）5ダメージ
        Debug.Log("Dアタック");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 5, 0, 3));
    }
    
    /// <summary>
    /// エネミーの攻撃 Dパターン2
    /// </summary>
    public override void OnAttackPatternD2()
    {
        // 12秒）6ダメージ
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 6, 0, 0));
    }
}
