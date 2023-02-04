/// <summary>
/// 防御スキルの基底クラス　SkillBaseClassを継承
/// </summary>
public class GuardSkillBase : SkillBase
{
    protected int guardDamage; // 防御ダメージ
    protected int guardDamageJust; // ジャストの防御
    protected int counterDamage; // 反射した時のダメージ

    /// <summary>
    /// ガードしたときの処理
    /// </summary>
    public virtual void OnGuard()
    {
        
    }

    /// <summary>
    /// ジャスト発動した時の処理
    /// </summary>
    public virtual void OnGuardJust()
    {
        
    }
}
