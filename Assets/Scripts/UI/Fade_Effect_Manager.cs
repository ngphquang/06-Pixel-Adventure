using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Fade_Effect_Manager : MonoBehaviour
    {
        [SerializeField] private Image FadeImage;

        public void ScreenFadeEffect(float targetAlpha, float duration, System.Action onComplete = null)
        {
            StartCoroutine(Fade(targetAlpha, duration, onComplete));
        }

        private IEnumerator Fade(float targetAlpha, float duration, System.Action onComplete)
        {
            float time = 0;
            var currentColor = FadeImage.color;
            var startAlpha = FadeImage.color.a;

            while (time < duration)
            {
                time += Time.deltaTime;
                var alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                FadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
                yield return
                    null; //Yield return null says that you need to wait not for seconds, not for amount of frames, but only until the next frame.
                //It will wait for the next frame.
            }

            FadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

            onComplete?.Invoke(); //This will check if onComplete is not equals to null then do a method.
        }
    }
}