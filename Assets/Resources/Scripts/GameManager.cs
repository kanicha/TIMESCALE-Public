using System.Collections.Generic;
using UniRx;
using UnityEngine;

public partial class GameManager : SingletonMonoBehaviour<GameManager>
{
    [Header("import / インポート")]
    [SerializeField] private Timer _timer;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private PlayerManager _playerManager;
    

#if UNITY_EDITOR
    [SerializeField] private StateChecker _stateChecker;
    [Header("Debug / デバッグ")]
    [SerializeField, Tooltip("ステートのチェックを行うかどうか")] private bool _isStateCheck = false;
#endif
    
    // 生成するタイマーのリスト
    public float[] _timerList = new float[11];
    
    // 値監視を行うためのReactiveCollection(List)変数
    public ReactiveCollection<SkillBase> skillList = new ReactiveCollection<SkillBase>();
    // 仮配置用のリスト
    public ReactiveCollection<SkillBase> skillListReservation = new ReactiveCollection<SkillBase>();

    private IMessageBroker _broker;
    private void Start()
    {
        // BGMの再生
        SoundManager.Instance.PlayBGM(0);
        
        _broker = EventEmitter.Instance.Broker;

        // ターン変更のイベント受信
        _broker.Receive<EventList.GameSystem.TimerInit>().Subscribe(_ =>
        {
            // 配置されてたオブジェクトを初期化
            TurnChangeInit();
            // このタイミングでカードが切れていたら補充
            ReplaceSkill();
        }).AddTo(this);

        // エネミー死亡イベント受信
        _broker.Receive<EventList.GameSystem.EnemyDead>().Subscribe(_ =>
        {
            // エネミーが死んだらゲームクリアシーンに移行
            FadeController.Instance.LoadScene(0.5f, GameScene.GameClearScene);
            SoundManager.Instance.StopBGM();
        });
        
        // プレイヤー死亡イベント受信
        _broker.Receive<EventList.GameSystem.PlayerDead>().Subscribe(_ =>
        {
            // プレイヤーが死んだらゲームオーバー画面に移行
            FadeController.Instance.LoadScene(0.5f, GameScene.GameOverScene);
            SoundManager.Instance.StopBGM();
        });
    }
    
    private void Update()
    {
#if UNITY_EDITOR
        // ステートチェックのフラグがtrueだった場合コメントにステートチェックを行う
        if (_isStateCheck)
            _stateChecker.ViewState();
#endif
        
        // 初期化のフラグがあったら
        if (StateManager.HasFlag(StateList.PlayerState.Init))
        {
            GameInit();
            
            // 初期化が終わったらゲーム開始のステートを飛ばす
            EventEmitter.Instance.Broker.
                Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.GameStart, true));
        }
        
        _timer.ReduceTime();
    }
}
