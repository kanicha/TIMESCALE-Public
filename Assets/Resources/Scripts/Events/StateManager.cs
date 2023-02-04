using System;
using UniRx;
using UnityEngine;

/// <summary>
/// イベントに対応した形でステート管理を行うクラス
/// </summary>
public class StateManager : MonoBehaviour
{
    private IMessageBroker _inputBroker;
    
    void Start()
    {
        StateChange(StateList.PlayerState.Init);
        
        _inputBroker = EventEmitter.Instance.Broker;
        StateEmitter();
    }

    /// <summary>
    /// ステートの変更を行う関数
    /// </summary>
    private void StateEmitter()
    {
        // ステート変更のイベントを受信したら
        _inputBroker.Receive<EventList.OnStateChangeRequest>().Subscribe(x =>
        {
            // trueだった場合
            if (x.IsAdd)
            {
                // 追加する際の例外処理
                switch (x.State)
                {
                    case StateList.PlayerState.None:
                        break;
                    case StateList.PlayerState.CatchSkill:
                        if (HasFlag(StateList.PlayerState.DetachSkill))
                            RemoveFlag(StateList.PlayerState.DetachSkill);
                        break;
                    case StateList.PlayerState.DetachSkill:
                        if (HasFlag(StateList.PlayerState.CatchSkill))
                            RemoveFlag(StateList.PlayerState.CatchSkill);
                        break;
                    case StateList.PlayerState.Init:
                        // 初期化を通るのでNoneを消す
                        if (HasFlag(StateList.PlayerState.None))
                            RemoveFlag(StateList.PlayerState.None);
                        break;
                    case StateList.PlayerState.GameStart:
                        // フラグに初期化があったら初期化を消す
                        if (HasFlag(StateList.PlayerState.Init))
                            RemoveFlag(StateList.PlayerState.Init);
                        break;
                    case StateList.PlayerState.Pause:
                        break;
                    case StateList.PlayerState.TimerStart:
                        if (HasFlag(StateList.PlayerState.TimerStop))
                            RemoveFlag(StateList.PlayerState.TimerStop);
                        break;
                    case StateList.PlayerState.TimerStop:
                        if (HasFlag(StateList.PlayerState.TimerStart))
                            RemoveFlag(StateList.PlayerState.TimerStart);
                        break;
                    case StateList.PlayerState.PlayerAttack:
                        break;
                    case StateList.PlayerState.EnemyAttack:
                        break;
                    case StateList.PlayerState.PlayerDead:
                        break;
                    case StateList.PlayerState.EnemyDead:
                        break;
                    case StateList.PlayerState.StandbySkill:
                        if (HasFlag(StateList.PlayerState.ReadySkill))
                            RemoveFlag(StateList.PlayerState.ReadySkill);
                        break;
                    case StateList.PlayerState.ReadySkill:
                        if (HasFlag(StateList.PlayerState.StandbySkill))
                            RemoveFlag(StateList.PlayerState.StandbySkill);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                // 例外処理を通ったらフラグの追加
                StateChange(x.State);
            }
            // falseだった場合削除
            else if (!x.IsAdd)
            {
                RemoveFlag(x.State);
            }
        }).AddTo(this);
    }

    /// <summary>
    /// ステートの変更を行う関数
    /// </summary>
    /// <param name="state"> 変更を行うステート先 </param>
    private void StateChange(StateList.PlayerState state)
    {
        AddFlag(state);
    }

    /// <summary>
    /// ステートの変数に指定のステートが存在しているかを判別するbool関数
    /// </summary>
    /// <param name="state"> 検知したいステート </param>
    /// <returns> 指定したステートが存在している </returns>
    public static bool HasFlag(StateList.PlayerState state)
    {
        return StateList.playerState.HasFlag(state);
    }

    /// <summary>
    /// ステートの変数に指定のステートを追加する関数
    /// </summary>
    /// <param name="state"></param>
    private void AddFlag(StateList.PlayerState state)
    {
        StateList.playerState |= state;
    }
    
    /// <summary>
    /// ステートの変数に指定のステートを削除する関数
    /// </summary>
    /// <param name="state"></param>
    private void RemoveFlag(StateList.PlayerState state)
    {
        StateList.playerState &= ~state;
    }
}
