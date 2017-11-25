using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;//要用這個命名空間才找得到VRInput這個類別
using UnityEngine.VR;
public class ShootingGunController : MonoBehaviour
{
    public AudioSource audioSource;
    public VRInput vrInput;//公開;類別;變數名稱
    public ParticleSystem flareParticle;
    public LineRenderer gunFlare;
    public Transform gunEnd;
    public float defaultLineLength = 70f;
    public float gunFlareVisibleSeconds = 0.07f;
    private void OnEnable()
    {
        vrInput.OnDown += HandleDown;
    }


    private void OnDisable()
    {
        vrInput.OnDown -= HandleDown;
    }

     private void HandleDown()
    {

        StartCoroutine(Fire());

    }

    private IEnumerator Fire()
    {
        audioSource.Play();
        float lineLength = defaultLineLength;//射線長度
        //TODO判斷有無射到東西
        flareParticle.Play();
        gunFlare.enabled = true;
        yield return StartCoroutine(MoveLineRenderer(lineLength));//開始做MoveLineRenderer腳本
        gunFlare.enabled = false;//上行完成才會往下走

    }

    private IEnumerator MoveLineRenderer(float lineLength)
    {
        float timer = 0f;
        while (timer < gunFlareVisibleSeconds)
        {
            gunFlare.SetPosition(0, gunEnd.position);
            gunFlare.SetPosition(1, gunEnd.position + gunEnd.forward * lineLength);
            yield return null;//等待
            timer += Time.deltaTime;//迴圈回去判斷
        }
    }
}


/*解釋Coroutine
private void HandleDown()
{
    Debug.Log("A");//先被呼叫
    StartCoroutine(Fire());//到C
    Debug.Log("B");//進到IEnumerator 

}

private IEnumerator Fire()
{
    Debug.Log("C");
    yield return null;//做到這裡結束，跳出去到B //yield->暫停這個方法執行到哪裡
    Debug.Log("D");//接續B
}
*/
