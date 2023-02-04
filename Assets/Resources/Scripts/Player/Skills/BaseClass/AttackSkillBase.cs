/// <summary>
/// 攻撃スキルの基底クラス　SkillBaseClassを継承
/// </summary>
public class AttackSkillBase : SkillBase
{
    protected int attackDamage; // 攻撃ダメージ
    protected int attackDamageJust; // ジャストの攻撃ダメージ

    /// <summary>
    /// 攻撃したときの処理
    /// </summary>
    public virtual void OnAttack()
    {
        
    }

    /// <summary>
    /// ジャスト発動をしたときの処理
    /// </summary>
    public virtual void OnAttackJust()
    {
        
    }
}
