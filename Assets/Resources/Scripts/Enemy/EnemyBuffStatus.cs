using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// エネミーのバフ・デバフの付与と管理、そして削除を行うクラス
/// </summary>
public class EnemyBuffStatus : MonoBehaviour
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
    private Dictionary<StatusNames.BuffName, float>
        _buffDictionary = new Dictionary<StatusNames.BuffName, float>();
    // 公開用ゲッター
    public Dictionary<StatusNames.BuffName, float> BuffDictionary => _buffDictionary;

    // デバフ版
    private Dictionary<StatusNames.DeBuffName, float>
        _deBuffDictionary = new Dictionary<StatusNames.DeBuffName, float>();
    // 公開用ゲッター
    public Dictionary<StatusNames.DeBuffName, float> DeBuffDictionary => _deBuffDictionary;

    private void Start()
    {
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
        
        // 必要であればシールド監視の追加
        EventEmitter.Instance.ObserveEveryValueChanged(_ => EnemyManager.enemyShield)
            .Subscribe(_ =>
            { 
                CountReduceDeleteBuff(StatusNames.BuffName.Shield);
                Debug.LogWarning("シールドの値が変動" + EnemyManager.enemyShield);
            }).AddTo(this);
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
                countNum = EnemyManager.enemyShield;
                break;
            default:
                break;
        }

        // カウントの値が0よりすくなけば
        if (countNum <= 0)
        {
            _buffDictionary.Remove(buff);
            EventEmitter.Instance.Broker.Publish(EventList.UI.AddBuff.GetEvent(buff, false, false));
        }
    }

    /// <summary>
    /// バフの付与 (スキルによって細かく分岐)
    /// </summary>
    /// <param name="buffName"> 追加するバフ </param>
    /// <param name="buffAbilityNum"> バフの能力値 </param>>
    /// <param name="buffTime"> バフの残り時間(ターンで1消費) </param>>
    /// <param name="buffCount"> バフの残り受けれる数 </param>>
    public void AddBuff(StatusNames.BuffName buffName, float buffAbilityNum, int buffCount)
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
                EnemyManager.enemyShield += (int)buffAbilityNum;
                break;
            default:
                break;
        }

        if (!_buffDictionary.ContainsKey(buffName))
        {
            // 追加
            _buffDictionary.Add(buffName, 0);
            EventEmitter.Instance.Broker.Publish(EventList.UI.AddBuff.GetEvent(buffName, true, false));
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
