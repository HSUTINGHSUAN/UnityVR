using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;
using UnityEngine.UI;//namespace 命名空間
    public class ShootingGalleryController : MonoBehaviour// Class 類別
{
    public UIController uiController;// Field 欄位
    public Reticle reticle;
    public SelectionRadial selectionRadial;
    public SelectionSlider selectionSlider;


    public Image timerBar;
    public float gameDuration = 30f;
    public float endDelay = 1.5f;


    public bool IsPlaying// Property 屬性  //屬性做封裝保護  //限制誰可以改這個值
    {
        private set;
        get;//外部只能讀取
    }

    private IEnumerator Start()// Method 方法
    {
        SessionData.SetGameType(SessionData.GameType.SHOOTER180);
        while (true)//true無窮迴圈
        {
            Debug.Log("Start StartPhase");//1
            yield return StartCoroutine(StartPhase());//2
            Debug.Log("Start PlayPhase");//4
            yield return StartCoroutine(PlayPhase());//5
            Debug.Log("Start EndPhase");//7
            yield return StartCoroutine(EndPhase());//8
            Debug.Log("Complete");//10
        }//回到while (true)
    }

    private IEnumerator StartPhase()//3
    {
        yield return StartCoroutine(uiController.ShowIntroUI());
        reticle.Show();//出現紅色點點
        selectionRadial.Hide();//紅色圈圈藏起來
        yield return StartCoroutine(selectionSlider.WaitForBarToFill());//填滿
        yield return StartCoroutine(uiController.HideIntroUI());
    }

    private IEnumerator PlayPhase()//計時器 //6
    {
        yield return StartCoroutine(uiController.ShowPlayerUI());
        IsPlaying = true;
        reticle.Show();
        SessionData.Restart();//重置資料庫遊玩資料

        float gameTimer = gameDuration;
        while (gameTimer > 0f)
        {
            yield return null;//等一帪暫停，下一帪才繼續做
            gameTimer -= Time.deltaTime;//deltaTime -> 一秒
            timerBar.fillAmount = gameTimer / gameDuration;
        }
        IsPlaying = false;
        yield return StartCoroutine(uiController.HidePlayerUI());
    }

    private IEnumerator EndPhase()//9
    {
        reticle.Hide();
        yield return StartCoroutine(uiController.ShowOutroUI());
        yield return new WaitForSeconds(endDelay);
        yield return StartCoroutine(selectionRadial.WaitForSelectionRadialToFill());//selectionRadial -> 紅色圈圈  //反白WaitForSelectionRadialToFill按F12，程式碼就出現囉!!
        yield return StartCoroutine(uiController.HideOutroUI());//UI淡出

    }
}



