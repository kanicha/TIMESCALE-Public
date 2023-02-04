/// <summary>
/// プレイヤーイベント関係をまとめたクラス
/// </summary>
public sealed class PlayerEventList
{
    /// <summary>
    /// アクション関係をまとめたクラス
    /// </summary>
    public sealed class Action
    {
        /// <summary>
        /// 攻撃スキルを発動のイベントメッセージクラス
        /// </summary>
        public sealed class OnAttackSkill : EventMessage<OnAttackSkill>
        {
            
        }
        
        /// <summary>
        /// 防御スキルを発動したときのイベントメッセージクラス
        /// </summary>
        public sealed class OnGuardSkill : EventMessage<OnGuardSkill>
        {
            
        }
        
        /// <summary>
        /// 補助スキルを発動したときのイベントメッセージクラス
        /// </summary>
        public sealed class OnAssistSkill : EventMessage<OnAssistSkill>
        {
            
        }
        
        /// <summary>
        /// 複合スキルを発動したときのイベントメッセージクラス
        /// </summary>
        public sealed class OnCompositeSkill : EventMessage<OnCompositeSkill>
        {
            
        }
    }
}
