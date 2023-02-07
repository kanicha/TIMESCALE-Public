using UnityEngine;

/// <summary>
/// ダメージを食らった時 テキストに動きをつけるクラス
/// </summary>
public class DamageMove : MonoBehaviour
{
    [SerializeField, Tooltip("アニメーションファイル")]
    private Animator _animator;

    private float animationLength; // アニメーションの長さ保持変数
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator.Play("DamageAnim");
        AnimatorStateInfo infoAnim = _animator.GetCurrentAnimatorStateInfo(0);
        animationLength = infoAnim.length;
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > animationLength)
        {
            Destroy(this.gameObject);
        }
    }
}
