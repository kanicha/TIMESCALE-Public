/// <summary>
/// 複合スキルの基底クラス SkillBaseClassを継承
/// </summary>
public class CompositeSkillBase : SkillBase
{
    // 攻撃
    protected int attackDamage; // 攻撃ダメージ
    
    // 防御
    protected int guardDamage; // 防御ダメージ
    protected int counterDamage; // 反射した時のダメージ

    // 補助


    /// <summary>
    /// 発動したときの処理
    /// </summary>
    public virtual void OnComposite()
    {
        
    }
}