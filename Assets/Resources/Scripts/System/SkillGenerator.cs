using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// スキルの生成と配置を行うクラス
/// </summary>
public class SkillGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("配置を行うポジション")]
    private GameObject[] _skillPositionObjects = new GameObject[7];

    [SerializeField, Tooltip("生成を行った後の親オブジェクト")]
    private GameObject _parentObject;

    [SerializeField, Tooltip("スキルを持っているときの親オブジェクト")]
    private GameObject _catchObject;
    
    [SerializeField, Tooltip("実際に生成を行うスキルprefab")]
    private List<GameObject> _skillPrefabs = new List<GameObject>();
    private void Start()
    {
        // スキル生成を行うイベントが飛ばされたら生成する
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.GenerateSkill>()
            .Subscribe(_ =>
            {
                SkillGenerate();
            }).AddTo(this);

        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.ReGenerateSkill>()
            .Subscribe(x =>
            {
                SkillGenerate(x.SkillNum);
            }).AddTo(this);
        
        // スキルを持っているときに親変更
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.SkillCatch>()
            .Subscribe(x =>
            {
                x.catchObject.transform.SetParent(_catchObject.transform, false);
            }).AddTo(this);
        
        // ハズレた場合は親を戻す
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.SkillDeCatch>()
            .Subscribe(x =>
            {
                x.catchObject.transform.SetParent(_parentObject.transform, false);
            }).AddTo(this);
    }

    /// <summary>
    /// スキルを生成する関数
    /// </summary>
    private void SkillGenerate()
    {
        // for文で回して各オブジェクトを配置
        for (var i = 0; i < _skillPositionObjects.Length; i++)
        {
            // 生成とparentの設定
            var parent = Instantiate(_skillPrefabs[i], _parentObject.transform, false);

            // 生成したオブジェクトのIDを設定
            parent.GetComponent<SkillMove>().skillIconID = i + 1;
            
            // ポジションの同期
            parent.transform.position = _skillPositionObjects[i].transform.position;
        }
    }

    /// <summary>
    /// 生成した値を指定してスキルを生成する
    /// </summary>
    /// <param name="num">　生成したいスキルの番号　</param>
    private void SkillGenerate(int num)
    {
        int createNum = num - 1;
        
        // 生成とparentの設定
        var parent = Instantiate(_skillPrefabs[createNum], _parentObject.transform, false);

        // 生成したオブジェクトのIDを設定
        parent.GetComponent<SkillMove>().skillIconID = createNum + 1;
            
        // ポジションの同期
        parent.transform.position = _skillPositionObjects[createNum].transform.position;
    }
}
