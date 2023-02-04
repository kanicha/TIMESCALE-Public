using UnityEngine;

/// <summary>
/// 心身調整スキル定義
/// </summary>
public class MindAssist : AssistSkillBase
{
    // コンストラクタ
    MindAssist()
    {
        skillType = StateList.SkillType.Assist;
        
        skillImageID = 3;

        skillName = "MindAssist";
        skillTime = 1f;
        buffName = StatusNames.BuffName.None;

        // このスキルの前に配置されたスキルを補充
        // just このスキルと1こまえのスキルを補充
    }

    public override void OnAssist()
    {
        Debug.Log("心身調整発動！");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem
            .ActiveAssist.GetEvent(AssistSkillName.Mind, buffName, 3, 0, 0, false));
    }

    public override void OnAssistJust()
    {
        Debug.Log("心身調整発動ジャスト！");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem
            .ActiveAssist.GetEvent(AssistSkillName.Mind, buffName, 5, 0, 0, true));
    }
}