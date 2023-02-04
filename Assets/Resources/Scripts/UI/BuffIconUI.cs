using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バフのアイコンを表示する処理
/// </summary>
public class BuffIconUI : MonoBehaviour
{
    [Header("出現する親オブジェクト")] [SerializeField, Tooltip("キャンバス")]
    private GameObject canvasObj;

    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private Transform playerParentObject;
    [SerializeField, Tooltip("エネミーオブジェクト")]
    private Transform enemyParentObject;

    [SerializeField, Tooltip("出現させるアイコンオブジェクト")]
    private GameObject[] iconObject;

    // 生成予定バフアイコン
    private Dictionary<StatusNames.BuffName, GameObject> _playerCreatedObj = new Dictionary<StatusNames.BuffName, GameObject>();
    private Dictionary<StatusNames.BuffName, GameObject> _enemyCreatedObj = new Dictionary<StatusNames.BuffName, GameObject>();

    /// <summary>
    /// バフアイコンの表示
    /// </summary>
    public void ShowBuffIconPlayer(StatusNames.BuffName buffName)
    {
        GameObject createObj = null;
        GameObject createdObj = null;
        
        // 現在付いているバフを習得
        switch (buffName)
        {
            case StatusNames.BuffName.None:
                break;
            case StatusNames.BuffName.Dodge:
                break;
            case StatusNames.BuffName.HardBlow:
                createObj = iconObject[0];
                break;
            case StatusNames.BuffName.HardDefense:
                createObj = iconObject[1];
                break;
            case StatusNames.BuffName.Invincible:
                createObj = iconObject[2];
                break;
            case StatusNames.BuffName.Shield:
                createObj = iconObject[3];
                break;
            default:
                break;
        }
        
        // 生成を行ったら親子関係の変更を行う
        createdObj = Instantiate(createObj, canvasObj.transform, false);
            createdObj.transform.parent = playerParentObject.transform;
            _playerCreatedObj.Add(buffName, createdObj);
        
    }

    /// <summary>
    /// バフアイコンの表示
    /// </summary>
    public void ShowBuffIconEnemy(StatusNames.BuffName buffName)
    {
        GameObject createObj = null;
        GameObject createdObj = null;
        
        // 現在付いているバフを習得
        switch (buffName)
        {
            case StatusNames.BuffName.None:
                break;
            case StatusNames.BuffName.Dodge:
                break;
            case StatusNames.BuffName.HardBlow:
                createObj = iconObject[0];
                break;
            case StatusNames.BuffName.HardDefense:
                createObj = iconObject[1];
                break;
            case StatusNames.BuffName.Invincible:
                createObj = iconObject[2];
                break;
            case StatusNames.BuffName.Shield:
                createObj = iconObject[3];
                break;
            default:
                break;
        }
        
        // 生成を行ったら親子関係の変更を行う
        createdObj = Instantiate(createObj, canvasObj.transform, false);
        createdObj.transform.parent = enemyParentObject.transform;
        _enemyCreatedObj.Add(buffName, createdObj);
        
    }
    
    /// <summary>
    /// バフアイコンを削除する処理
    /// </summary>
    public void DeleteBuffIconPlayer(StatusNames.BuffName buffName)
    {
        if (_playerCreatedObj.ContainsKey(buffName))
        {
            Destroy(_playerCreatedObj[buffName]);
            _playerCreatedObj.Remove(buffName);
        }
    }
    
    /// <summary>
    /// バフアイコンを削除する処理
    /// </summary>
    public void DeleteBuffIconEnemy(StatusNames.BuffName buffName)
    {
        if (_enemyCreatedObj.ContainsKey(buffName))
        {
            Destroy(_enemyCreatedObj[buffName]);
            _enemyCreatedObj.Remove(buffName);
        }
    }
}