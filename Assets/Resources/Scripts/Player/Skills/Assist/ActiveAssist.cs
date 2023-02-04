/// <summary>
/// 活性スキル定義
/// </summary>
public class ActiveAssist : AssistSkillBase
{
    // コンストラクタ
    ActiveAssist()
    {
        skillType = StateList.SkillType.Assist;
        
        skillImageID = 3;

        skillName = "ActiveAssist";
        buffName = StatusNames.BuffName.HardBlow;
        skillTime = 3f;
        
        // 強撃1 1ターン
        // just 強撃 2ターン 
    }

    /// <summary>
    /// スキル発動処理
    /// </summary>
    public override void OnAssist()
    {
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem
            .ActiveAssist.GetEvent(AssistSkillName.Active, buffName, 1, 1, 0, false));
    }
    
    /// <summary>
    /// ジャスト発動したときの処理
    /// </summary>
    public override void OnAssistJust()
    {
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem
            .ActiveAssist.GetEvent(AssistSkillName.Active, buffName, 1, 2, 0, true));
    }
}