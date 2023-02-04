using UnityEngine;

/// <summary>
/// 気合スキル定義
/// </summary>
public class SpiritAssist : AssistSkillBase
{
    // コンストラクタ
    SpiritAssist()
    {
        skillType = StateList.SkillType.Assist;
        
        skillImageID = 3;

        skillName = "SpiritAssist";
        skillTime = 2f;
        buffName = StatusNames.BuffName.HardDefense;

        // 強防50% 1
        // just 強防75% 1 
    }

    public override void OnAssist()
    {
        Debug.Log("気合発動！");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAssist.GetEvent(AssistSkillName.Spirit, buffName,50, 0, 1, false));
    }

    public override void OnAssistJust()
    {
        Debug.Log("気合発動ジャスト!");
        EventEmitter.Instance.Broker.Publish(EventList.GameSystem.
            ActiveAssist.GetEvent(AssistSkillName.Spirit, buffName, 75, 0, 1, true));
    }
}