using UnityEngine;

/// <summary>
/// スクリーンの画面をタッチした時に座標や各管理を行うクラス
/// </summary>
public class ScreenTouch
{
    /// <summary>
    /// Androidフラグ
    /// </summary>
    static readonly bool IsAndroid = Application.platform == RuntimePlatform.Android;

    /// <summary>
    /// iOSフラグ
    /// </summary>
    static readonly bool IsIOS = Application.platform == RuntimePlatform.IPhonePlayer;

    /// <summary>
    /// エディタフラグ
    /// </summary> 
    static readonly bool IsEditor = !IsAndroid && !IsIOS;

    /// <summary>
    /// デルタポジション判定用・前回のポジション
    /// </summary>
    static Vector3 prebPosition;

    /// <summary>
    /// タッチ情報を取得(エディタとスマホを考慮)
    /// </summary>
    /// <returns>タッチ情報</returns>
    public static Phase GetPhase()
    {
        if (IsEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                prebPosition = Input.mousePosition;
                return Phase.Began;
            }
            else if (Input.GetMouseButton(0))
            {
                return Phase.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                return Phase.Ended;
            }
        }
        else
        {
            if (Input.touchCount > 0) return (Phase)((int)Input.GetTouch(0).phase);
        }

        return Phase.None;
    }

    /// <summary>
    /// マウスのポジションをvector2で返す
    /// </summary>
    /// <returns> マウスのポジション </returns>>
    public static Vector2 GetMousePosition()
    {
        return Input.mousePosition;
    }

    /// <summary>
    /// タッチポジションを取得(エディタとスマホを考慮)
    /// </summary>
    /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetPosition()
    {
        if (IsEditor)
        {
            if (GetPhase() != Phase.None) return Input.mousePosition;
        }
        else
        {
            if (Input.touchCount > 0) return Input.GetTouch(0).position;
        }

        return Vector3.zero;
    }

    /// <summary>
    /// タッチデルタポジションを取得(エディタとスマホを考慮)
    /// </summary>
    /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetDeltaPosition()
    {
        if (IsEditor)
        {
            var phase = GetPhase();
            if (phase != Phase.None)
            {
                var now = Input.mousePosition;
                var delta = now - prebPosition;
                prebPosition = now;
                return delta;
            }
        }
        else
        {
            if (Input.touchCount > 0) return Input.GetTouch(0).deltaPosition;
        }

        return Vector3.zero;
    }
}

/// <summary>
/// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
/// </summary>
public enum Phase
{
    None = -1,
    Began = 0,              // タッチされている状態
    Moved = 1,              // 移動している状態
    Stationary = 2,         // 停止している状態
    Ended = 3,              // 話している状態
    Canceled = 4            // タッチキャンセル
}