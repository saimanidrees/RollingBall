using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GameData.MyScripts;
using UnityEngine.Animations;

public class StickManController : MonoBehaviour
{
    Animator anim;
    public float runningDuration;
    public int playerLevel;
    public SkinnedMeshRenderer mesh1, mesh2;
    public Material playerMatt;
    [SerializeField] private Color[] colors = { Color.gray, Color.red,Color.gray,Color.green,Color.blue,Color.cyan,Color.magenta};
    
    public GameObject ball;
    public GameObject StickManOpen;
    public GameObject cannon;
    public GameObject cannonFunnel;
    public GameObject RingsLast;
    public GameObject downToUpTiles;
    public GameObject upToDownTiles;

    // Particles
    public GameObject funnelParticles;
    public GameObject shootVFX;
    [Header("Starting Cams")]
    //public Camera startStickManCam;
    
    // CineMachine
    public CinemachineVirtualCamera BallCam;
    public CinemachineVirtualCamera StickManCam;
    public CinemachineVirtualCamera CannonCam;
    public CinemachineVirtualCamera finalCam;
    public CinemachineVirtualCamera downToUpCam;

    public Transform tempCanonPosition;

    public GameObject smokeEffect;
    public bool start;
    public bool end;
    public bool upToDown=true;
    public bool win;
   
    private void Start()
    {   /*if (start) {
            onStart.Invoke();
        }
        else if (end)
        {
            onEnd.Invoke();
        }*/
        /*if(start)
            OnStart();*/
    }
    /*private void OnEnable()
    {
        onStart += OnStart;
        onEnd += OnEnd;
    }
    private void OnDisable()
    {
        onStart -= OnStart;
        onEnd -= OnEnd;
    }*/
    #region Starting Transition
    public void OnStart()
    {
        //startStickManCam.gameObject.SetActive(true);
        anim = StickManOpen.GetComponent<Animator>();
        StickManOpen.SetActive(true);
        mesh1.SetBlendShapeWeight(0, 0);
        mesh2.SetBlendShapeWeight(0, 0);
        if (anim != null) {
            StartCoroutine(RunToBall());
        }
    }
    IEnumerator RunToBall() {
        float t = 0;
        anim.SetTrigger("Running");
        while (t < runningDuration) {
            transform.position += Vector3.forward * 2f*Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("Roll");
        t = 0;
        ball.transform.localScale = Vector3.zero;
        ball.SetActive(true);
       // startStickManCam.LookAt = ball.transform;
        //startStickManCam.Follow = ball.transform;
        while (t < 1.2)
        {
            transform.position += Vector3.forward * 2f * Time.deltaTime;
            mesh1.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, t / 1.2f));
            mesh2.SetBlendShapeWeight(0, Mathf.Lerp(0, 100, t / 1.2f));
            StickManOpen.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one*0.5f, t / 1.2f);
            ball.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 0.5f, t / 1.2f);
            ball.transform.eulerAngles = Vector3.Lerp(Vector3.zero, Vector3.right * 180, t / 1.2f);
            t += Time.deltaTime;
            yield return null;
        }
              
        mesh1.SetBlendShapeWeight(0, 100);
        mesh2.SetBlendShapeWeight(0, 100);
        t = 0;
        while (t < 0.2)
        {
            ball.transform.localScale = Vector3.Lerp( Vector3.one*0.5f, Vector3.one,t/0.2f);
            ball.transform.eulerAngles = Vector3.Lerp(Vector3.zero, Vector3.right * 180, t / 0.2f);
            t += Time.deltaTime;
            yield return null;
        }

        anim.enabled = false;
        StickManOpen.SetActive(false);
        ball.SetActive(true);
        ball.GetComponent<Rigidbody>().AddForce(Vector3.forward * 15f, ForceMode.Impulse);
        //Camera transition;
        t = 0;
        /*Vector3 initialPos = startStickManCam.gameObject.transform.position;
        Vector3 targetPos = GamePlayManager.Instance.playerCamera.transform.position;
        Debug.Log("Initial position:"+initialPos+"targetPosition"+targetPos);
        while (t < 1) {
            startStickManCam.gameObject.transform.position = Vector3.Lerp(initialPos,targetPos,t/1f);
            t += Time.deltaTime;
            yield return null;
        }
        startStickManCam.gameObject.SetActive(false);*/
        GamePlayManager.Instance.playerCamera.SetActive(true);
       
      
        //startStickManCam.Priority -= 10;

    }
    #endregion

    #region Ending Transition
    public void OnEnd() {
        ball.SetActive(true);
        Debug.Log("oN eND");
        //BallCam.gameObject.SetActive(true);
        GamePlayManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().Priority -= 15;
        cannon.SetActive(true);
        mesh1.SetBlendShapeWeight(0, 100);
        mesh2.SetBlendShapeWeight(0, 100);
      //  GamePlayManager.Instance.playerCamera.GetComponent<CinemachineBrain>().enabled = true;
        anim = StickManOpen.GetComponent<Animator>();
        ball.GetComponent<Rigidbody>().AddForce(Vector3.forward*5,ForceMode.Impulse);
        StartCoroutine(BallToStickMan());
    }
     public   bool stopApplyingForce;
    IEnumerator BallToStickMan()
    {
        Debug.Log("ball to to stick");
    
        StickManOpen.transform.position = ball.transform.position;
        StickManOpen.transform.rotation = ball.transform.rotation;
        yield return new WaitForSeconds(1f);
        StickManOpen.SetActive(true);
        smokeEffect.SetActive(true);
        ball.SetActive(false);
        float t = 0;
        anim.SetTrigger("Roll");
        StickManOpen.transform.position = new Vector3(0, 0.3f, ball.transform.position.z);
        StickManOpen.transform.eulerAngles = new Vector3(90, 0, 0);
        while (t < 1)
        {
            StickManOpen.transform.position +=  Vector3.forward * 1f * Time.deltaTime;
            StickManOpen.transform.eulerAngles = Vector3.Lerp(new Vector3(90,0,0), Vector3.zero, t / 1);
            mesh1.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, t / 1));
            mesh2.SetBlendShapeWeight(0, Mathf.Lerp(100, 0, t / 1));
            t += Time.deltaTime;
            yield return null;
        }
        smokeEffect.SetActive(false);
        mesh1.SetBlendShapeWeight(0, 0);
        mesh2.SetBlendShapeWeight(0, 0);
        BallCam.Priority -= 5;
        StickManCam.gameObject.SetActive(true);
        anim.SetTrigger("Running");
        t = 0;
        while (t < 1f)
        {
            StickManOpen.transform.position += Vector3.forward * 5f * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;

        }
        anim.SetTrigger("Jump");
        t = 0;
        Vector3 initialPos = StickManOpen.transform.position;
        Quaternion initialRot = StickManOpen.transform.rotation;

        yield return new WaitForSeconds(0.4f);
        // StickManCam.gameObject.SetActive(false);
        // CannonCam.gameObject.SetActive(true);
        //anim.SetTrigger("Fly");
        while (t < 1f)
        {
            StickManOpen.transform.position = Vector3.Lerp(initialPos, tempCanonPosition.position, t / 1);
            StickManOpen.transform.rotation = Quaternion.Slerp(initialRot, tempCanonPosition.rotation, t / 1);
            t += Time.deltaTime;
            yield return null;

        }

        StickManOpen.transform.parent = cannonFunnel.transform;
        t = 0;
        initialRot = cannonFunnel.transform.localRotation;
        Vector3 rot = new Vector3(-90, 0, 0);



        yield return new WaitForSeconds(0.5f);

        Animator cannonAnim = cannon.GetComponent<Animator>();

        if (upToDown)
        {
            cannonAnim.SetTrigger("Rotate120");
            upToDownTiles.SetActive(true);
        }
        else {
            cannonAnim.SetTrigger("Rotate90");
            downToUpTiles.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        funnelParticles.SetActive(true);
        CannonCam.gameObject.SetActive(false);
        StickManCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        t = 0;
        StickManOpen.transform.parent = null;
        initialPos = StickManOpen.transform.position;
        shootVFX.gameObject.SetActive(true);

      
        Rigidbody rb = StickManOpen.AddComponent(typeof(Rigidbody)) as Rigidbody;

      
       // anim.SetTrigger("Fly");
       // rb.freezeRotation = true;


        if (!upToDown)
        {
            downToUpCam.gameObject.SetActive(true);
            StickManOpen.transform.localEulerAngles = new Vector3(-90, 0, 0);
            rb.useGravity = false;
            rb.freezeRotation = true;
            anim.SetTrigger("Fly"); StickManCam.gameObject.SetActive(false);
            StickManOpen.GetComponent<BoxCollider>().enabled = true;
            //rb.AddRelativeForce((Vector3.up) * 15f, ForceMode.Impulse);
            //rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            while (t < 6)
            {
                if (stopApplyingForce)
                {
                    t = 7;
                    continue;
                }
                // rb.AddExplosionForce(10f, StickManOpen.transform.position, 5f);
                // rb.AddForce(Vector3.up * 10, ForceMode.Force);
               
               StickManOpen.transform.position = Vector3.Lerp(initialPos, RingsLast.transform.position, t / 6);
                t += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            finalCam.gameObject.SetActive(true);
            rb.useGravity = true;
            rb.freezeRotation = true;
            rb.AddRelativeForce((Vector3.up) * 16f, ForceMode.Impulse);
            yield return new WaitForSeconds(1f);
            StickManOpen.GetComponent<BoxCollider>().enabled = true;
            anim.SetTrigger("Falling");
            t = 0;
            initialRot = StickManOpen.transform.rotation;
            StickManOpen.transform.eulerAngles = new Vector3(0, StickManOpen.transform.eulerAngles.y,0);
            /*while (t < 1)
             {
                StickManOpen.transform.eulerAngles =Vector3.Lerp(initialRot.eulerAngles, new Vector3(360, StickManOpen.transform.eulerAngles.y,StickManOpen.transform.eulerAngles.z),t/1f);
                t += Time.deltaTime;
                yield return null;
            }*/

            /*while (t < 5)
            {
                if (stopApplyingForce)
                {
                    t = 6;
                    continue;
                }
                // rb.AddExplosionForce(10f, StickManOpen.transform.position, 5f);
                // rb.AddForce(Vector3.up * 10, ForceMode.Force);
                //StickManOpen.transform.position = Vector3.Lerp(initialPos, RingsLast.transform.position, t / 5);
                t += Time.deltaTime;
                yield return null;
            }*/
        }
    }
        public void PerformLastStunt()
    {
        StickManCam.gameObject.SetActive(false);
        if (upToDown)
        {
            anim.SetTrigger("Land");
            Debug.Log("PerformLastStunt");
            GamePlayManager.Instance.GameComplete(2f);
        }
        else {            
            StartCoroutine(RotateBack());
        }
    }

        private IEnumerator RotateBack() {
        float t = 0;
        anim.SetTrigger("FlyHit");
        yield return new WaitForSeconds(1f);
       
        var initialRot = StickManOpen.transform.rotation;
        while (t < 2f) {

           //StickManOpen.transform.LookAt(downToUpCam.transform,Vector3.up);// = Quaternion.RotateTowards(StickManOpen.transform.rotation, Quaternion.Inverse(downToUpCam.transform.rotation), t/2f);
           StickManOpen.transform.rotation = Quaternion.Slerp(initialRot,downToUpTiles.transform.GetChild(0).transform.rotation,t/2f);

            t += Time.deltaTime;
            yield return null;
        }
        if (!win)
        {
            anim.SetTrigger("Argue");
        }
        else {
            anim.SetTrigger("Win");
        }
        Debug.Log("RotateBack");
        GamePlayManager.Instance.GameComplete(2f);
    }
    public void HideAllVirtualCameras()
    {
        BallCam.gameObject.SetActive(false);
    }
    
    #endregion
}
