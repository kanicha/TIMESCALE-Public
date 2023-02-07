using UnityEngine;
using UniRx;

/// <summary>
/// UI管理マネージャークラス
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private TimerUI _timer;
    [SerializeField] private Bar _bar;
    [SerializeField] private SkillToGage _skillToGage;
    [SerializeField] private PlayerHPUI _playerHpUI;
    [SerializeField] private EnemyHPUI _enemyHpUI;
    [SerializeField] private BuffIconUI _buffIconUI;
    [SerializeField] private EnemyAttackTimingUI _enemyAttackUI;
    [SerializeField] private EnemyDebugUI _enemyUI;
    
    private IMessageBroker _broker;
    
    private void Start()
    {
        _broker = EventEmitter.Instance.Broker;
        
        // タイマースタートイベント受信処理
        _broker.Receive<EventList.GameSystem.TimeScaleBarStart>()
            .Subscribe(x =>
            {
                _bar.MovingBar(x.NowTimer);
                _bar.BarLine();
            })
            .AddTo(this);

        // タイマー終了のイベント処理
        _broker.Receive<EventList.GameSystem.TimerInit>()
            .Subscribe(x =>
            {
                _bar.InitBar();
            }).AddTo(this);

        _broker.Receive<EventList.GameSystem.StandbyEnemyAttack>()
            .Subscribe(x =>
            {
                _enemyAttackUI.ShowAttackIcon(x.AttackWaitTime, x.isAttack);
            }).AddTo(this);

        _broker.Receive<EventList.UI.AddBuff>().Subscribe(x =>
        {
            if (x.isTrue)
            {
                if (x.isPlayer)
                    _buffIconUI.ShowBuffIconPlayer(x._buffName);
                else
                    _buffIconUI.ShowBuffIconEnemy(x._buffName);
            }
            else
            {
                /*SoundManager.Instance.PlaySE(6);*/
                
                if (x.isPlayer)
                 _buffIconUI.DeleteBuffIconPlayer(x._buffName);
                else
                 _buffIconUI.DeleteBuffIconEnemy(x._buffName);
            }
        }).AddTo(this);

        // skillListの配列が新しく追加されたとき
        GameManager.Instance.skillList.ObserveAdd().Subscribe(x =>
        {
            // 配置されたら仮のやつを消す
            _skillToGage.ReservationObjDelete();
            
            // ゲージ追加処理
            _skillToGage.ToGage(x.Index);
            
        }).AddTo(this);

        // Skillのバーが仮に配置された時
        GameManager.Instance.skillListReservation.ObserveAdd().Subscribe(x =>
        {
            _skillToGage.ToGageReservation(x.Index);
        }).AddTo(this);
        
        GameManager.Instance.skillListReservation.ObserveRemove().Subscribe(x =>
        {
            _skillToGage.ReservationObjDelete();
        }).AddTo(this);
        
        //プレイヤーのHPの変更が行われた時
        this.ObserveEveryValueChanged(x => PlayerManager.playerHP)
            .Where(_ => !StateManager.HasFlag(StateList.PlayerState.Init))
            .Subscribe(_ =>
            {
                _playerHpUI.ShowHP();
                _playerHpUI.SliderHPViewUI();
            }).AddTo(this);
        
        // エネミーのHP変更が行われた時
        this.ObserveEveryValueChanged(x => EnemyManager.enemyHP)
            .Where(_ => !StateManager.HasFlag(StateList.PlayerState.Init))
            .Subscribe(_ =>
            {
                _enemyHpUI.InitShowHP();
                _enemyHpUI.ShowHP();
                _enemyHpUI.EnemySliderHPViewUI();
            }).AddTo(this);
        
        // プレイヤーが攻撃した時
        _broker.Receive<EventList.GameSystem.ActiveAttack>()
            .Subscribe(x =>
            {
                _enemyHpUI.ShowDamageUI(x.AttackDamage);
            }).AddTo(this);
        
        // エネミーが攻撃した時
        _broker.Receive<EventList.GameSystem.EnemyAttack>()
            .Subscribe(x =>
            {
                _playerHpUI.ShowDamageUI(x.AttackDamage);
            }).AddTo(this);
    }

    private void Update()
    {
        _timer.TimerUIDraw();
        _enemyUI.ShowEnemyName();
    }
}