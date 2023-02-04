/// <summary>
/// ゲームのステートを管理するクラス
/// </summary>
public static class StateList
{ 
    // ビット演算に変更したステート
    [System.Flags]
    public enum PlayerState : ulong
    {
        None = 1L << 0,
        // 動作関係 (1 ~ 10)
        CatchSkill = 1L << 1,
        DetachSkill = 1L << 2,
        // ゲームフロウ (11 ~ 30)
        Init = 1L << 11,
        GameStart = 1L << 12,
        Pause = 1L << 13,
        TimerStart = 1L << 14,
        TimerStop = 1L << 15,
        PlayerAttack = 1L << 16,
        EnemyAttack = 1L << 17,
        PlayerDead = 1L << 18,
        EnemyDead = 1L << 19,
        StandbySkill = 1L << 20,
        ReadySkill = 1L << 21
    }
    public static PlayerState playerState = PlayerState.None;

    /// <summary>
    /// スキルのタイプを管理するenum
    /// </summary>
    public enum SkillType
    {
        None,
        Assist,
        Attack,
        Guard
    }
}
