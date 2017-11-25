using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRStandardAssets.Utils;
using VRStandardAssets.Common;

public class ShootingTarget : MonoBehaviour
{
    public int score = 1;
    public float destroyTimeOutDuration = 2f;
    public float timeOutDuration = 2f;
    //宣告event
    public event Action<ShootingTarget> OnRemove; //ShootingTarget -> 註冊參數
    private Transform cameraTransform;
    private AudioSource audioSource;
    private VRInteractiveItem vrInteractiveItem;
    private Renderer mRenderer;
    private Collider mCollider;
    public AudioClip destroyClip;
    public GameObject destroyPrefab;//碎掉後，再產生一個新的，不重複使用
    public AudioClip spawnClip;
    public AudioClip missedClip;

    private bool isEnding;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
        vrInteractiveItem = GetComponent<VRInteractiveItem>();//偵測物體有沒有打到
        mRenderer = GetComponent<Renderer>();
        mCollider = GetComponent<Collider>();

    }

    private void OnEnable()//事件用+=
    {
        vrInteractiveItem.OnDown += HandleDown;
    }

    private void OnDisable()//有+=就要有-=
    {
        vrInteractiveItem.OnDown -= HandleDown;
    }

    private void OnDestroy()
    {
        OnRemove = null;
    }

    private void HandleDown()
    {
        StartCoroutine(OnHit());
    }

    private IEnumerator OnHit()
    {
        if (isEnding)
            yield break;//直接跳脫coroutine
        isEnding = true;
        mRenderer.enabled = false;
        mCollider.enabled = false;
        audioSource.clip = destroyClip;
        audioSource.Play();
        SessionData.AddScore(score);
        GameObject destroyTarget = Instantiate<GameObject>(destroyPrefab,transform.position,transform.rotation);//Instantiate -> 物件產生用的API
        Destroy(destroyTarget, destroyTimeOutDuration);
        yield return new WaitForSeconds(destroyClip.length);//等待音效播放的時間
        if (OnRemove != null)
        {
            OnRemove(this);//觸發事件必須傳入指定參數ShootingTarget
        }
    }

    public void Restart()
    {
        mRenderer.enabled = true;
        mCollider.enabled = true;
        isEnding = false;
        audioSource.clip = spawnClip;
        audioSource.Play();
        transform.LookAt(cameraTransform.position);
        StartCoroutine(MissTarget());//等待2秒
        //StartCoroutine(GameOver());
    }

    private IEnumerator MissTarget()
    {
        yield return new WaitForSeconds(timeOutDuration);//等待秒數
        if (isEnding)
            yield break;
        isEnding = true;
        mRenderer.enabled = false;
        mCollider.enabled = false;
        audioSource.clip = missedClip;
        audioSource.Play();
        yield return new WaitForSeconds(missedClip.length);
        if (OnRemove != null)
            OnRemove(this);
    }

}
