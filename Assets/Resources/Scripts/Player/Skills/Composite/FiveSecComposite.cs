/// <summary>
/// 5秒で発動する複合スキル定義
/// </summary>
public class FiveSecComposite : CompositeSkillBase
{
    FiveSecComposite()
    {
        skillImageID = 4;
        
        skillName = "fiveSecComposite";
        skillTime = 5f;

        attackDamage = 5;
        guardDamage = 10;
    }
}
