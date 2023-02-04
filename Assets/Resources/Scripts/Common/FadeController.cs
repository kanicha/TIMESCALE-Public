using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 画面のフェードインフェードアウトを処理するクラス
/// </summary>
public class FadeController : SingletonMonoBehaviour<FadeController>
{
    private static Canvas canvas;

    private static Image image;

    private bool _canLoadScene = true;

    /// <summary>
    /// 各初期化
    /// </summary>
    private static void Init()
    {
        GameObject canvasObject = new GameObject("CanvasFade");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canvasObject.AddComponent<GraphicRaycaster>();

        image = new GameObject("ImageFade").AddComponent<Image>();
        image.transform.SetParent(canvas.transform, false);
        image.rectTransform.anchoredPosition = Vector3.zero;
        Camera camera = Camera.main;
        image.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        image.color = Color.clear;
        image.raycastTarget = false;

        DontDestroyOnLoad(canvas.gameObject);
        DontDestroyOnLoad(Instance.gameObject);
        canvasObject.AddComponent<FadeController>();
    }
    
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// シーンをロードする処理
    /// </summary>
    /// <param name="interval"> インターバルの時間 </param>
    /// <param name="gameScene"> 移行先 </param>
     public void LoadScene(float interval, GameScene gameScene)
     {
         if (_canLoadScene)
         {
             StartCoroutine(Fade(interval, gameScene.ToString()));
         }
     }

    private IEnumerator Fade(float interval, string transition2)
    {
        yield return FadeOut(interval);

        yield return SceneManager.LoadSceneAsync(transition2);

        yield return FadeIn(interval);
    }

    private IEnumerator FadeOut(float interval)
    {
        _canLoadScene = false;
        float time = 0f;
        canvas.enabled = true;

        while (time <= interval)
        {
            float fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            image.color = new Color(0.0f, 0.0f, 0.0f, fadeAlpha);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeIn(float interval)
    {
        _canLoadScene = true;
        float time = 0f;
        time = 0f;
        while (time <= interval)
        {
            float fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            image.color = new Color(0.0f, 0.0f, 0.0f, fadeAlpha);
            time += Time.deltaTime;
            yield return null;
        }
        canvas.enabled = false;
    }
}