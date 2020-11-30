using System.Collections;
using UnityEngine;

public class GenericAnimationEvents : MonoBehaviour
{
	public void SystemPause()
	{
		GameManager.instance.SystemPause();
	}

	public void SystemResume()
	{
		GameManager.instance.SystemResume();
	}

	public void LoadScene(int buildIndex)
	{
		SceneLoader.instance.LoadScene(buildIndex);
	}

	//public void FadeToBlack(float duration)
	//{
	//	ScreenFader.instance.Fade(ScreenFader.FadeType.OUT, false);
	//}

	//public void FadeIn()
	//{
	//	ScreenFader.instance.Fade(ScreenFader.FadeType.IN, false); 
	//}

	public void FadeAnimEvent(float duration)
	{
		StopAllCoroutines();
		StartCoroutine(Fadeout(duration)); 
	}


	private IEnumerator Fadeout(float duration)
	{
		StopAllCoroutines();
		ScreenFader.instance.FadeCo(ScreenFader.FadeType.OUT, false);
		yield return new WaitForSeconds(duration);
		ScreenFader.instance.FadeCo(ScreenFader.FadeType.IN, false); 
	}

	//private IEnumerator WaitForSecond(float seconds)
	//{
	//	yield return new WaitForSeconds(seconds);
	//}
}
