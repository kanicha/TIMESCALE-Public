using UnityEngine;
using Random = UnityEngine.Random;
using UniRx;

/// <summary>
/// バーのtimer処理を管理するクラス
/// </summary>
public class Timer : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("Debug / デバッグ")] [SerializeField, Tooltip("デバッグをするかどうか")]
    private bool _isDebug = false;

    [SerializeField, Tooltip("タイマーで生成する値"), Range(5, 15)]
    private float _debugTimer = 0f;
#endif

    // 現在のタイマー
    private float _nowTimer = 0f;
    // 数え下ろすのではなくカウントするタイマー
    private float _countNowTimer = 0f;
    // 次のタイマー
    private float _nextTimer = 0f;

    // 一時的にタイマーの値を保持する変数
    private float _tempTimer = 0f;

    // 表示用キャスト後保持変数
    public static int _intNowTimer = 0;
    public static int _intNowTimerLength = 0;
    public static int _intNextTimer = 0;
    public static int _intCountNowTimer = 0;
    public static float _floatCountNowTimer = 0;

    public enum Timers
    {
        fiveSec,
        sixSec,
        sevenSec,
        eightSec,
        nineSec,
        tenSec,
        elevenSec,
        twelveSec,
        thirteenSec,
        fourteenSec,
        fifteenSec
    };

    private void Start()
    {
        // タイマーの作成イベントの受信後処理
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.TimerCreate>().Subscribe(_ => { TimerSet(); });
    }

    /// <summary>
    /// タイマーの代入を行うクラス
    /// </summary>
    private void TimerSet()
    {
        // タイマー保持用変数
        float timer = 0f;

#if UNITY_EDITOR
        // 初回だった場合nextTimerが存在してない為生成を行う
        /*if (StateManager.HasFlag(StateList.PlayerState.Init))
        {
            _isFirst = false;
            RandomTimerGenerate();
        }*/
#endif

        // タイマー初期化イベントの発行
        EventEmitter.Instance.Broker
            .Publish(EventList.GameSystem.TimerInit.GetEvent());
        // タイマーが初期化されたのでカウントも初期化
        _countNowTimer = 0f;


#if UNITY_EDITOR
        if (_isDebug)
        {
            timer = _debugTimer;
        }
        else
        {
#endif
            timer = _nextTimer;
#if UNITY_EDITOR
        }
#endif

        // 待機してた値を代入して、nextTimerを生成する
        _nowTimer = timer;
        _intNowTimerLength = (int)timer;

        if (!StateManager.HasFlag(StateList.PlayerState.Init))
            RandomTimerGenerate();
        
        // タイマー生成完了イベントの発行
        EventEmitter.Instance.Broker
            .Publish(EventList.GameSystem.TimeScaleBarStart.GetEvent(_intNowTimerLength, (int)timer));
        // タイマー生成完了したので同時にステートをつける
        EventEmitter.Instance.Broker
            .Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.TimerStart, true));
    }

    /// <summary>
    /// タイマーの時間を減らす処理
    /// </summary>
    public void ReduceTime()
    {
        _nowTimer -= Time.deltaTime;
        _countNowTimer += Time.deltaTime;
        
        // タイマーが0より少なく、初期化のステートが立っていなければ次のタイマーの生成
        if (_nowTimer <= -0.1 && !StateManager.HasFlag(StateList.PlayerState.Init))
        {
            // タイマーが生成されていないのでステートをつける
            EventEmitter.Instance.Broker
                .Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.TimerStop, true));

            EventEmitter.Instance.Broker.Publish(EventList.GameSystem.TimerCreate.GetEvent());
        }


        // キャスト
        _intNowTimer = (int)_nowTimer;
        _intCountNowTimer = (int)_countNowTimer;
        _floatCountNowTimer = _countNowTimer;
        _intNextTimer = (int)_nextTimer;
    }

    /// <summary>
    /// 予め決められていたタイマーをランダムで決定する
    /// </summary>
    private void RandomTimerGenerate()
    {
        // 一時的に保持した値と次のタイマーが一緒だったら抽選をし直す
        while ((int)_tempTimer == (int)_nextTimer)
        {
            _tempTimer = GameManager.Instance._timerList[Random.Range((int)EnemyManager.Enemy.EnemyTimer[0], 
                                                                    (int)EnemyManager.Enemy.EnemyTimer[1])];

            // _tempTimerと_nextTimerがちがった (つまり同じのが選択されていない場合通す)
            if ((int)_tempTimer != (int)_nextTimer)
            {
                _nextTimer = _tempTimer;
                break;
            }
        }
    }

    /// <summary>
    /// 現在の時間とリストにある値をみて合ってる値を返す関数
    /// </summary>
    /// <returns> 配列の値 </returns>
    public static Timers TimerChecker()
    {
        Timers returnNum = 0;

        for (int i = 0; i < GameManager.Instance._timerList.Length; i++)
        {
            if ((int)GameManager.Instance._timerList[i] == _intNowTimerLength)
            {
                switch (i)
                {
                    case 0:
                        returnNum = Timers.fiveSec;
                        break;
                    //6sec
                    case 1:
                        returnNum = Timers.sixSec;
                        break;
                    //7sec
                    case 2:
                        returnNum = Timers.sevenSec;
                        break;
                    //8sec
                    case 3:
                        returnNum = Timers.eightSec;
                        break;
                    //9sec
                    case 4:
                        returnNum = Timers.nineSec;
                        break;
                    //10sec
                    case 5:
                        returnNum = Timers.tenSec;
                        break;
                    //11sec
                    case 6:
                        returnNum = Timers.elevenSec;
                        break;
                    //12sec
                    case 7:
                        returnNum = Timers.twelveSec;
                        break;
                    //13sec
                    case 8:
                        returnNum = Timers.thirteenSec;
                        break;
                    //14sec
                    case 9:
                        returnNum = Timers.fourteenSec;
                        break;
                    // 15sec
                    case 10:
                        returnNum = Timers.fifteenSec;
                        break;
                    default:
                        break;
                }
            }
        }

        return returnNum;
    }
    
    
}