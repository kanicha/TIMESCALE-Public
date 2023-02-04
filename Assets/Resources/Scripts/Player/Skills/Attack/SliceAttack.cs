using UnityEngine;

/// <summary>
/// スライススキル定義
/// </summary>
public class SliceAttack : AttackSkillBase
{
    // コンストラクタ
    SliceAttack()
    {
        skillType = StateList.SkillType.Attack;
        
        skillImageID = 1;
        
        skillName = "SliceAttack";
        skillTime = 3f;
        
        attackDamage = 7;
        attackDamageJust = 10;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    public override void OnAttack()
    {
        Debug.Log("スライス発動");

        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAttack.GetEvent(AttackSkillName.Slice, attackDamage, false));
    }

    /// <summary>
    /// ジャストスキルの発動
    /// </summary>
    public override void OnAttackJust()
    {
        Debug.Log("スライス発動ジャスト");

        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAttack.GetEvent(AttackSkillName.Slice, attackDamageJust, true));
    }
}
