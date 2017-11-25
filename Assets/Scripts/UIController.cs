using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;//UIFader
using VRStandardAssets.Common;//SessionData
using UnityEngine.UI;
public class UIController : MonoBehaviour
{

    public UIFader introUI;//UIFader ->做淡入淡出
    public UIFader outroUI;
    public UIFader playerUI;
    public Text totalScore;
    public Text highScore;

    public IEnumerator ShowIntroUI()
    {
        yield return StartCoroutine(introUI.InteruptAndFadeIn());
    }


    public IEnumerator HideIntroUI()
    {
        yield return StartCoroutine(introUI.InteruptAndFadeOut());
    }

    public IEnumerator ShowOutroUI()
    {
        totalScore.text = SessionData.Score.ToString();
        highScore.text = SessionData.HighScore.ToString();
        yield return StartCoroutine(outroUI.InteruptAndFadeIn());
    }

    public IEnumerator HideOutroUI()
    {
        yield return StartCoroutine(outroUI.InteruptAndFadeOut());
    }


    public IEnumerator ShowPlayerUI()
    {
        yield return StartCoroutine(playerUI.InteruptAndFadeIn());
    }

    public IEnumerator HidePlayerUI()
    {
        yield return StartCoroutine(playerUI.InteruptAndFadeOut());
    }

}