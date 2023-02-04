/// <summary>
/// 3秒で補助をするカード定義
/// </summary>
public class ThreeSecAssist : AssistSkillBase
{
    // コンストラクタ
    ThreeSecAssist()
    {
        skillType = StateList.SkillType.Assist;
        
        skillImageID = 3;

        skillName = "threeSecAssist";
        skillTime = 3f;
    }
}
