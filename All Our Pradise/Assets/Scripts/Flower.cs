using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flower : MonoBehaviour
{
	public float lifespan = 3;
	public UnityEvent onFinishEvent;

	private Vector3 origScale = Vector3.zero;
	private float countdown;

	void OnEnable()
	{
		StartCoroutine(Grow());
	}

	public void Refresh()
	{
		countdown = lifespan;
	}

    IEnumerator Grow()
    {
		if (origScale == Vector3.zero)
			origScale = transform.localScale;
		
		transform.localScale = Vector3.zero;

		for (float t = 0; t < 1; t += Time.deltaTime)
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, origScale, Mathf.SmoothStep(0, 1, t));
			yield return null;
		}

		transform.localScale = origScale;
		countdown = lifespan;
		StartCoroutine(Countdown());
    }

	IEnumerator Countdown()
    {
		while (countdown > 0)
		{
			countdown -= Time.deltaTime;
			yield return null;
		}

		StartCoroutine(Shrink());
	}

	IEnumerator Shrink()
    {
		for (float t = 0; t < 1; t += Time.deltaTime)
		{
			transform.localScale = Vector3.Lerp(Vector3.zero, origScale, Mathf.SmoothStep(1, 0, t));
			yield return null;
		}

		transform.localScale = Vector3.zero;
		onFinishEvent.Invoke();
    }
}
