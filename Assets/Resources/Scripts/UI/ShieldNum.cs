using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using UnityEngine.UI;

/// <summary>
/// シールドの残り数の表示を行うクラス
/// </summary>
public class ShieldNum : MonoBehaviour
{
    void Start()
    {
        this.ObserveEveryValueChanged(x => PlayerManager.playerShield)
            .Where(_ => !StateManager.HasFlag(StateList.PlayerState.Init))
            .Subscribe(_ =>
            {
                this.GameObject().GetComponent<Text>().text = PlayerManager.playerShield.ToString();
            }).AddTo(this);
    }
}
