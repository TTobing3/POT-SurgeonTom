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

    string scene = "����";

    string[] scripts = new string[] { "������ ���ߴ�", "�̺�, ��!", "���� �ȴٸ� �� �߶󳻰�\n�� �� ���� ����?", "�׷� ������� ����\n�ǻ��� �ҷ��ٰž�!", "�׷��� ����\n�ܰ��ǻ簡 �Ǿ���." };

    private void Start()
    {
        if (DataCarrier.instance != null)
        {
            titleText.gameObject.SetActive(false);
            startText.gameObject.SetActive(false);

            if(DataCarrier.instance.killScore > 10 || DataCarrier.instance.honor < -10)
            {
                cut.sprite = failScene[0];
                scene = "����";
                ShowScreen("����", ()=> { ShowScreen("���� ����� ������߰�\n������ �����ƴ�.", () => { ShowScreen($"����� ���� ��� �� : {DataCarrier.instance.killScore}\n�����  ���� : {DataCarrier.instance.honor}", null); }); });
            }
            else
            {
                cut.sprite = successScene[0];
                scene = "�αͿ�ȭ";
                ShowScreen("�αͿ�ȭ", () => { ShowScreen("��� ������ ���� ġ���Ͽ�\nū ���� �޾Ҵ�.", null); });
            }
        }

    }

    public void Skip()
    {
        if(scene == "����")
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
            scene = "����";

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
        if (scene == "����")
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
        // �ؽ�Ʈ

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
