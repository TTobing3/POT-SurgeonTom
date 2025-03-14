using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class CutSceneManager : MonoBehaviour
{
    public Sprite[] startScenes, failScene, successScene;

    public SpriteRenderer cut;

    public CanvasGroup panelCanvasGroup;
    public TextMeshProUGUI panelText, titleText, startText;
    public Image fade;

    int index = 0;

    string scene = "시작";

    string[] scripts = new string[] { "제리는 말했다", "이봐, 톰!", "썩은 팔다리 좀 잘라내고\n돈 벌 생각 없나?", "그럼 사람들이 톰을\n의사라고 불러줄거야!", "그렇게 톰은\n외과의사가 되었다." };

    private void Start()
    {
        if (DataCarrier.instance != null)
        {
            titleText.gameObject.SetActive(false);
            startText.gameObject.SetActive(false);

            if(DataCarrier.instance.killScore > 10 || DataCarrier.instance.honor < -10)
            {
                cut.sprite = failScene[0];
                scene = "사형";
                ShowScreen("사형", ()=> { ShowScreen("톰은 마녀로 지목당했고\n제리는 도망쳤다.", () => { ShowScreen($"당신이 죽인 사람 수 : {DataCarrier.instance.killScore}\n당신의  평판 : {DataCarrier.instance.honor}", null); }); });
            }
            else
            {
                cut.sprite = successScene[0];
                scene = "부귀영화";
                ShowScreen("부귀영화", () => { ShowScreen("톰과 제리는 왕을 치료하여\n큰 상을 받았다.", null); });
            }
        }

    }

    public void Skip()
    {
        if(scene == "시작")
        {
            SetFade(true, () => SceneManager.LoadScene("MainScene"));
        }
        else
        {
            Destroy(DataCarrier.instance.gameObject);
            SetFade(true, () => SceneManager.LoadScene("CutScene"));
        }
    }

    public void GameStart()
    {
        if (index > 0) return;

        if (DataCarrier.instance == null)
        {
            scene = "시작";

            titleText.DOFade(0,1);
            startText.DOFade(0, 1);
            ShowScreen(scripts[index], () =>
            {
                NextCut();
            });

        }

    }

    public void NextCut()
    {
        if (scene == "시작")
        {
            if(index < startScenes.Length - 1)
            {
                SetFade(true, () => 
                {
                    index++;
                    cut.sprite = startScenes[index];
                    SetFade(false, null);
                    ShowScreen(scripts[index], () => NextCut());
                });
            }
            else if (index == startScenes.Length - 1)
            {
                SetFade(true, () => SceneManager.LoadScene("MainScene"));
            }
        }
    }

    public void SetFade(bool isFadeOut, System.Action action)
    {
        if (isFadeOut)
        {
            fade.color = new Color(0, 0, 0, 0);
            fade.gameObject.SetActive(true);

            fade.DOFade(1, 0.5f).OnComplete(() =>
            {
                if (action != null) action();
            });
        }
        else
        {

            fade.color = new Color(0, 0, 0, 1);
            fade.gameObject.SetActive(true);

            fade.DOFade(0, 0.5f).OnComplete(() =>
            {
                fade.gameObject.SetActive(false);
            });
        }
    }


    public void ShowScreen(string text, System.Action action)
    {
        // 텍스트

        panelText.text = text;
        panelCanvasGroup.alpha = 0;

        panelCanvasGroup.gameObject.SetActive(true);

        panelCanvasGroup.DOFade(1, 0.5f).SetDelay(1).OnComplete(() =>
        {
            panelCanvasGroup.DOFade(0, 0.5f).SetDelay(1f).OnComplete(() =>
            {
                panelCanvasGroup.gameObject.SetActive(false);
                if (action != null)
                    action();
            });
        });

    }
}
