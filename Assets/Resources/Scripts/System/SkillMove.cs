using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// タッチパッド or マウスでのスキル移動を行うクラス
/// </summary>
public class SkillMove : MonoBehaviour
{
    // オブジェクト自体のポジション変数
    private Vector2 _objPos;

    // タッチしているときの座標保持用変数、キャッシュ用
    private Vector2 _touchPosition;

    // オブジェクトのスタート位置
    private Vector2 _startPosition;

    // スキルアイコンID
    public int skillIconID = 0;

    // タッチできる範囲
    [SerializeField, Tooltip("触れる範囲")] private float _radius = 100f;

    // スキルの情報
    [SerializeField, Tooltip("スキルの情報")] private SkillBase _skillBase;

    // スキルセットできる状態かどうかを表すフラグ
    private bool _isSkillSet = false;

    // スキルIDを保持する変数
    private int _tempSkillID = 0;

    private void Update()
    {
        Move();
    }

    private void Start()
    {
        _objPos = this.GameObject().transform.position;
        _startPosition = _objPos;
    }

    /// <summary>
    /// スキルの移動を行う関数
    /// </summary>
    private void Move()
    {
        _touchPosition = ScreenTouch.GetPosition();

        // 画面がタッチされている && カーソルがスキルのアイコンに近いか
        if (_touchPosition != Vector2.zero && RadiusCheck())
        {
            if (!StateManager.HasFlag(StateList.PlayerState.CatchSkill))
                // スキルのIDをtempに保持
                this._tempSkillID = this.skillIconID;
            
            // スキルをもっているのでステートをつける (スキルを持っている)
            EventEmitter.Instance.Broker
                .Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.CatchSkill, true));
        }

        // キャッチされている時且つそのIDが自身と一致していたら
        if (StateManager.HasFlag(StateList.PlayerState.CatchSkill) && _tempSkillID == this.skillIconID)
        {
            // タッチされている間はオブジェクト自身に座標を代入する
            _objPos = _touchPosition;
            // 親子関係も変更 (イベント発行)
            EventEmitter.Instance.Broker
                .Publish(EventList.GameSystem.SkillCatch.GetEvent(this.gameObject));
            
            // 触るのが終わったことを検知したら
            if (ScreenTouch.GetPhase() == Phase.Ended)
            {
                // ステートの削除 (離したという情報のステートを投げる) 
                EventEmitter.Instance.Broker
                    .Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.DetachSkill, true));
                // tempIDを初期化
                _tempSkillID = 0;

                // スキルポジション以外の場所で手を離したら
                if (_isSkillSet && StateManager.HasFlag(StateList.PlayerState.DetachSkill))
                {
                    // ゲージにスキルを追加する処理
                    Destroy(this.GameObject()); // 仮の削除処理
                    // スキル追加のイベントの発行
                    EventEmitter.Instance.Broker.Publish(EventList.GameSystem.SetSkilled.GetEvent(_skillBase, skillIconID));
                }
                else
                {
                    // ゲージ以外のところで手を離したら初期位置にもどす
                    _objPos = _startPosition;
                    EventEmitter.Instance.Broker
                        .Publish(EventList.GameSystem.SkillDeCatch.GetEvent(this.gameObject));
                }
            }

            // 最終的に代入を行う
            this.GameObject().transform.position = _objPos;
        }
    }

    /// <summary>
    /// オブジェクトの範囲を見て触れる触れないを判別する関数
    /// </summary>
    /// <returns> 触れる: true 触れない: false </returns>
    private bool RadiusCheck()
    {
        var objRadius = Vector2.Distance(ScreenTouch.GetMousePosition(), _objPos);

        return (objRadius < _radius);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        int currentNum = 0;
        
        // スキルを持っている時にコライダーのところに行き、そこで指を離すとスキル追加
        if (StateManager.HasFlag(StateList.PlayerState.CatchSkill) &&
            LayerMask.LayerToName(col.gameObject.layer).Equals("BG") &&
            !_isSkillSet)
        {
            // フラグ追加を行う
            _isSkillSet = true;

            // スキルの合計時間が超えていないかチェック or 発動しようとしているスキルがタイマーを超えていないか
            if (SkillToGage.AddListCheck(_skillBase.skillTime) 
                /*|| _skillBase.skillTime + SkillToGage.ActivatedSkillTime() <= Timer._intCountNowTimer*/)
            {
            }
            else
            {
                // 渡された情報をリストに追加する
                GameManager.Instance.skillListReservation.Add(_skillBase);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // スキルを持っている時にコライダーのところに行くとフラグ追加されるので、
        // 一回通って戻したらフラグを下げる処理
        if (StateManager.HasFlag(StateList.PlayerState.CatchSkill) &&
            LayerMask.LayerToName(col.gameObject.layer).Equals("BG") &&
            _isSkillSet)
        {
            // フラグ削除加を行う
            _isSkillSet = false;
            
            // スキルの仮配置 削除
            GameManager.Instance.skillListReservation.Remove(_skillBase);
        }
    }
}