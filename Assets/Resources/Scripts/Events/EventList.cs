using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// ゲームのイベント関係をまとめたクラス
/// </summary>
public sealed class EventList
{
    /// <summary>
    /// UI関係のイベントメッセージクラス
    /// </summary>
    public sealed class UI
    {
        /// <summary>
        /// UIをバックした時のイベントメッセージ
        /// </summary>
        public sealed class OnBack : EventMessage<OnBack>
        {
        }
        
        /// <summary>
        /// タイマーのアニメーション再生イベントメッセージ
        /// </summary>
        public sealed class TimerAnim : EventMessage<TimerAnim>
        {
        }
        
        /// <summary>
        /// バフを追加したときのイベント 第一引数: 追加したバフ 第二引数: true:追加 false:削除 第三引数: プレイヤーかどうか
        /// </summary>
        public sealed class AddBuff : EventMessage<AddBuff, StatusNames.BuffName, bool, bool>
        {
            public StatusNames.BuffName _buffName => param1;
            public bool isTrue => param2;
            public bool isPlayer => param3;
        }
    }
    
    /// <summary>
    /// ゲームシステム関係のイベントメッセージクラス
    /// </summary>
    public sealed class GameSystem
    {
        /// <summary>
        /// ポーズをしたときのイベントメッセージ
        /// </summary>
        public sealed class Pause : EventMessage<Pause>
        {
            
        }
        
        /// <summary>
        /// エネミーが攻撃準備を行っている時のイベントメッセージ
        /// 第一引数: エネミーベース
        /// 第二引数: 抽選したスキル
        /// </summary>
        public sealed class StandbyEnemyAttack : EventMessage<StandbyEnemyAttack, EnemyBase, int, List<float>>
        {
            public EnemyBase EnemyBase => param1;
            public int SelectNum => param2;
            public List<float> AttackWaitTime => param3;
        }
        
        /// <summary>
        /// エネミーが死んだときのイベントメッセージ
        /// </summary>
        public sealed class EnemyDead : EventMessage<EnemyDead>
        {
            
        }
        
        /// <summary>
        /// プレイヤーが死んだときのイベントメッセージ
        /// </summary>
        public sealed class PlayerDead : EventMessage<PlayerDead>
        {
            
        }
        
        /// <summary>
        /// スキルを生成する時用のイベントメッセージ
        /// </summary>
        public sealed class GenerateSkill : EventMessage<GenerateSkill>
        {
            
        }
        
        /// <summary>
        /// スキルを再生成を行うときのイベントメッセージ
        /// </summary>
        public sealed class ReGenerateSkill : EventMessage<ReGenerateSkill, int>
        {
            public int SkillNum => param1;
        }
        
        /// <summary>
        /// スキルを配置した時用のイベントメッセージ
        /// 第一引数:置いたスキルのスキルの情報(クラス)
        /// 第二引数:生成しようとしているスキルのID
        /// </summary>
        public sealed class SetSkilled : EventMessage<SetSkilled, SkillBase, int>
        {
            public SkillBase SkillBase => param1;
            public int SkillNum => param2;
        }

        /// <summary>
        /// スキルを持っているときのイベントメッセージ
        /// </summary>
        public sealed class SkillCatch : EventMessage<SkillCatch, GameObject>
        {
            public GameObject catchObject => param1;
        }
        
        /// <summary>
        /// スキルを離したときのイベントメッセージ
        /// </summary>
        public sealed class SkillDeCatch : EventMessage<SkillDeCatch, GameObject>
        {
            public GameObject catchObject => param1;
        }
        
        /// <summary>
        /// タイマーを生成する時のイベントメッセージ
        /// </summary>
        public sealed class TimerCreate : EventMessage<TimerCreate>
        {
            
        }
        
        
        /// <summary>
        /// タイムスケールスタート開始時のイベントメッセージ NowTimerとNextTimerをもっている
        /// </summary>
        public sealed class TimeScaleBarStart : EventMessage<TimeScaleBarStart, int, int>
        {
            public int NowTimer => param1;
            public int NextTimer => param2;
        }
        
        /// <summary>
        /// タイマー終了時のイベントメッセージ
        /// </summary>
        public sealed class TimerInit : EventMessage<TimerInit>
        {
            
        }

        /// <summary>
        /// エネミー割当終了後のメッセージ
        /// </summary>
        public sealed class EnemyInit : EventMessage<EnemyInit>
        {
            
        }
        
        /// <summary>
        /// スキル発動完了イベント
        /// </summary>
        public sealed class ActivatedSkill : EventMessage<ActivatedSkill>
        {
        }
        
        /// <summary>
        /// スキルの値調整が終わった時に通知するイベント 攻撃
        /// 第一引数: 発動しようとしているスキル 第二引数: 過去に発動したスキルの値
        /// </summary>
        public sealed class ActiveSetAttackSkill : EventMessage<ActiveSetAttackSkill,AttackSkillBase, float>
        {
            public AttackSkillBase Skill => param1;
            public float ActivatedSkillTime => param2;
        }
        
        /// <summary>
        /// スキルの値調整が終わった時に通知するイベント アシスト
        /// 第一引数: 発動しようとしているスキル 第二引数: 過去に発動したスキルの値
        /// </summary>
        public sealed class ActiveSetAssistSkill : EventMessage<ActiveSetAssistSkill,AssistSkillBase, float>
        {
            public AssistSkillBase Skill => param1;
            public float ActivatedSkillTime => param2;
        }
        
        /// <summary>
        /// スキルの値調整が終わった時に通知するイベント 攻撃
        /// 第一引数: 発動しようとしているスキル 第二引数: 過去に発動したスキルの値
        /// </summary>
        public sealed class ActiveSetGuardSkill : EventMessage<ActiveSetGuardSkill,GuardSkillBase, float>
        {
            public GuardSkillBase Skill => param1;
            public float ActivatedSkillTime => param2;
        }
        
        /// <summary>
        /// 攻撃発動を行うときのイベント
        /// 第一引数: 発動しようとしているスキルの名 第二引数: 攻撃力 第三引数: ジャストかどうか
        /// </summary>
        public sealed class ActiveAttack : EventMessage<ActiveAttack, AttackSkillName, int, bool>
        {
            public AttackSkillName SkillName => param1;
            public int AttackDamage => param2;
            public bool IsJust => param3;
        }
        
        /// <summary>
        /// アシスト発動を行うときのイベント
        /// 第一引数: 発動しようとしているスキルの名 第二引数: 効果能力値 第三引数: 効果時間  第四引数: 効果回数 第五引数: ジャストかどうか
        /// </summary>
        public sealed class ActiveAssist : EventMessage<ActiveAssist, AssistSkillName, StatusNames.BuffName, float, float, int, bool>
        {
            public AssistSkillName SkillName => param1;
            public StatusNames.BuffName BuffName => param2;
            public float Num => param3;
            public float Time => param4;
            public int Count => param5;
            public bool IsJust => param6;
        }
        
        /// <summary>
        /// アシスト発動を行うときのイベント
        /// 第一引数: 発動しようとしているスキルの名 第二
        /// 引数:スキルの効果 第二引数: 効果時間 第三引数: 効果能力値
        /// </summary>
        public sealed class ActiveGuard : EventMessage<ActiveGuard, GuardSkillName, StatusNames.BuffName ,float, float, int, bool>
        {
            public GuardSkillName SkillName => param1;
            public StatusNames.BuffName BuffName => param2;
            public float Num => param3;
            public float Time => param4;
            public int Count => param5;
            public bool IsJust => param6;
        }

        /// <summary>
        /// エネミーが攻撃する時の計算呼び出しイベント
        /// </summary>
        public sealed class EnemyAttackCal : EventMessage<EnemyAttackCal, StatusNames.BuffName, int, float, int>
        {
            public StatusNames.BuffName BuffName => param1;
            public int AttackDamage => param2;
            public float AbilityNum => param3;
            public int Count => param4;
        }
        
        public sealed class EnemyAttack : EventMessage<EnemyAttack>
        {
            
        }
    }
    
    /// <summary>
    /// ステート変更を行うイベントメッセージ
    /// </summary>
    public sealed class OnStateChangeRequest : EventMessage<OnStateChangeRequest, StateList.PlayerState, bool>
    {
        public StateList.PlayerState State => param1; // 変更先のステート
        public bool IsAdd => param2; // ステートを追加するかどうか true:追加 false:削除
    }
}
