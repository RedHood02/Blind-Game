using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSwipeMap : MonoBehaviour
{
	[SerializeField] float screenPercentage;
	[SerializeField] bool canShow, showingMap;
	[SerializeField] Vector2 touchStartPos;
	[SerializeField] float distanceDeadspace;
	[SerializeField] float timer, timerMax;


	[SerializeField] GameObject mapObj;

	private void Update()
	{
		ShowMinimap();
		HideMinimap();
	}


	void ShowMinimap()
	{
		if (showingMap)
		{
			return;
		}

		if (Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);

			if (t.phase == TouchPhase.Began)
			{
				Vector2 touchPos = t.position;

				Vector2 viewportPos = Camera.main.ScreenToViewportPoint(touchPos);
				print(viewportPos.y);
				if (viewportPos.y >= screenPercentage)
				{
					canShow = true;
					touchStartPos = t.position;
					timer = Time.time;
				}
			}

			if (t.phase == TouchPhase.Ended && canShow && (Time.time - timer) < timerMax)
			{
				Vector2 startViewportPos = Camera.main.ScreenToViewportPoint(touchStartPos);
				Vector2 endViewportPos = Camera.main.ScreenToViewportPoint(t.position);

				float distance = startViewportPos.y - endViewportPos.y;
				if (distance >= distanceDeadspace)
				{
					mapObj.SetActive(true);
					showingMap = true;
					FindObjectOfType<Movement>().canMove = false;
				}
			}
		}
	}

	void HideMinimap()
	{
		if (!showingMap)
		{
			return;
		}

		if (Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);

			if (t.phase == TouchPhase.Began)
			{
				canShow = true;
				touchStartPos = t.position;
				timer = Time.time;
			}

			if (t.phase == TouchPhase.Ended && canShow && (Time.time - timer) < timerMax)
			{
				Vector2 startViewportPos = Camera.main.ScreenToViewportPoint(touchStartPos);
				Vector2 endViewportPos = Camera.main.ScreenToViewportPoint(t.position);

				float distance = (endViewportPos.y - startViewportPos.y) * -1;
				Debug.Log("Distance is " + distance);
				if (distance <= distanceDeadspace)
				{
					mapObj.SetActive(false);
					showingMap = false;
					FindObjectOfType<Movement>().canMove = true;
				}
			}
		}
	}
}
