using UnityEngine;
using UniRx;

/// <summary>
/// プレイヤーの管理を行うマネージャークラス
/// </summary>
public class PlayerManager : MonoBehaviour
{
    // プレイヤーのHP
    public static int playerHP = PlayerConstParams.DEFAULT_HP;
    public static int playerDefaultHP = PlayerConstParams.DEFAULT_HP;
    // プレイヤーのシールド
    public static int playerShield = 0;
    
    private IMessageBroker _broker;

    private void Start()
    {
        _broker = EventEmitter.Instance.Broker;
        
        _broker.ObserveEveryValueChanged(_ => playerHP)
            .Where(_ => playerHP <= 0 )
            .Subscribe(_ =>
            {
                PlayerDeathChecker();
            }).AddTo(this);
    }

    /// <summary>
    /// プレイヤーが死んだ時
    /// </summary>
    private void PlayerDeathChecker()
    {
        _broker.Publish(EventList.GameSystem.PlayerDead.GetEvent());
        _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.PlayerDead, true));
    }
}
