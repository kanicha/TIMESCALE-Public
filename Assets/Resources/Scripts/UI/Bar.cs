using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 現在どこにいるかを判別するバー処理クラス
/// </summary>
public class Bar : MonoBehaviour
{
    [SerializeField, Tooltip("実際に動かすバーオブジェクト")]
    private GameObject _proglessBar = null;

    [SerializeField, Tooltip("バーが動く始点オブジェクト")]
    private Transform _startPostion = null;

    [SerializeField, Tooltip("バーが動く終点オブジェクト")]
    private Transform _endPostion = null;

    [SerializeField, Tooltip("バーのバックグラウンドイメージオブジェクト")]
    private Image _imageObj = null;

    [SerializeField, Tooltip("画像のイメージ配列")] 
    private Sprite[] _backGroundsImages;
    
    private Tweener _tweener;

    /// <summary>
    /// バーを動かす処理関数
    /// </summary>
    public void MovingBar(int movingNum)
    {
        // 等速直線運動でイベントで渡された値分動く
        _tweener = _proglessBar.transform.DOMove(_endPostion.transform.position, movingNum)
            .SetEase(Ease.Linear);
    }

    /// <summary>
    /// バーの位置を初期化する関数
    /// </summary>
    public void InitBar()
    {
        // 動いてる途中はtransformの変更が効かないため一回 kill で途中停止を行う
        _tweener.Kill();
        _proglessBar.transform.position = _startPostion.transform.position;
    }

    /// <summary>
    /// バーのラインを生成する関数
    /// </summary>
    public void BarLine()
    {
        // 代入
        _imageObj.sprite = _backGroundsImages[(int)Timer.TimerChecker()];
    }
}