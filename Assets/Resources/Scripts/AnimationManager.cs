using UniRx;
using UnityEngine;

/// <summary>
/// アニメーションを管理するクラス
/// </summary>
public class AnimationManager : MonoBehaviour
{
    [SerializeField, Tooltip("プレイヤーのオブジェクト")]
    private GameObject[] _playerObjects;

    [SerializeField, Tooltip("エネミーのオブジェクト")]
    private GameObject _enemyObject;
    
    private bool _isAnimated = false;

    private void Start()
    {
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.ActiveAttack>().Subscribe(_ =>
        {
            PlayAttackMotion();
        }).AddTo(this);
        
        /*EventEmitter.Instance.Broker.Receive<EventList.GameSystem.ActiveGuard>().Subscribe(_ =>
        {
            PlayGuardMotion();
        }).AddTo(this);
        
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.ActiveAssist>().Subscribe(_ =>
        {
            PlayAssistMotion();
        }).AddTo(this);*/

        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.EnemyAttack>().Subscribe(_ =>
        {
            PlayEnemyAttackMotion();
        });
    }

    private void Update()
    {
            foreach (var playerObject in _playerObjects)
            {
                if (!playerObject.GetComponent<Animation>().IsPlaying("sword_attack_01"))
                {
                    playerObject.GetComponent<Animation>().Play("sword_idle");
                }
                /*if (!playerObject.GetComponent<Animation>().IsPlaying("sword_gearburst_01_A"))
                {
                    playerObject.GetComponent<Animation>().Play("sword_idle");
                }
                if (!playerObject.GetComponent<Animation>().IsPlaying("sword_guard"))
                {
                    playerObject.GetComponent<Animation>().Play("sword_idle");
                }*/
            }

            if (!_enemyObject.GetComponent<Animation>().IsPlaying("drgn00_attack_sp_06_c"))
                _enemyObject.GetComponent<Animation>().Play("drgn00_idle");
    }

    /// <summary>
    /// こうげきのイベントが飛んできた時にアニメーションを再生する
    /// </summary>
    private void PlayAttackMotion()
    {
        foreach (var playerObject in _playerObjects)
        {
            playerObject.GetComponent<Animation>().Play("sword_attack_01");
        }
    }
    
    /// <summary>
    /// ガードのイベントが飛んできた時にアニメーションを再生する
    /// </summary>
    private void PlayGuardMotion()
    {
        foreach (var playerObject in _playerObjects)
        {
            playerObject.GetComponent<Animation>().Play("sword_guard");
        }
    }
    
    /// <summary>
    /// こアシストのイベントが飛んできた時にアニメーションを再生する
    /// </summary>
    private void PlayAssistMotion()
    {
        foreach (var playerObject in _playerObjects)
        {
            playerObject.GetComponent<Animation>().Play("sword_gearburst_01_A");
        }
    }

    /// <summary>
    /// 相手の攻撃イベントが飛んできた時にアニメーション再生
    /// </summary>
    private void PlayEnemyAttackMotion()
    {
        _enemyObject.GetComponent<Animation>().Play("drgn00_attack_sp_06_c");
    }
}