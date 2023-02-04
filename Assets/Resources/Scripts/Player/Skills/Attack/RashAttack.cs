using UnityEngine;

/// <summary>
/// ラッシュスキル定義
/// </summary>
public class RashAttack : AttackSkillBase
{
    // コンストラクタ
    RashAttack()
    {
        skillType = StateList.SkillType.Attack;
        
        skillImageID = 1;
        
        skillName = "RashAttack";
        skillTime = 5f;

        // 3 * 5
        // just 3 * 6
        attackDamage = 15;
        attackDamageJust = 12;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    public override void OnAttack()
    {
        Debug.Log("ラッシュアタック！");

        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAttack.GetEvent(AttackSkillName.Rash, attackDamage, false));
    }

    public override void OnAttackJust()
    {
        Debug.Log("ラッシュアタックジャスト！");

        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAttack.GetEvent(AttackSkillName.Rash, attackDamageJust, true));
    }
}
