/// <summary>
/// 補助スキルの基底クラス　SkillBaseClassを継承
/// </summary>
public class AssistSkillBase : SkillBase
{
    /// <summary>
    /// 補助を発動したときの処理
    /// </summary>
    public virtual void OnAssist()
    {
    }

    /// <summary>
    /// ジャスト発動したときの処理
    /// </summary>
    public virtual void OnAssistJust()
    {
        
    }
}