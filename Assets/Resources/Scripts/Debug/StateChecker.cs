#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI上にステートの情報を表示させるスクリプト : デバッグ用
/// </summary>
public class StateChecker : MonoBehaviour
{
    // ステートを保持するリスト
    private readonly List<StateList.PlayerState> _stateList = new List<StateList.PlayerState>();
    
    /// <summary>
    /// ステート表示を行う関数
    /// </summary>
    public void ViewState()
    {
        // foreachで一個一個要素を取り出していく
        foreach (var stateString in Enum.GetNames(typeof(StateList.PlayerState)))
        {
            // 文字列で持ってきた値をキャストする
            var playerState =
                (StateList.PlayerState)Enum.Parse(typeof(StateList.PlayerState), stateString);
            
            // 入っていたものがtrueだった場合 かつ リストに対象物のstateがなかった場合リストに追加する
            if (StateManager.HasFlag(playerState) && 
                !_stateList.Contains(playerState))
            {
                _stateList.Add(playerState);
            }
            // 入っていたものがfalseだった時 かつ りすとに対象物のstateがあった場合リストから削除する
            else if (!StateManager.HasFlag(playerState) &&
                     _stateList.Contains(playerState))
            {
                _stateList.Remove(playerState);
            }
        }

        // ログの表示
        foreach (var sl in _stateList)
        {
            Debug.Log("======================== \n" + 
                      sl + "\n");
        }
    }
}

#endif