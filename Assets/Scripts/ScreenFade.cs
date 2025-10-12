using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.25f;
    public float waitTime = 1.5f;

    public void StartFade()
    {
        Debug.Log("Fading Screen");
        StartCoroutine(FadeToBlackAndBack());
    }

    public IEnumerator FadeToBlackAndBack()
    {
        yield return StartCoroutine(Fade(0f, 1f)); // Fade to black
        yield return new WaitForSeconds(waitTime); // Wait while black
        yield return StartCoroutine(Fade(1f, 0f)); // Fade back in
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }
        // Ensure exact final value
        color.a = endAlpha;
        fadeImage.color = color;
    }
}
