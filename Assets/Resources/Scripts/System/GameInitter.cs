using UnityEngine;

/// <summary>
/// ゲームの初期化処理の関数を保持するクラス
/// </summary>
public partial class GameManager
{
    [SerializeField, Tooltip("削除を行うスキルバー")] private Transform _skillBar;
    [SerializeField, Tooltip("スキルの親オブジェクト")] private Transform _skillObj;
    [SerializeField, Tooltip("削除する攻撃アイコン")]
    private Transform _attackIconObj;
    [SerializeField, Tooltip("出現させるモンスター")]
    private EnemyName _enemy;
    
    /// <summary>
    /// ゲームの状態をすべて初期化する関数
    /// </summary>
    private void GameInit()
    {
        // スキルの配置
        _broker.Publish(EventList.GameSystem.GenerateSkill.GetEvent());
        // タイマーの生成
        _broker.Publish(EventList.GameSystem.TimerCreate.GetEvent());
        // スキル発動準備ステートの発行
        _broker.Publish(EventList.OnStateChangeRequest.GetEvent(StateList.PlayerState.ReadySkill, true));
        
        // モンスターの分岐
        _enemyManager.EnemyEmitter(_enemy);

        // HPとShieldの初期化
        PlayerManager.playerHP = PlayerManager.playerDefaultHP;
        PlayerManager.playerShield = 0;
        EnemyManager.enemyHP = EnemyManager.enemyDefaultHP;
        EnemyManager.enemyShield = 0;
    }

    /// <summary>
    /// ターンが変更された時に必要な情報を初期化
    /// </summary>
    private void TurnChangeInit()
    {
        GameManager.Instance.skillList.Clear();
        SkillToGage._createdBarObject.Clear();
        SkillToGage._additionTime = 0f;

        for (int i = 0; i < _skillBar.childCount; i++)
        {
            // スキルバーのスキルを削除する
            Destroy(_skillBar.GetChild(i).gameObject);
        }

        // シールドを初期化
        PlayerManager.playerShield = 0;
        EnemyManager.enemyShield = 0;
    }

    /// <summary>
    /// スキルがなかった場合補充する
    /// </summary>
    private void ReplaceSkill()
    {
        if (_skillObj.childCount <= 0)
        {
            // スキルの配置
            _broker.Publish(EventList.GameSystem.GenerateSkill.GetEvent());
        }
    }
}
