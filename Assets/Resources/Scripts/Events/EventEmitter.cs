using UniRx;
using UniRx.Triggers;

/// <summary>
/// ブローカーくんの所持クラス
/// </summary>
public class EventEmitter : SingletonMonoBehaviour<EventEmitter>
{
    // イベント送受信用のbrokerを用意
    private readonly MessageBroker _broker = new MessageBroker();
    // 外部公開用broker
    public IMessageBroker Broker => _broker;

    private void Start()
    {
        // 削除されたときに自身を削除するように設定
        this.OnDestroyAsObservable()
            .Subscribe(_ => _broker.Dispose())
            .AddTo(this);
    }
}
