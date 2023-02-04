using UnityEngine;

/// <summary>
/// スティングスキル定義
/// </summary>
public class StingAttack : AttackSkillBase
{
    // コンストラクタ
    StingAttack()
    {
        skillType = StateList.SkillType.Attack;
        
        skillImageID = 1;
        
        skillName = "StingAttack";
        skillTime = 5f;
        
        attackDamage = 17;
        attackDamageJust = 23;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    public override void OnAttack()
    {
        Debug.Log("スティングアタック！");
        
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAttack.GetEvent(AttackSkillName.String, attackDamage, false));
    }

    public override void OnAttackJust()
    {
        Debug.Log("スティングアタックジャスト！");

        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAttack.GetEvent(AttackSkillName.String, attackDamageJust, true));
    }
}