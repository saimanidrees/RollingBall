using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GameData.MyScripts;

public class JumpOnEnd : MonoBehaviour
{
    Animator anim;
    public FallOnTiles fallonTiles;
    public GameObject ball;
    public GameObject StickManOpen;
    public CinemachineVirtualCamera BallCam;
    public CinemachineVirtualCamera StickManCam;
    public SkinnedMeshRenderer mesh1;
    public SkinnedMeshRenderer mesh2;

    public int playerValue;
    public Material playerMatt;
    // Start is called before the first frame update
    void Start()
    {
        //OnEnd();
    }

    public void OnEnd()
    {

        ball.transform.position = GamePlayManager.Instance.currentPlayer.transform.position;
        ball.transform.rotation = GamePlayManager.Instance.currentPlayer.transform.rotation;

        StickManOpen.GetComponent<StickManTextureSetting>().SetTextureOffset(PlayerPrefsHandler.BallNumbers.IndexOf(playerValue));
        ball.SetActive(true);
        BallCam.gameObject.SetActive(true);
        mesh1.SetBlendShapeWeight(0, 100);
        mesh2.SetBlendShapeWeight(0, 100);
        anim =StickManOpen.GetComponent<Animator>();
        ball.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10, ForceMode.Impulse);
        fallonTiles.playerValue = playerValue;
        fallonTiles.playerMatt = playerMatt;
        StartCoroutine(BallToStickMan());
    }
    IEnumerator BallToStickMan()
    {
        yield return new WaitForSeconds(1);

        StickManOpen.SetActive(true);
        StickManOpen.transform.position = ball.transform.position;
        StickManOpen.transform.rotation = ball.transform.rotation;
        // SphereCollider coll= StickManOpen.AddComponent<SphereCollider>();
        //coll.radius = 1;
        
        ball.SetActive(false);
        float t = 0;
        anim.SetTrigger("Roll");
        StickManOpen.transform.position = new Vector3(0, 0.3f, ball.transform.position.z);
        StickManOpen.transform.eulerAngles = new Vector3(90, 0, 0);

        while (t < 1)
        {
            StickManOpen.transform.position +=  Vector3.forward * 1f * Time.deltaTime;
            StickManOpen.transform.eulerAngles = Vector3.Lerp(new Vector3(90, 0, 0), Vector3.zero, t / 1);
            mesh1.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, t / 1));
            mesh2.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, t / 1));

            t += Time.deltaTime;
            yield return null;
        }
        mesh1.SetBlendShapeWeight(0, 0);
        mesh2.SetBlendShapeWeight(0, 0);
        BallCam.gameObject.SetActive(false);
        StickManCam.gameObject.SetActive(true);
        anim.SetTrigger("Running");
        t = 0;
        while (t < 1.5f)
        {
            StickManOpen.transform.position += Vector3.forward * 5f * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;

        }
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f);
        iTween.MoveTo(StickManOpen, (StickManOpen.transform.position+Vector3.up*3f + Vector3.forward)*4f, 5f);
        yield return new WaitForSeconds(0.5f);
        StickManOpen.SetActive(false);
        StickManCam.Priority -= 3;

        fallonTiles.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fallonTiles.BreakOnTile();
    }
}
