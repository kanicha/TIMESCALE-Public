using UnityEngine;

/// <summary>
/// ガードスキル定義
/// </summary>
public class Guard : GuardSkillBase
{
    // コンストラクタ
    Guard()
    {
        skillType = StateList.SkillType.Guard;
        
        skillImageID = 2;
        
        skillName = "Guard";
        skillTime = 3f;
        
        guardDamage = 7;
        guardDamageJust = 10;
    }

    public override void OnGuard()
    {
        Debug.Log("ガード発動！");
        
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.ActiveGuard.
            GetEvent(GuardSkillName.Guard, StatusNames.BuffName.None, guardDamage, 0, 0,false));
    }

    public override void OnGuardJust()
    {
        Debug.Log("ガード発動ジャスト！");
        
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.ActiveGuard.
            GetEvent(GuardSkillName.Guard, StatusNames.BuffName.None, guardDamageJust, 0, 0,true));
    }
}
