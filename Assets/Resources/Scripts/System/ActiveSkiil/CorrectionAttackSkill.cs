using System;
using UnityEngine;

/// <summary>
/// 攻撃スキルの処理クラス
/// </summary>
public partial class ActiveSkill
{
    /// <summary>
    /// 攻撃スキルの値調整処理
    /// </summary>
    private void CorrectionAttackSkill(AttackSkillBase attackSkill, bool isJust)
    {
        // スキル時間の値保持を行う変数
        float isActivatedSkillTime = 0f;

        // 発動済みリストが0より上だった場合
        if (_activatedSkills.Count > 0)
        {
            // 発動済みリストに入っているスキルの時間を加算をする
            foreach (var activeSkill in _activatedSkills)
            {
                isActivatedSkillTime += activeSkill.skillTime;
            }
        }

        if (!isJust)
        {
            // イベントの発行
            _broker.Publish(EventList.GameSystem.ActiveSetAttackSkill.GetEvent(attackSkill, isActivatedSkillTime));
        }
        else
        {
            AttackSkillActive(attackSkill, isActivatedSkillTime);
        }
    }

    /// <summary>
    /// スキルの発動チェック関数
    /// </summary>
    /// <param name="skill"> 発動しようとしているスキル </param>>
    /// <param name="isActivatedSkillTime"> スキルの発動時間調整用変数 </param>
    /// <param name="nowTimer"> 現在のタイマー時間 </param>
    private bool AttackSkillActiveChecker(AttackSkillBase skill, float isActivatedSkillTime, float nowTimer)
    {
        bool isActive = false;
        
        // ジャストスキル計算用変数
        float addSkillTime = skill.skillTime + isActivatedSkillTime;
        
        // 現在の時間とスキルの発動時間を合わせた値
        float currentTime = Math.Abs(addSkillTime - nowTimer);
        
        // もし発動時間が過ぎてた場合trueにして破棄を行う
        if (addSkillTime < nowTimer)
        {
            // 終了処理
            _broker.Publish(EventList.GameSystem.ActivatedSkill.GetEvent());
            _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.ReadySkill, true));

            isActive = true;
        }
        
        // 0.1の範囲であれば発動を行う
        if (currentTime < 0.1)
        {
            // スキルの値 + 今までのスキルの発動合計時間 == 今回のタイマーの最大時間が一緒
            if ((int)addSkillTime == Timer._intNowTimerLength)
            {
                SoundManager.Instance.PlaySE(0);
                // ジャストスキルの発動
                skill.OnAttackJust();
            }
            else
            {
                SoundManager.Instance.PlaySE(0);
                // スキルの発動
                skill.OnAttack();
            }

            // スキルの処理がおわったらイベントを飛ばす
            _broker.Publish(EventList.GameSystem.ActivatedSkill.GetEvent());
            // Standbyステートを削除し、ReadySkillステートをつける
            _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.ReadySkill, true));

            isActive = true;
        }

        return isActive;
    }

    /// <summary>
    /// スキル即時発動関数
    /// </summary>
    /// <param name="skill"> 発動しようとしているスキル </param>>
    /// <param name="isActivatedSkillTime"> スキルの発動時間調整用変数 </param>
    private void AttackSkillActive(AttackSkillBase skill, float isActivatedSkillTime)
    {
        // ジャストスキル計算用変数
        float addSkillTime = skill.skillTime + isActivatedSkillTime;

        // スキルの値 + 今までのスキルの発動合計時間 == 今回のタイマーの最大時間が一緒
        if ((int)addSkillTime == Timer._intNowTimerLength)
        {
            SoundManager.Instance.PlaySE(0);
            // ジャストスキルの発動
            skill.OnAttackJust();
        }
        else
        {
            SoundManager.Instance.PlaySE(0);
            // スキルの発動
            skill.OnAttack();
        }

        // Standbyステートを削除し、ReadySkillステートをつける
        _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.ReadySkill, true));
    }
}