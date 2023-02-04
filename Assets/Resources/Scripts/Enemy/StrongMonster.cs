using System.Collections.Generic;

public class StrongMonster : EnemyBase
{
    StrongMonster()
    {
        EnemyID = 2;
        
        EnemyName = "ちょっと強いモンスター";
        EnemyHP = 800;

        /*
        AttackTimePatternA.Add(8);
        AttackTimePatternB.Add(10);
        AttackTimePatternC.Add(6);
        AttackTimePatternC.Add(10);
        AttackTimePatternD.Add(6);*/
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
        // 8秒) 18ダメージ
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 18, 0, 0));
    }

    /// <summary>
    /// エネミーの攻撃 Bパターン
    /// </summary>
    public override void OnAttackPatternB()
    {
        // 10秒）20ダメージ
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 20, 0, 0));
    }

    /// <summary>
    /// エネミーの攻撃 Cパターン
    /// </summary>
    public override void OnAttackPatternC()
    {
        // 6秒）シールド10を得る
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.Shield, 0, 10, 0));
    }

    /// <summary>
    /// エネミーの攻撃 Cパターン2
    /// </summary>
    public override void OnAttackPatternC2()
    {
        // 10秒）10ダメージ
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 10, 0, 0));
    }

    /// <summary>
    /// エネミーの攻撃 Dパターン
    /// </summary>
    public override void OnAttackPatternD()
    {
        // 6秒）5ダメージ
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            EnemyAttackCal.GetEvent(StatusNames.BuffName.None, 5, 0, 0));;
    }
}
