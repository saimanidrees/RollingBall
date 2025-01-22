using GameData.MyScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class LoadingScript : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private UnityEvent[] animationEvents;
    public void InvokeAnimationEvent(int eventIndex)
    {
        animationEvents[eventIndex].Invoke();
    }
    public static void SwitchScene()
    {
        PlayerPrefsHandler.CurrentMode = 2;
        SceneManager.LoadScene(PlayerPrefsHandler.CurrentMode + 1);
    }
}