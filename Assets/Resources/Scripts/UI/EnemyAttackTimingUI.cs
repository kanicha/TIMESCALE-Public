using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーの攻撃タイミングをアイコンで表示する処理クラス
/// </summary>
public class EnemyAttackTimingUI : MonoBehaviour
{
    [SerializeField, Tooltip("出現させるキャンバス")]
    private Transform canvas;
    [SerializeField, Tooltip("入れる親オブジェクト")]
    private Transform parentObj;
    [SerializeField, Tooltip("バフのアイコン画像")] private Sprite buffIcon;

    [Header("生成を行う横幅配列")] 
    [SerializeField] private List<List<GameObject>> createWeightList = new List<List<GameObject>>();
    
    // 本当はもっときれいにしたいです...
    [SerializeField] private List<GameObject> createNumFive = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumSix = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumEightSeven = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumEight = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumNine = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumTen = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumEleven = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumTwelve = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumThirteen = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumFourteen = new List<GameObject>();
    [SerializeField] private List<GameObject> createNumFifteen = new List<GameObject>();

    private List<GameObject> _createdObj = new List<GameObject>();

    private void Awake()
    {
        // リストの初期化
        createWeightList = new List<List<GameObject>>
        {
            createNumFive,
            createNumSix,
            createNumEightSeven,
            createNumEight,
            createNumNine,
            createNumTen,
            createNumEleven,
            createNumTwelve,
            createNumThirteen,
            createNumFourteen,
            createNumFifteen
        };
    }

    /// <summary>
    /// アタックタイミング表示
    /// </summary>
    /// <param name="attackTiming"> アタックタイミングが入っているリスト </param>
    /// <param name="isAttack"> 攻撃どうか </param>
    public void ShowAttackIcon(List<float> attackTiming, bool isAttack)
    {
        // アタックタイミングの値と現在のタイマーの最大値を見る
        foreach (var timing in attackTiming)
        {
            // アタックタイミングが現在のタイマーの最大値を超えていたら終了
            if ((int)timing > Timer._intNowTimerLength)
            {
                return;
            }

            // 過去に生成した カウントが1でもあったら
            if (_createdObj.Count > 0)
            {
                // 削除
                foreach (var t in _createdObj)
                {
                    Destroy(t);
                }

                _createdObj.Clear();
            }
            
            // 生成を行う横幅を保持するリスト
            List<GameObject> createWidthPosition = new List<GameObject>();

          // 生成処理
          // タイマーがまず何秒か習得する(Timer.timerChacker)
          // switch等で分岐
          
          // 生成場所を保持したリストを習得
          createWidthPosition = createWeightList[(int)Timer.TimerChecker()];

          int timingNum = (int)timing;
          timingNum--;
          
          // 生成を行う(canvas)
          // 生成をキャンバスで行う
          _createdObj.Add(Instantiate(createWidthPosition[timingNum], canvas, false)); 
          // 生成を行ったら親子関係の変更を行う
          // 攻撃時間で分岐を行って座標が入っているオブジェクトを親として入れる
          _createdObj.Last().transform.parent = parentObj.transform;
          
          // バフだった場合、画像を変更させる
          if (!isAttack)
          {
              _createdObj[0].GetComponent<Image>().sprite = buffIcon;
          }
        }
    }
}
