/// <summary>
/// 3秒で攻撃するカード定義
/// </summary>
public class ThreeSecAttack : AttackSkillBase
{
    // コンストラクタ
    ThreeSecAttack()
    {
        skillType = StateList.SkillType.Attack;
        
        skillImageID = 1;
        
        skillName = "3secAttack";
        skillTime = 3f;

        attackDamage = 15;
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    public override void OnAttack()
    {
        
    }
}
