using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;//要用這個命名空間才找得到VRInput、Reticle這個類別
using UnityEngine.VR;//Unity提供API
public class ShootingGunController : MonoBehaviour
{
    public AudioSource audioSource;
    public VRInput vrInput;//公開;類別;變數名稱
    public ParticleSystem flareParticle;
    public LineRenderer gunFlare;
    public Transform gunEnd;

    public Transform cameraTransform;
    public Reticle reticle;//準心
    public Transform gunContainer;
    //下3->移動參數
    public float damping = 0.5f;//手追隨攝影機所需時間參數;
    public float dampingCoef = -20f;
    public float gunContainerSmooth = 10f;
    //-----------------------------------------------------------
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
            gunFlare.SetPosition(0, gunEnd.position);//射線起點
            gunFlare.SetPosition(1, gunEnd.position + gunEnd.forward * lineLength);//射線終點
            yield return null;//等待
            timer += Time.deltaTime;//回去(timer < gunFlareVisibleSeconds)判斷
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, InputTracking.GetLocalRotation(VRNode.Head),damping * (1-Mathf.Exp(dampingCoef * Time.deltaTime)));// InputTracking->算出A-B旋轉時間差
        transform.position = cameraTransform.position;
        //上2->身體移動
        Quaternion lookAtRotation = Quaternion.LookRotation(reticle.ReticleTransform.position - gunContainer.position);  // Quaternion -> 旋轉四元素
        gunContainer.rotation = Quaternion.Slerp(gunContainer.rotation, lookAtRotation, gunContainerSmooth * Time.deltaTime);
        //上2->槍的移動

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
