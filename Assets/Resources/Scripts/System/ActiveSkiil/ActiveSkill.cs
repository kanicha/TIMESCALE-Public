using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// スキル発動を確認し実際に発動するクラス
/// </summary>
public partial class ActiveSkill : MonoBehaviour
{
    // 発動待ちスキルのリスト
    private List<SkillBase> _standbySkills = new List<SkillBase>();

    // 発動済みスキルのリスト
    private List<SkillBase> _activatedSkills = new List<SkillBase>();
    public static List<SkillBase> _readySkillList = new List<SkillBase>();

    private IMessageBroker _broker;

    void Start()
    {
        _broker = EventEmitter.Instance.Broker;

        // skillListの配列が新しく追加されたとき
        GameManager.Instance.skillList.ObserveAdd().Subscribe(x => { AddSkillList(x.Value); }).AddTo(this);

        // スキルの発動処理
        _broker.Receive<EventList.GameSystem.ActivatedSkill>().Subscribe(_ => { ActiveSkillNext(); })
            .AddTo(this);

        // ターンが変更されたら
        _broker.Receive<EventList.GameSystem.TimerInit>().Subscribe(_ => { TurnChangeProcessing(); }).AddTo(this);

        // 各スキルのイベント受信処理 //
        /*_var disposable = new SingleAssignmentDisposable();
            broker.Receive<EventList.GameSystem.ActiveSetAttackSkill>()
            .Where(_ => StateManager.HasFlag(StateList.PlayerState.StandbySkill))
            .Subscribe(x =>
            {
                disposable.Disposable = _broker.ObserveEveryValueChanged(_ => Timer._intCountNowTimer).Subscribe(y =>
                {
                    if (AttackSkillActiveChecker(x.Skill, x.ActivatedSkillTime, y))
                    {
                        // 処理が終わったら購読終了
                        disposable.Dispose();
                    }
                });
            }).AddTo(this);*/

        _broker.Receive<EventList.GameSystem.ActiveSetAttackSkill>()
            .Where(_ => StateManager.HasFlag(StateList.PlayerState.StandbySkill))
            .Subscribe(x =>
            {
                _broker.ObserveEveryValueChanged(_ => Timer._floatCountNowTimer)
                    .TakeWhile(y => !AttackSkillActiveChecker(x.Skill, x.ActivatedSkillTime, y))
                    .Subscribe(_ => { });
            }).AddTo(this);

        _broker.Receive<EventList.GameSystem.ActiveSetAssistSkill>()
            .Where(_ => StateManager.HasFlag(StateList.PlayerState.StandbySkill))
            .Subscribe(x =>
            {
                _broker.ObserveEveryValueChanged(_ => Timer._floatCountNowTimer)
                    .TakeWhile(y => !AssistSkillActiveChecker(x.Skill, x.ActivatedSkillTime, y))
                    .Subscribe(_ => { });
            }).AddTo(this);

        _broker.Receive<EventList.GameSystem.ActiveSetGuardSkill>()
            .Where(_ => StateManager.HasFlag(StateList.PlayerState.StandbySkill))
            .Subscribe(x =>
            {
                _broker.ObserveEveryValueChanged(_ => Timer._floatCountNowTimer)
                    .TakeWhile(y => !GuardSkillActiveChecker(x.Skill, x.ActivatedSkillTime, y))
                    .Subscribe(_ => { });
            }).AddTo(this);
    }

    /// <summary>
    /// skillBaseで渡された値をリストに追加
    /// </summary>
    /// <param name="skillBase"> 発動しようとしているスキル </param>>
    private void AddSkillList(SkillBase skillBase)
    {
        bool isActive = false;
        
        // 引数でもらった値をリストに追加する
        _standbySkills.Add(skillBase);

        // Readyステートがあったらかつリストに追加されていたら
        if (StateManager.HasFlag(StateList.PlayerState.ReadySkill))
        {
            // スキルの発動処理関数を呼ぶ
            SkillEmitter(false);
        }
    }

    /// <summary>
    /// スキルの種類で関数の分岐
    /// </summary>
    /// <param name="isJust"> 最後の一つかどうか </param>>
    private void SkillEmitter(bool isJust)
    {
        // ステートをつける (StandbySkill)
        _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.StandbySkill, true));
        _readySkillList.Add(_standbySkills[0]);

        // リストの0番目を見てスキルの種類でキャストをし処理関数を呼ぶ
        switch (_standbySkills[0].skillType)
        {
            case StateList.SkillType.None:
                break;
            case StateList.SkillType.Assist:
                CorrectionAssistSkill(_standbySkills[0] as AssistSkillBase, isJust);
                break;
            case StateList.SkillType.Attack:
                CorrectionAttackSkill(_standbySkills[0] as AttackSkillBase, isJust);
                break;
            case StateList.SkillType.Guard:
                CorrectionGuardSkill(_standbySkills[0] as GuardSkillBase, isJust);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// 次の発動にむけ処理を行う
    /// </summary>
    private void ActiveSkillNext()
    {
        // 分岐が終わったらリストから削除
        // 分岐済みスキルを発動済みリストに追加
        _activatedSkills.Add(_standbySkills[0]);
        // スキル発動が終わったらリストから削除する
        _standbySkills.RemoveAt(0);

        // カウントが残っているときでも スキルの合計時間が超えてしまうならここの処理は通さない
        if (_standbySkills.Count > 0)
        {
            if (!IsSkillTimeOver())
            {
                return;
            }

            // 次のスキル発動準備
            SkillEmitter(false);
        }
    }

    /// <summary>
    /// ターンが変更されたら処理を行う
    /// </summary>
    private void TurnChangeProcessing()
    {
        // スタンバイスキルに1個でも残っていた場合
        if (_standbySkills.Count > 0)
        {
            // そのスキルを即時発動
            SkillEmitter(true);
        }

        // 各情報のクリア
        _standbySkills.Clear();
        _activatedSkills.Clear();
        _readySkillList.Clear();
    }

    /// <summary>
    /// スキルの合計時間が超えているかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsSkillTimeOver()
    {
        float addSkillTime = 0f;
        float correctionTime = 0f;

        foreach (var skills in _activatedSkills)
        {
            addSkillTime += skills.skillTime;
        }

        correctionTime = _activatedSkills[0].skillTime + addSkillTime;

        return correctionTime < Timer._intNowTimerLength;
    }
}