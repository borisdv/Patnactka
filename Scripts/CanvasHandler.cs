using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour {
    //image that fades the screen 
    public Image fadeImage;
    //moves count text
    public Text MovesCount;

    public Text solvedMovesCount;

    public GameObject solvedMenu;

    public GameObject MainMenu;

	
    ///<summary>
    /// Fading the screen in or out
    /// </summary>
    ///<param name = "fadeIn" > false - represents fading out from color, true - fading in </param>
    ///<param name = "speed" > speed of fading </param>
    public void Fade(bool fadeIn, float speed)
    {
        if (fadeIn)
        {
            StartCoroutine(FadeIn(speed));
        }
        else
        {
            StartCoroutine(FadeOut(speed));
        }

    }
    /// <summary>
    /// Coroutine Fading the screen in 
    /// </summary>
    IEnumerator FadeIn(float speed)
    {
        Color newColor = fadeImage.color;

        for (float t = 0.0f; t < 1f; t += Time.deltaTime/speed)
        {
            newColor.a = t;
            fadeImage.color = newColor;
            yield return null;
        }

    }
    /// <summary>
    /// Coroutine Fading the screen out 
    /// </summary>
    IEnumerator FadeOut(float speed)
    {
        Color newColor = fadeImage.color;

        for (float t = 1f; t > 0f; t -= Time.deltaTime / speed)
        {
            newColor.a = t;
            fadeImage.color = newColor;
            yield return null;
        }

        yield return null;
    }
    /// <summary>
    /// Sets the moves count text 
    /// </summary>
    public void SetMovesCount(int moves)
    {
        MovesCount.text = moves.ToString();
    }

    /// <summary>
    /// Sets the moves count text on the solved menu
    /// </summary>
    public void ShowSolvedMenu(int moves)
    {
        solvedMovesCount.text = "in " + moves.ToString() + " moves !";
        solvedMenu.SetActive(true);
        MainMenu.SetActive(true);
    }
}
