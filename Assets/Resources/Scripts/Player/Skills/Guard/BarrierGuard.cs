using UnityEngine;

/// <summary>
/// バリアスキル定義
/// </summary>
public class BarrierGuard : GuardSkillBase
{
    // コンストラクタ
    BarrierGuard()
    {
        skillType = StateList.SkillType.Guard;
        
        skillImageID = 2;
        
        skillName = "BarrierGuard";
        skillTime = 2f;

        buffName = StatusNames.BuffName.Invincible;
        
        // 無敵 1 1ターン
        // just 無敵 1 2ターン
        guardDamage = 10;
    }

    public override void OnGuard()
    {
        Debug.Log("バリア発動！");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.ActiveGuard.
            GetEvent(GuardSkillName.Barrier, buffName, 1, 0, 1, false));
    }

    public override void OnGuardJust()
    {
        Debug.Log("バリア発動ジャスト！");
        
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.ActiveGuard.
            GetEvent(GuardSkillName.Barrier, buffName, 2, 0, 1, true));
    }
}
