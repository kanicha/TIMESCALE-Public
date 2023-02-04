using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UniRx;

/// <summary>
/// エネミーの攻撃関連クラス
/// </summary>
public class EnemyAttackEmitter : MonoBehaviour
{
    private IMessageBroker _broker;
    private bool _isSecond = false;
    private bool _isActive = false;
    private bool _isWaitAttack = false; 
    private int _tempNum = 0;
    
    // 何の攻撃を行うかのランダム値を入れる変数
    private int _randomAttackNum = 0;
    private List<float> _attackWaitTimeList = new List<float>();
    
    private void Start()
    {
        _broker = EventEmitter.Instance.Broker;

        _broker.Receive<EventList.GameSystem.StandbyEnemyAttack>()
            .Subscribe(_ =>
            {
                _isWaitAttack = true;
            }).AddTo(this);
        
        _broker.ObserveEveryValueChanged(_ => Timer._intCountNowTimer)
            .Where(_ => _isWaitAttack && !_isActive)
            /*.Where(_ => !StateManager.HasFlag(StateList.PlayerState.EnemyAttack))*/
            .Subscribe(_ =>
            { 
                AttackActive(EnemyManager.Enemy, _randomAttackNum , Timer._intCountNowTimer);
            }).AddTo(this);
        
        _broker.Receive<EventList.GameSystem.TimerCreate>()
            .Subscribe(_ =>
            {
                _attackWaitTimeList.Clear();
                _isActive = false;
                _isWaitAttack = false;
                _isSecond = false;
            }).AddTo(this);
    }
    
    /// <summary>
    /// エネミーの攻撃が何の攻撃をするか、番号を選定する
    /// </summary>
    /// <param name="enemyBase"></param>
    public void EnemyAttackPatternEmit(EnemyBase enemyBase)
    {
        do
        {
            // 攻撃パターンを選ぶため、ランダムの値を選定する (0 ~ 3)
            _randomAttackNum = Random.Range(0, 4);
            // もし、一個前に選んだ値と同じだった場合違う値が出るまで抽選
        } while (_tempNum == _randomAttackNum);
        
        // ループを抜けたら一回値を tempNum に保持
        _tempNum = _randomAttackNum;
        
        // 抽選された番号とエネミーのリストを見る
        foreach (var choicePattern in enemyBase.AttackTimes[_randomAttackNum])
        {
            // 選ばれたスキルの発動時間を、メンバー変数のリストに入れる
            _attackWaitTimeList.Add(choicePattern);
        }

        // 表示するアイコンの種類の選定を行う
        if (_randomAttackNum != 2)
            _broker.Publish(EventList.GameSystem.StandbyEnemyAttack.GetEvent(enemyBase, _randomAttackNum, _attackWaitTimeList, true));
        else 
            _broker.Publish(EventList.GameSystem.StandbyEnemyAttack.GetEvent(enemyBase, _randomAttackNum, _attackWaitTimeList, false));
    }
     
    /// <summary>
    /// 発動処理
    /// </summary>
    /// <param name="enemyBase"> 発動するエネミーのスクリプト </param>
    /// <param name="selectNum"> 選ばれた番号 </param>
    /// <param name="timer"> 時間 </param>
    private void AttackActive(EnemyBase enemyBase ,int selectNum,int timer)
    {
        _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.EnemyAttack, true));

        // randomAttackNum = 攻撃するパターンの番号 0,1,2,3
        // _attackWaitTimeList = 攻撃を行う時間

        // 引数で渡された時間と選定されたスキルのリストを見る
        if (timer == (int)_attackWaitTimeList[0])
        {
            Debug.LogWarning("行動！");
            // 選定された番号で発動スキルを決める
            switch (selectNum)
            {
                case 0:
                    enemyBase.OnAttackPatternA();
                    break;
                case 1:
                    enemyBase.OnAttackPatternB();
                    break;
                case 2:
                    enemyBase.OnAttackPatternC();
                    break;
                case 3:
                    enemyBase.OnAttackPatternD();
                    break;
                default:
                    break;
            }
            
            if (_attackWaitTimeList.Count <= 1)
            {
                _isActive = true;
            }
            else
            {
                // もし2回めの行動が時間を超えてたら終了する
                if (_attackWaitTimeList[1] <= Timer._intCountNowTimer)
                {
                    _isActive = true;
                    return;
                }
                
                _isSecond = true;
                _isActive = false;
            }
        }

        if (_isSecond)
        {
            // _attackWaitList.Countが1以上 つまり、2回攻撃を行うパターンだった場合別で処理をおこなう
            if (timer == (int)_attackWaitTimeList[1])
            {
                SoundManager.Instance.PlaySE(3);

                // 選定された番号で発動スキルを決める
                switch (selectNum)
                {
                    case 0:
                        enemyBase.OnAttackPatternA2();
                        break;
                    case 1:
                        enemyBase.OnAttackPatternB2();
                        break;
                    case 2:
                        enemyBase.OnAttackPatternC2();
                        break;
                    case 3:
                        enemyBase.OnAttackPatternD2();
                        break;
                    default:
                        break;
                }

                _isActive = true;
            }
        }
    }
}
