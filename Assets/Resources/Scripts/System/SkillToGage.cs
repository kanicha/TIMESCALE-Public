using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// スキルの移動イベントが発行されたら受信し、スキルをゲージに追加するクラス
/// </summary>
public class SkillToGage : MonoBehaviour
{
    [SerializeField] private GameObject _canvasObj;
    [SerializeField] private GameObject _parentObj;

    [Header("生成するバーオブジェクト")] [SerializeField]
    private GameObject _assistSkillPrefab;

    [SerializeField] private GameObject _attackSkillPrefab;
    [SerializeField] private GameObject _guardSkillPrefab;
    [SerializeField] private GameObject _reservationPrefab;
    [Header("生成を行う横幅配列")] [SerializeField] private float[] resizeNum;

    private readonly float _createObjWidth = 85;

    // スキルの値を足し算した値を保持する変数
    public static float  _additionTime = 0f;

    // 生成を行ったオブジェクトを保管するリスト変数
    public static List<GameObject> _createdBarObject = new List<GameObject>();
    private GameObject _reservationObj;

    private void Start()
    {
        // イベントの受信処理
        EventEmitter.Instance.Broker.Receive<EventList.GameSystem.SetSkilled>().Subscribe(x => { ToList(x.SkillBase, x.SkillNum); })
            .AddTo(this);
    }

    /// <summary>
    /// ゲージに追加する処理関数
    /// </summary>
    /// <param name="skillBase"> 渡されたスキルの情報 </param>>
    /// <param name="createSkillNum"> 生成しようとしているスキル番号 </param>>
    private void ToList(SkillBase skillBase, int createSkillNum)
    {
        // スキルの合計時間が超えていないかチェック or 発動しようとしているスキルがタイマーを超えていないか
        if (AddListCheck(skillBase.skillTime) /*|| skillBase.skillTime + ActivatedSkillTime() <= Timer._intCountNowTimer*/)
        {
            // 超えていたら追加しようとしていたスキルをスキル置き場に戻すイベントの発行
            EventEmitter.Instance.Broker.
                Publish(EventList.GameSystem.ReGenerateSkill.GetEvent(createSkillNum));
        }
        else
        {
            // 渡された情報をリストに追加する
            GameManager.Instance.skillList.Add(skillBase);
        }
    }

    /// <summary>
    /// ゲージにスキルの見た目を反映させる
    /// </summary>
    /// <param name="listNum"> リストの番号 </param>>
    public void ToGage(int listNum)
    {
        // 生成を行うオブジェクトを保持する変数
        GameObject barObj = null;

        Debug.Log("スキルがリストに追加");

        // タイプと時間を見て分岐
        switch (GameManager.Instance.skillList[listNum].skillType)
        {
            case StateList.SkillType.None:
                break;
            case StateList.SkillType.Assist:
                barObj = _assistSkillPrefab;
                break;
            case StateList.SkillType.Attack:
                barObj = _attackSkillPrefab;
                break;
            case StateList.SkillType.Guard:
                barObj = _guardSkillPrefab;
                break;
            default:
                break;
        }

        // 生成をキャンバスで行う
        GameObject createdObj = Instantiate(barObj, _canvasObj.transform, false);
        // 生成を行ったら親子関係の変更を行う
        createdObj.transform.parent = _parentObj.transform;

        // 生成する場所も決めてあげる
        RePositionObject(_createdBarObject, createdObj);
        // 生成を行う前にオブジェクトの横幅調整
        ResizeObject(createdObj, listNum);

        // 調整のためにリストに追加
        _createdBarObject.Add(createdObj);
    }

    /// <summary>
    /// スキルアイコンが判定内に入っていたら灰色で表示
    /// </summary>
    public void ToGageReservation(int listNum)
    {
        // 生成を行うオブジェクトを保持する変数
        GameObject barObj = _reservationPrefab;

        // 生成をキャンバスで行う
        GameObject createdObj = Instantiate(barObj, _canvasObj.transform, false);
        // 生成を行ったら親子関係の変更を行う
        createdObj.transform.parent = _parentObj.transform;

        // 生成する場所も決めてあげる
        RePositionObject(_createdBarObject, createdObj);
        // 生成を行う前にオブジェクトの横幅調整
        ReservationResizeObject(createdObj, listNum);
        
        _reservationObj = createdObj;
    }

    public void ReservationObjDelete()
    {
        Destroy(_reservationObj);
    }

    /// <summary>
    /// スキルの時間を見て横軸の調整を行う処理関数
    /// </summary>
    /// <param name="obj"> 調整を行うオブジェクト </param>
    /// <param name="listNum"> 何番のリストか </param>
    private void ResizeObject(GameObject obj, int listNum)
    {
        obj.GetComponent<RectTransform>().sizeDelta =
            new Vector2(ResizeValue(GameManager.Instance.skillList[listNum],
                Timer.TimerChecker()), _createObjWidth);
    }
    
    private void ReservationResizeObject(GameObject obj, int listNum)
    {
        obj.GetComponent<RectTransform>().sizeDelta =
            new Vector2(ResizeValue(GameManager.Instance.skillListReservation[listNum],
                Timer.TimerChecker()), _createObjWidth);
    }

    /// <summary>
    /// スキルの時間とタイマーの時間を渡すことで代入すべき値を算出する関数
    /// </summary>
    /// <returns></returns>
    private float ResizeValue(SkillBase skillBase, Timer.Timers timers)
    {
        float defaultSize = 0f;
        float size = 0f;

        switch (timers)
        {
            case Timer.Timers.fiveSec:
                defaultSize = resizeNum[0];
                break;
            case Timer.Timers.sixSec:
                defaultSize = resizeNum[1];
                break;
            case Timer.Timers.sevenSec:
                defaultSize = resizeNum[2];
                break;
            case Timer.Timers.eightSec:
                defaultSize = resizeNum[3];
                break;
            case Timer.Timers.nineSec:
                defaultSize = resizeNum[4];
                break;
            case Timer.Timers.tenSec:
                defaultSize = resizeNum[5];
                break;
            case Timer.Timers.elevenSec:
                defaultSize = resizeNum[6];
                break;
            case Timer.Timers.twelveSec:
                defaultSize = resizeNum[7];
                break;
            case Timer.Timers.thirteenSec:
                defaultSize = resizeNum[8];
                break;
            case Timer.Timers.fourteenSec:
                defaultSize = resizeNum[9];
                break;
            case Timer.Timers.fifteenSec:
                defaultSize = resizeNum[10];
                break;
        }

        if ((int)skillBase.skillTime == 1)
        {
            size = defaultSize;
        }
        else if ((int)skillBase.skillTime == 2)
        {
            size = defaultSize * 2;
        }
        else if ((int)skillBase.skillTime == 3)
        {
            size = defaultSize * 3;
        }
        else if ((int)skillBase.skillTime == 4)
        {
            size = defaultSize * 4;
        }
        else if ((int)skillBase.skillTime == 5)
        {
            size = defaultSize * 5;
        }

        return size;
    }

    /// <summary>
    /// オブジェクトの出現場所調節を行う関数
    /// </summary>
    /// <param name="obj"> バーのオブジェクトリスト </param>>
    /// <param name="changeObj"> 変更を行うオブジェクト </param>>
    private void RePositionObject(List<GameObject> obj, GameObject changeObj)
    {
        // 初回の一回目は無視
        if (obj.Count <= 0)
        {
            return;
        }

        // 一個前の値習得をする変数
        int afterCountNum = 0;
        afterCountNum = obj.Count - 1;
        RectTransform rectObj = obj[afterCountNum].GetComponent<RectTransform>();

        // 一個前の設置済みオブジェクトの右辺情報習得
        // 現在のオブジェクトにx座標の代入
        Vector2 objTransform = Vector2.zero;
        objTransform.x = rectObj.offsetMax.x;
        objTransform.y = rectObj.localPosition.y;

        // 代入
        changeObj.GetComponent<RectTransform>().localPosition = objTransform;
    }

    /// <summary>
    /// リストが追加できるか追加できないかを判断する関数
    /// </summary>
    /// <returns> スキルの合計値が現在の時間を超えている時: true </returns>>
    public static bool AddListCheck(float skill)
    {
        bool isCheck = false;
        
        // バーの中にスキルが全て入っている(リストの中に入っているスキルの時間をみてそれを越している場合追加はできない)
        // スキルリストに入っている全部のスキルの値を足し算をする
        foreach (var skillLists in GameManager.Instance.skillList)
        {
            _additionTime += skillLists.skillTime;
        }
        
        if (_additionTime + skill > Timer._intNowTimerLength)
        {
            Debug.Log("追加できないよ！");
            isCheck = true;
        }
        else
        {
            isCheck = false;
        }

        // 計算が終わったら初期化 
        _additionTime = 0f;
        
        // スキルの値が現在の時間多い時trueを返す
        return isCheck;
    }
    
    /// <summary>
    /// 今まで発動したスキルの合計時間
    /// </summary>
    /// <returns></returns>
    public static float ActivatedSkillTime()
    {
        float addSkillTime = 0f;
        float correctionTime = 0f;

        foreach (var skills in ActiveSkill._readySkillList)
        {
            addSkillTime += skills.skillTime;
        }
        correctionTime = addSkillTime;

        return correctionTime;
    }
}