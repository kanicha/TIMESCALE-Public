using UnityEngine;

/// <summary>
/// パリィスキル定義
/// </summary>
public class ParryGuard : GuardSkillBase
{
    // コンストラクタ
    ParryGuard()
    {
        skillType = StateList.SkillType.Guard;
        
        skillImageID = 2;
        
        skillName = "ParryGuard";
        skillTime = 1f;
        buffName = StatusNames.BuffName.None;

        // シールド +2
        // just シールド + 4
        guardDamage = 2;
        guardDamageJust = 4;
    }

    public override void OnGuard()
    {
        Debug.Log("パリィ発動！");
        
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.ActiveGuard.
            GetEvent(GuardSkillName.Parry, buffName, guardDamage, 0, 0, false));
    }

    public override void OnGuardJust()
    {
        Debug.Log("パリィ発動ジャスト！");
        
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.ActiveGuard.
            GetEvent(GuardSkillName.Parry, buffName, guardDamageJust, 0, 0, true));
    }
}
