using UnityEngine;

/// <summary>
/// スキルの基底クラス
/// </summary>
public class SkillBase : MonoBehaviour
{
    public int skillImageID; // スキルのイメージの名前

    public string skillName; // スキルの名前
    public float skillTime; // スキルの発動までの時間
    public StateList.SkillType skillType; // スキルのタイプを所持しておく
    public StatusNames.BuffName buffName; // つけるバフ
    public StatusNames.BuffName deBuffName; // つけるデバフ
}
