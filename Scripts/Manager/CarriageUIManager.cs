using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class CarriageUIManager : MonoBehaviour
{
    public static CarriageUIManager instance;

    [Header("ȭ��")]
    public Image fade;

    [Header("��ȭ UI")]
    public TextMeshProUGUI killScore;
    public TextMeshProUGUI goldText, honorText;

    [Header("���� �Ƿڼ� UI")]
    public RectTransform amutationPaper;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI tierText, jobText, statusText, historyText;

    [Header("���ȼ� UI")]
    public RectTransform proposePaper;
    public TextMeshProUGUI descriptionText, proposeText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        SetFade(false, null);

        ZoomCamera(4);

        if (CarriageManager.instance != null && DataCarrier.instance != null)
        {
            SetPropertyText();
        }
    }

    public void SetPropertyText()
    {
        killScore.DOText(DataCarrier.instance.killScore + "", 1);
        goldText.DOText(DataCarrier.instance.gold + "", 1);
        honorText.DOText(DataCarrier.instance.honor + "", 1);

    }

    public void SetFade(bool isFadeOut, System.Action action)
    {
        if (isFadeOut)
        {
            fade.color = new Color(0, 0, 0, 0);
            fade.gameObject.SetActive(true);

            fade.DOFade(1, 1).OnComplete(() =>
            {
                if (action != null) action();
            });
        }
        else
        {

            fade.color = new Color(0, 0, 0, 1);
            fade.gameObject.SetActive(true);

            fade.DOFade(0, 1).OnComplete(() =>
            {
                fade.gameObject.SetActive(false);
            });
        }
    }

    public void ZoomCamera(float size)
    {
        Camera.main.DOOrthoSize(size, 3);
    }

    public void SetPatientText(PatientData patientData)
    {
        amutationPaper.anchoredPosition = new Vector2(0, -1200);

        amutationPaper.DOAnchorPosY(0, 2).SetEase(Ease.OutBack);

        tierText.text = "��� : "+patientData.tier;
        jobText.text = "���� : " + patientData.job;

        nameText.text = "�̸� : " + patientData.name;

        statusText.text = "Ư�̻��� : " + patientData.status;
        historyText.text = "[ ��� ]\n" + patientData.history;

        historyText.text += $"\n\n�� ��{patientData.honor} ��ȭ {patientData.gold}";
        if (DataCarrier.instance.killScore + 1 > 10 || DataCarrier.instance.honor - patientData.honor < -10)
            historyText.text += "\n\n[ ���� �� ���� ]";
    }

    public void SetProposeText(WeaponData curWeaponData, WeaponData weaponData)
    {
        proposePaper.anchoredPosition = new Vector2(0, -1200);

        proposePaper.DOAnchorPosY(0, 2).SetEase(Ease.OutBack);

        proposeText.text = $"����� {curWeaponData.name}(��)��\n" +
            $"��ȭ {weaponData.cost}���� �شٸ�\n" +
            $"���� {weaponData.name}(��)�� �ְڼ�.";

        descriptionText.text = $"{weaponData.name}\n\n" +
            $"{weaponData.description}";
    }

    public void DumpProposePaper()
    {
        print("ġ��");
        proposePaper.DOAnchorPosY(-1200, 1).SetEase(Ease.OutBack);
    }
}
