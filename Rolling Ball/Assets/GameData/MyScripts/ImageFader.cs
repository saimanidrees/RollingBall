using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ImageFader : MonoBehaviour
{
	public static ImageFader Instance;
	public Image img;
	[SerializeField] private float fadeRate = 1f;
	private void Awake()
	{
		Instance = this;
	}
	private void Start () 
	{	
		StartCoroutine(nameof(FadeOut));
	}
	public void FadeInOut(float delay)
	{
		gameObject.SetActive(true);
		StartCoroutine(nameof(DelayForFadeInOut), delay);
	}
	private IEnumerator DelayForFadeInOut(float waitTime)
	{
		for (float i = 0; i <= 1; i += fadeRate * Time.deltaTime)
		{
			// set color with i as alpha
			img.color = new Color(0, 0, 0, i);
			yield return null;
		}
		img.color = new Color(0, 0, 0, 1);
		yield return new WaitForSeconds(waitTime);
		for (float i = 1; i >= 0; i -= fadeRate * Time.deltaTime)
		{
			// set color with i as alpha
			img.color = new Color(0, 0, 0, i);
			yield return null;
		}
		img.color = new Color(0, 0, 0, 0);
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
	}
	public void FadeIn()
	{
		gameObject.SetActive(true);
		//Debug.Log("FadeIn()");
		StartCoroutine(DelayForFadeIn());
	}
	private IEnumerator DelayForFadeIn()
	{
		for (float i = 0; i <= 1; i += fadeRate * Time.deltaTime)
		{
			// set color with i as alpha
			img.color = new Color(0, 0, 0, i);
			yield return null;
		}
	}
	public void FadeOut()
	{
		//Debug.Log("FadeOut()");
		gameObject.SetActive(true);
		StartCoroutine(DelayForFadeOut());
	}
	private IEnumerator DelayForFadeOut()
	{
		for (float i = 1; i >= 0; i -= fadeRate * Time.deltaTime)
		{
			// set color with i as alpha
			img.color = new Color(0, 0, 0, i);
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		gameObject.SetActive(false);
	}
}