using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenIntro : MonoBehaviour
{
    [SerializeField] TMP_Text text, percentage;
    [SerializeField] List<string> textToRead = new();
    [SerializeField] Image loadImage;
    AsyncOperation asyncLoad;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        loadImage.type = Image.Type.Filled;
        loadImage.fillAmount = 0f;
        StartCoroutine(Load());
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;
        asyncLoad = SceneManager.LoadSceneAsync(2);
        while (!asyncLoad.isDone)
        {
            loadImage.fillAmount = asyncLoad.progress;
            percentage.text = (asyncLoad.progress * 100).ToString();
            if(asyncLoad.progress >= 0.9f)
            {
                percentage.text = "100%";
            }
            asyncLoad.allowSceneActivation = false;
            yield return null;
        }
    }

    IEnumerator Load()
    {
        int j = 0;
        while (j < textToRead.Count)
        {
            for (int i = 0; i < textToRead.Count; i++)
            {
                if (j == 3 || j == 6)
                {
                    yield return new WaitForSeconds(1.5f);
                }

                yield return new WaitForSeconds(0.5f);

                text.text = textToRead[i];
                j++;
            }
        }
        asyncLoad.allowSceneActivation = true;
    }
}
