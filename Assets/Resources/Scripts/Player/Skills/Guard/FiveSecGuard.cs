/// <summary>
/// 5秒でガードするカード定義
/// </summary>
public class FiveSecGuard : GuardSkillBase
{
    // コンストラクタ
    FiveSecGuard()
    {
        skillType = StateList.SkillType.Guard;
        
        skillImageID = 2;
        
        skillName = "fiveSecGuard";
        skillTime = 5f;

        guardDamage = 10;
    }
}
