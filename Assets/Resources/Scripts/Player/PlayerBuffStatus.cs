using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤーのバフ・デバフの付与と管理、そして削除を行うクラス
/// </summary>
public class PlayerBuffStatus : MonoBehaviour
{
    // --- 計算用 --- //
    // 強撃の能力値
    public float _hardBlowNum = 0;
    // 攻防の能力値
    public float _hardDefenseNum = 0;
    
    // --- 残り回数 ---//
    // 残り無敵回数
    public int _invincibleCount = 0;
    // 残り強撃回数
    public int _hardBlowCount = 0;
    // 残り攻防回数
    public int _hardDefenseCount = 0;
    
    // バフの名前と残りバフ時間(ターン)を保持するDictionary変数
    private static Dictionary<StatusNames.BuffName, float>
        _buffDictionary = new Dictionary<StatusNames.BuffName, float>();
    // 公開用ゲッター
    public static Dictionary<StatusNames.BuffName, float> BuffDictionary => _buffDictionary;

    // デバフ版
    private Dictionary<StatusNames.DeBuffName, float>
        _deBuffDictionary = new Dictionary<StatusNames.DeBuffName, float>();
    // 公開用ゲッター
    public Dictionary<StatusNames.DeBuffName, float> DeBuffDictionary => _deBuffDictionary;

    private void Start()
    {
        // ターンが切り替わったイベント受信
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.TimerInit>().Subscribe(_ =>
        {
            // ターン経過で付いているバフの値を減らしていく処理
            TurnReduceBuff();
        }).AddTo(this);
        
        // スキルのカウントが変更されたら終了かチェック
        EventEmitter.Instance.ObserveEveryValueChanged(_ => _invincibleCount)
            .Subscribe(_ =>
            { 
                CountReduceDeleteBuff(StatusNames.BuffName.Invincible);
            }).AddTo(this);
        
        EventEmitter.Instance.ObserveEveryValueChanged(_ => _hardBlowCount)
            .Subscribe(_ =>
            { 
                CountReduceDeleteBuff(StatusNames.BuffName.HardBlow);
            }).AddTo(this);
        
        EventEmitter.Instance.ObserveEveryValueChanged(_ => _hardDefenseCount)
            .Subscribe(_ =>
            { 
                CountReduceDeleteBuff(StatusNames.BuffName.HardDefense);
            }).AddTo(this);
        
        EventEmitter.Instance.ObserveEveryValueChanged(_ => PlayerManager.playerShield)
            .Subscribe(_ =>
            { 
                CountReduceDeleteBuff(StatusNames.BuffName.Shield);
            }).AddTo(this);
    }

    /// <summary>
    /// 付いているバフ・デバフをターン消費で消す処理
    /// </summary>
    private void TurnReduceBuff()
    {
        // ターン変更が通知された後に呼ばれる関数
        var keyList = new List<StatusNames.BuffName>(_buffDictionary.Keys);
        int countNum = 0;
        bool turnPriority = false; // ターンが消費されるか値が一定値下がるかを優先するか
        bool reducePriority = false; // 消費優先
        
        foreach (var buff in keyList)
        {
            // まず付いているバフをすべて見る
            // ターンが変更されているのが確定しているので ついているすべてのバフの value を -1 する
            _buffDictionary[buff]--;
            
            // もしこの時に valueが 0以下 かつ カウントが0以下の時に リストからそのスキルを削除する
            switch (buff)
            {
                case StatusNames.BuffName.None:
                    break;
                case StatusNames.BuffName.Dodge:
                    break;
                case StatusNames.BuffName.HardBlow:
                    countNum = _hardBlowCount;
                    break;
                case StatusNames.BuffName.HardDefense:
                    countNum = _hardDefenseCount;
                    /*reducePriority = true;*/
                    break;
                case StatusNames.BuffName.Invincible:
                    countNum = _invincibleCount;
                    turnPriority = true;
                    break;
                case StatusNames.BuffName.Shield:
                    break;
                default:
                    break;
            }

            if (turnPriority)
            {
                if (_buffDictionary[buff] <= 0 || countNum <= 0)
                {
                    Debug.Log("削除したバフ" + buff);
                    _buffDictionary.Remove(buff);
                    EventEmitter.Instance.Broker.Publish(EventList.UI.AddBuff.GetEvent(buff, false, true));
                }
                
                turnPriority = false;
            }
            else
            {
                if (_buffDictionary[buff] <= 0 && countNum <= 0)
                {
                    Debug.Log("削除したバフ" + buff);
                    _buffDictionary.Remove(buff);
                    EventEmitter.Instance.Broker.Publish(EventList.UI.AddBuff.GetEvent(buff, false, true));
                }
            }

            if (_buffDictionary.ContainsKey(buff))
            {
                Debug.Log("バフの名前" + buff);
                Debug.Log("残りターン" + _buffDictionary[buff]);
            }
        }
    }

    /// <summary>
    /// カウントで減った場合スキルを削除する処理
    /// </summary>
    private void CountReduceDeleteBuff(StatusNames.BuffName buff)
    {
        int countNum = 0;

        // もしこの時にカウントが0以下の時にリストからそのスキルを削除する
        switch (buff)
        {
            case StatusNames.BuffName.None:
                break;
            case StatusNames.BuffName.Dodge:
                break;
            case StatusNames.BuffName.HardBlow:
                countNum = _hardBlowCount;
                break;
            case StatusNames.BuffName.HardDefense:
                countNum = _hardDefenseCount;
                break;
            case StatusNames.BuffName.Invincible:
                countNum = _invincibleCount;
                break;
            case StatusNames.BuffName.Shield:
                countNum = PlayerManager.playerShield;
                break;
            default:
                break;
        }

        // カウントの値が0よりすくなけば
        if (countNum <= 0)
        {
            _buffDictionary.Remove(buff);
            
            EventEmitter.Instance.Broker.Publish(EventList.UI.AddBuff.GetEvent(buff, false, true));
        }
    }

    /// <summary>
    /// バフの付与 (スキルによって細かく分岐)
    /// </summary>
    /// <param name="buffName"> 追加するバフ </param>
    /// <param name="buffAbilityNum"> バフの能力値 </param>>
    /// <param name="buffTime"> バフの残り時間(ターンで1消費) </param>>
    /// <param name="buffCount"> バフの残り受けれる数 </param>>
    public void AddBuff(StatusNames.BuffName buffName, float buffAbilityNum, float buffTime, int buffCount)
    {
        if (buffName == StatusNames.BuffName.None) return;
        
        switch (buffName)
        {
            case StatusNames.BuffName.Dodge:
                break;
            case StatusNames.BuffName.HardBlow:
                // 能力値の追加
                _hardBlowNum = buffAbilityNum;
                // 残り回数の追加
                _hardBlowCount = buffCount;
                break;
            case StatusNames.BuffName.HardDefense:
                // 能力値の追加
                _hardDefenseNum = buffAbilityNum;
                // 残り回数の追加
                _hardDefenseCount = buffCount;
                break;
            case StatusNames.BuffName.Invincible:
                // 残り回数の追加
                _invincibleCount = buffCount;
                break;
            case StatusNames.BuffName.Shield:
                break;
            default:
                break;
        }

        if (!_buffDictionary.ContainsKey(buffName))
        {
            // UI表示のためのイベント発行
            EventEmitter.Instance.Broker.Publish(EventList.UI.AddBuff.GetEvent(buffName, true, true));
            
            // 追加
            _buffDictionary.Add(buffName, buffTime);
        }
        else
        {
            Debug.Log("更新");

            // すでに存在していた場合は更新
            _buffDictionary[buffName] = buffTime;
        }
    }
    
    /// <summary>
    /// デバフ追加
    /// </summary>
    /// <param name="deBuffName"></param>
    private void AddDeBuff(StatusNames.DeBuffName deBuffName)
    {
        switch (deBuffName)
        {
            case StatusNames.DeBuffName.None:
                break;
            case StatusNames.DeBuffName.EyeSight:
                break;
            case StatusNames.DeBuffName.Despondency:
                break;
            case StatusNames.DeBuffName.Fragility:
                break;
            default:
                break;
        }
        
        if (!_deBuffDictionary.ContainsKey(deBuffName))
        {
            // 追加
            _deBuffDictionary.Add(deBuffName, 0f);
        }
        else
        {
            // すでに存在していた場合は更新
            _deBuffDictionary[deBuffName] = 0f;
        }
    }
    
    /// <summary>
    /// バフの削除
    /// </summary>
    /// <param name="buffName"> 削除するバフ </param>
    private void DeleteBuff(StatusNames.BuffName buffName)
    {
        // バフ検索を行う
        if (_buffDictionary.ContainsKey(buffName))
        {
            // 削除
            _buffDictionary.Remove(buffName);
        }
    }
    
    private void DeleteDeBuff(StatusNames.DeBuffName deBuffName)
    {
        if (_deBuffDictionary.ContainsKey(deBuffName))
        {
            _deBuffDictionary.Remove(deBuffName);
        }
    }
    
    /// <summary>
    /// すでに同じバフが入っているかどうかのチェック関数
    /// </summary>
    /// <returns> true: 入っている false: 入っていない </returns>
    public bool ActiveBuffCheck(StatusNames.BuffName buffName)
    {
        // _buffDictionary.ContainsKey(buffName) で検索できるからこの関数いらないかも
        return _buffDictionary.ContainsKey(buffName);
    }
    
    /// <summary>
    /// 現在付与されているバフと効果時間の表示
    /// </summary>
    public void ShowBuff()
    {
        foreach (var buff in _buffDictionary)
        {
            Debug.Log(buff.Key);
            Debug.Log(buff.Value);
            
            switch (buff.Key)
            {
                case StatusNames.BuffName.HardBlow:
                    Debug.Log("強撃能力値" + _hardBlowNum);
                    Debug.Log("強撃残り回数:" + _hardBlowCount);
                    break;
                case StatusNames.BuffName.HardDefense:
                    Debug.Log("攻防能力値" + _hardDefenseNum);
                    Debug.Log("攻防残り回数" + _hardDefenseCount);
                    break;
                case StatusNames.BuffName.Invincible:
                    Debug.Log("無敵時間残り回数" + _invincibleCount);
                    break;
            }
        }
    }

    /// <summary>
    /// 引数で渡された値のターン数値を減らす
    /// </summary>
    /// <param name="buffName"> バフの名前 </param>
    public void ReduceBuff(StatusNames.BuffName buffName)
    {
        switch (buffName)
        {
            case StatusNames.BuffName.None:
                break;
            case StatusNames.BuffName.Dodge:
                break;
            case StatusNames.BuffName.HardBlow:
                _hardBlowCount--;
                break;
            case StatusNames.BuffName.HardDefense:
                _hardDefenseCount--;
                break;
            case StatusNames.BuffName.Invincible:
                _invincibleCount--;
                break;
            case StatusNames.BuffName.Shield:
                break;
            default:
                break;
        }
    }
}
