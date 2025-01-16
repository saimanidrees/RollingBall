using GameData.MyScripts;
using UnityEngine;
public class SoundController : MonoBehaviour {
    [SerializeField] private AudioClip btnClickSound, backgroundMusic,
        gameCompleteSound, gameOverSound, parkingSound, buySound,
        ballMergeSound, ballUnmergeSound, ballHitSound, glassBreakSound,
        fellInWaterSound, popupSound, wallBreakingSound;
    [SerializeField]
    private AudioClip rollingBallBgMusic, rollingBallWinSound, rollingBallCoinSound, rollingBallHitSound;
    public AudioSource soundAudioSource, bgAudioSource, extraAudioSource;
    private const string SoundString = "Sound";
	public static SoundController Instance;
    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
        DontDestroyOnLoad (gameObject);
        PlayBackgroundMusic();
    }
    public void PlayBackgroundMusic()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        bgAudioSource.clip = backgroundMusic;
        bgAudioSource.volume = 0.2f;
        bgAudioSource.Play ();
    }
	public void MuteBackgroundMusic(){
		bgAudioSource.clip = null;
		bgAudioSource.Stop ();
	}
    public void MuteSound(){
		soundAudioSource.Stop ();
	}
    public void PlayBtnClickSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        soundAudioSource.clip = btnClickSound;
        soundAudioSource.Play ();
    }
    public void PlayGameCompleteSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = gameCompleteSound;
        soundAudioSource.Play();
    }
    public void PlayGameOverSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = gameOverSound;
        soundAudioSource.Play();
    }
    public void PlayParkingSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = parkingSound;
        soundAudioSource.Play();
    }
    public void PlayBuySound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = buySound;
        soundAudioSource.Play();
    }
    public void PlayGlassBreakSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = glassBreakSound;
        soundAudioSource.Play();
    }
    public void PlayBallMergeSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!extraAudioSource.isPlaying)
            extraAudioSource.clip = ballMergeSound;
        extraAudioSource.Play();
    }
    public void PlayBallUnMergeSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = ballUnmergeSound;
        soundAudioSource.Play();
    }
    public void PlayBallHitSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = ballHitSound;
        soundAudioSource.Play();
    }
    public void PlayBallWaterSplashSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = fellInWaterSound;
        soundAudioSource.Play();
    }
    public void PlayPopupSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!extraAudioSource.isPlaying)
            extraAudioSource.clip = popupSound;
        extraAudioSource.Play();
    }
    public void PlayWallBreakingSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!extraAudioSource.isPlaying)
            extraAudioSource.clip = wallBreakingSound;
        extraAudioSource.Play();
    }
    public void PlayRollingBallBgMusic()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        bgAudioSource.clip = rollingBallBgMusic;
        bgAudioSource.volume = 0.4f;
        bgAudioSource.Play ();
    }
    public void PlayRollingBallCoinSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = rollingBallCoinSound;
        soundAudioSource.Play();
    }
    public void PlayRollingBallHitSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = ballHitSound;
        soundAudioSource.Play();
    }
    public void PlayRollingBallWinSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(SoundString))
            return;
        if (!soundAudioSource.isPlaying)
            soundAudioSource.clip = rollingBallWinSound;
        soundAudioSource.Play();
    }
}