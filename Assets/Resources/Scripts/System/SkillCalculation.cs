using System;
using UnityEngine;
using UniRx;

/// <summary>
/// 各スキルの計算処理クラス
/// </summary>
public class SkillCalculation : MonoBehaviour
{
    // プレイヤーのバフ管理クラス
    [SerializeField] private PlayerBuffStatus playerBuffStatus;
    // エネミーのバフ管理クラス
    [SerializeField] private EnemyBuffStatus enemyBuffStatus;

    private int _tempShield = 0;
    private bool _shieldJust = false;
    
    private IMessageBroker _broker;
    private void Start()
    {
        _broker = EventEmitter.Instance.Broker;
        
        // 各イベント受信時
        _broker.Receive<EventList.GameSystem.ActiveAttack>().Subscribe(x =>
        {
            // 関数の実行 
            AttackCalculation(x.AttackDamage);
        }).AddTo(this);

        _broker.Receive<EventList.GameSystem.ActiveGuard>().Subscribe(x =>
        {
            // 関数の実行
            GuardCalculation(x.SkillName, x.BuffName, x.Num, x.Time, x.Count, x.IsJust);
        }).AddTo(this);

        _broker.Receive<EventList.GameSystem.ActiveAssist>().Subscribe(x =>
        {
            // 関数の実行
            AssistCalculation(x.SkillName, x.BuffName, x.Num, x.Time, x.Count, x.IsJust);
        }).AddTo(this);

        // シールドがジャストしたとき限定処理
        _broker.Receive<EventList.GameSystem.TimerCreate>()
            .Where(_ => _shieldJust)
            .Subscribe(_ =>
            {
                PlayerManager.playerShield += _tempShield;
                playerBuffStatus.AddBuff(StatusNames.BuffName.Shield, _tempShield , 0, 0);

                _tempShield = 0;
                _shieldJust = false;
                Debug.Log("プレイヤーのシールド" + PlayerManager.playerShield);
            }).AddTo(this);

        // エネミーの処理受信
        _broker.Receive<EventList.GameSystem.EnemyAttackCal>().Subscribe(x =>
        {
            EnemyActionEmitter(x.BuffName, x.AttackDamage, x.AbilityNum, x.Count);
        }).AddTo(this);
    }

    /// <summary>
    /// 攻撃イベントが発生したら処理を行う
    /// </summary>
    /// <param name="attackDamage"> スキルのダメージ量 </param>>
    private void AttackCalculation(int attackDamage)
    {
        // 計算後のダメージ
        int calcDamage = 0;

        // 攻撃
        // バフが会った場合バフ計算処理
        if (playerBuffStatus.ActiveBuffCheck(StatusNames.BuffName.HardBlow))
        {
            // 自身の持っているバフを掛け合わして乗算 (計算後の値を出す)
            calcDamage = attackDamage + (int)playerBuffStatus._hardBlowNum;
            playerBuffStatus.ReduceBuff(StatusNames.BuffName.HardBlow);
        }
        // なかった場合そのまま値を通す
        else if (!playerBuffStatus.ActiveBuffCheck(StatusNames.BuffName.HardBlow))
        {
            calcDamage = attackDamage;
        }

        // 相手のバフがかかっているかどうかのチェック関数
        // 防御バフのチェック
        if (enemyBuffStatus.ActiveBuffCheck(StatusNames.BuffName.HardDefense))
        {
            // 計算したら代入する
            calcDamage = ProportionCalculation(attackDamage, enemyBuffStatus._hardDefenseNum);
        }

        if (EnemyManager.enemyShield >= 0)
        {
            // 存在していたら攻撃しようとしていた値から減算を行う
            EnemyManager.enemyHP -= (calcDamage - EnemyManager.enemyShield);
        }
        else
        {
            // 処理
            EnemyManager.enemyHP -= calcDamage;
        }
        
        Debug.Log("ダメージ" + calcDamage);
        Debug.Log(EnemyManager.enemyHP);

    }

    /// <summary>
    /// アシストの計算処理
    /// </summary>
    private void AssistCalculation
        (AssistSkillName skillName ,StatusNames.BuffName buffName, float abilityNum, float time, int count, bool isJust)
    { 
        // アシスト
        switch (skillName)
        {
            case AssistSkillName.Active:
                playerBuffStatus.AddBuff(buffName, abilityNum, time, count);
                break;
            case AssistSkillName.Mind:
                if (PlayerManager.playerHP <= PlayerManager.playerDefaultHP)
                {
                    // HPかいふく
                    PlayerManager.playerHP += (int)abilityNum;
                }
                break;
            case AssistSkillName.Spirit:
                playerBuffStatus.AddBuff(buffName, abilityNum, time, count);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(skillName), skillName, null);
        }
           
    }

    /// <summary>
    /// ガードの計算処理
    /// </summary>
    private void GuardCalculation
        (GuardSkillName skillName, StatusNames.BuffName buffName, float abilityNum, float time, int count, bool isJust)
    {
        // 防御
        switch (skillName)
        {
            case GuardSkillName.Barrier:
                playerBuffStatus.AddBuff(buffName, abilityNum, time, count);
                break;
            case GuardSkillName.Guard:
                if (!isJust)
                {
                    PlayerManager.playerShield += (int)abilityNum;
                    playerBuffStatus.AddBuff(StatusNames.BuffName.Shield, abilityNum , 0, 0);
                }
                else
                {
                    // 次のターン開始時にシールド10
                    _tempShield = (int)abilityNum;
                    _shieldJust = true;
                }
                break;
            case GuardSkillName.Parry:
                if (!isJust)
                {
                    PlayerManager.playerShield += (int)abilityNum;
                    playerBuffStatus.AddBuff(StatusNames.BuffName.Shield, abilityNum , 0, 0);
                }
                else
                {
                    // 次のターン開始時にシールド10
                    _tempShield = (int)abilityNum;
                    _shieldJust = true;
                }
                break;
            default:
                break;
        }
    }
    
    // 攻撃される時に、防御バフがついていたらその値を見て割合計算、その値分プレイヤーのHPを削る
    private void EnemyActionEmitter(StatusNames.BuffName buffName, int attackDamage , float abilityNum, int count)
    {
        // 攻撃のタイプが何かみる
        // None(攻撃以外)だったらバフの追加
        if (buffName != StatusNames.BuffName.None)
        {
            SoundManager.Instance.PlaySE(4);
            
            enemyBuffStatus.AddBuff(buffName, abilityNum, count);
        }
        else
        {
            // Noneだった場合は攻撃なので処理を行う
            // 計算後のダメージ
            int calcDamage = 0;

            // 攻撃
            // バフが会った場合バフ計算処理
            if (enemyBuffStatus.ActiveBuffCheck(StatusNames.BuffName.HardBlow))
            {
                // 自身の持っているバフを掛け合わして乗算 (計算後の値を出す)
                calcDamage = attackDamage + (int)enemyBuffStatus._hardBlowNum;
                enemyBuffStatus.ReduceBuff(StatusNames.BuffName.HardBlow);
            }
            // なかった場合そのまま値を通す
            else if (!playerBuffStatus.ActiveBuffCheck(StatusNames.BuffName.HardBlow))
            {
                calcDamage = attackDamage;
            }

            // 相手のバフがかかっているかどうかのチェック関数
            // 先に無敵があるかどうかを見る
            if (playerBuffStatus.ActiveBuffCheck(StatusNames.BuffName.Invincible))
            {
                SoundManager.Instance.PlaySE(5);
                
                // 会った場合、攻撃を中断し無敵の値を減らす
                playerBuffStatus.ReduceBuff(StatusNames.BuffName.Invincible);
            }
            // 無敵がなかった場合は計算処理
            else
            {
                // 防御バフのチェック
                if (playerBuffStatus.ActiveBuffCheck(StatusNames.BuffName.HardDefense))
                {
                    // 計算したら代入する
                    calcDamage = ProportionCalculation(attackDamage, playerBuffStatus._hardDefenseNum);
                    playerBuffStatus.ReduceBuff(StatusNames.BuffName.HardDefense);
                }

                if (PlayerManager.playerShield >= 0)
                {
                    SoundManager.Instance.PlaySE(3);
                    
                    // 存在していたら攻撃しようとしていた値から減算を行う
                    PlayerManager.playerHP -= (calcDamage - PlayerManager.playerShield);
                }
                else
                {
                    SoundManager.Instance.PlaySE(3);
                    
                    // 処理
                    PlayerManager.playerHP -= calcDamage;
                }

                _broker.Publish(EventList.GameSystem.EnemyAttack.GetEvent());
                _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.EnemyAttack, false));
            }
        }
    }
    
    /// <summary>
    /// 計算したい値を入れると割合の計算する値を返す
    /// </summary>
    /// <param name="calcNum"> 計算する値 </param>>
    /// <param name="originalNum"> もとの値 </param>>
    private int ProportionCalculation(float calcNum, float originalNum)
    {
        float returnNum = 0f;

        returnNum = calcNum / originalNum * 100;

        return (int)returnNum;
    }
}
