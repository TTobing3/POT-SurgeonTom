using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        StartAmputation();
    }

    [ContextMenu("���� ����")]
    public void StartAmputation()
    {
        AmputationManager.instance.SetPatient();

        if(DataCarrier.instance.isFirst)
        {
            DataCarrier.instance.isFirst = false;
            UIManager.instance.ShowScreen("������ ���� ������ ������\n�����մϴ�.",
                () => { UIManager.instance.ShowScreen("�찮�� ��� ���, \n���� ������ �� �ֽ��ϴ�.", 
                    () => { UIManager.instance.ShowScreen("������ ������ ����\nü���� ��� ��� ȯ�ڴ� �׽��ϴ�.", 
                        () => { UIManager.instance.ShowScreen("��� ������\n������ �� �ֽ��ϴ�.",
                            () => { UIManager.instance.ShowScreen("������ �����Ͽ�\n���� ȹ���Ͻʽÿ�.", 
                                () => { UIManager.instance.ShowScreen("���� ���������,\n���� ġ���Ͽ� �αͿ�ȭ�� ���� ���Դϴ�!",
                                    () => { UIManager.instance.ShowScreen("�ٸ�, ������ �����ϸ�\n���� ���ߵ� ���Դϴ�.",
                                        () => { UIManager.instance.ShowScreen("��! �ø����� ���� ������\n���������� ��ġ�ʽÿ�!",
                                            () => { AmputationManager.instance.StartAmputation();
                                     }); }); }); }); }); }); }); });

        }
        else
        {

            UIManager.instance.ShowScreen("���� ����", () => { AmputationManager.instance.StartAmputation(); });

        }
        // ī�޶�
        UIManager.instance.ZoomCamera(2.5f); //2.1
    }

    public void FinishAmputation(bool isSuccess)
    {
        UIManager.instance.SetFade(true, ()=>
        {
            DOTween.KillAll();

            if (DataCarrier.instance.killScore > 10 || DataCarrier.instance.honor < -10)
            {
                SceneManager.LoadScene("CutScene");
            }
            else if (DataCarrier.instance.honor > 10000)
            {
                SceneManager.LoadScene("CutScene");
            }
            else
            {
                SceneManager.LoadScene("CarriageScene");
            }
        });
    }

}
