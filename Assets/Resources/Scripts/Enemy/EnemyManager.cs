using UnityEngine;
using UniRx;

/// <summary>
/// エネミーの管理を行うマネージャークラス
/// </summary>
public class EnemyManager : MonoBehaviour
{
    private static EnemyBase _enemy;
    // ゲッター
    public static EnemyBase Enemy => _enemy;

    [SerializeField, Tooltip("出現予定のエネミー配列変数")]
    private EnemyBase[] _enemys;
    [SerializeField, Tooltip("エネミーの攻撃処理クラス")]
    private EnemyAttackEmitter _enemyAttackEmitter;
    
    public static int enemyHP = 0;
    public static int enemyDefaultHP = 0;
    public static int enemyShield = 0;

    private IMessageBroker _broker;
    private void Start()
    {
        _broker = EventEmitter.Instance.Broker;
        
        _broker.ObserveEveryValueChanged(_ => enemyHP)
            .Where(_ => enemyHP <= 0 )
            .Subscribe(_ =>
            {
                EnemyDeathSendEvent();
            }).AddTo(this);

        // ターンが変わったら
        _broker.Receive<EventList.GameSystem.TimeScaleBarStart>()
            .Where(_ => !StateManager.HasFlag(StateList.PlayerState.Init))
            .Subscribe(_ =>
        {
            _enemyAttackEmitter.EnemyAttackPatternEmit(_enemy);
        });
    }

    /// <summary>
    /// 引数で渡された値でモンスターの分岐と初期化
    /// </summary>
    /// <param name="enemyNames"> 選定されたモンスター </param>
    public void EnemyEmitter(EnemyName enemyNames)
    {
        _enemy = _enemys[(int) enemyNames];
        
        // エネミーのHP初期化
        enemyHP = _enemy.EnemyHP;
        enemyDefaultHP = _enemy.EnemyHP;
        
        Debug.Log("エネミーの名前" + _enemy.EnemyName);
    }
    
    /// <summary>
    /// エネミーのHPを見てHPが0以下だったらイベントを飛ばす
    /// </summary>
    /// <returns></returns>
    private void EnemyDeathSendEvent()
    {
      // 死んだのでステートをつけ、イベントの発行
      _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.EnemyDead, true));
      _broker.Publish(EventList.GameSystem.EnemyDead.GetEvent());
    }
}
