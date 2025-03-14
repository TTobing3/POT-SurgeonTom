using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.U2D.Animation;

public enum SliderType {  Life, Skin, Bone };

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("화면")]
    public CanvasGroup panelCanvasGroup;
    public TextMeshProUGUI panelText;
    public Image fade;
    public TextMeshProUGUI tierText, nameText;

    [Header("적 UI")]
    public Slider patientLifeSlider;
    public Slider patientSkinSlider, patientBoneSlider;

    [Header("장비 UI")]
    public List<Transform> weaponButtons; 

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
        SetWeaponButton();
        ChangeWeaponButton(0);
    }

    void Update()
    {
        if(AmputationManager.instance != null)
        {
            UpdateHpBar(patientLifeSlider, (int)AmputationManager.instance.patient.life);
            UpdateHpBar(patientSkinSlider, (int)AmputationManager.instance.patient.skinHp);
            UpdateHpBar(patientBoneSlider, (int)AmputationManager.instance.patient.boneHp);
        }
    }

    #region Slider

    void UpdateHpBar(Slider hpBar, int hp)
    {
        if (hpBar.value > hp) hpBar.value = hpBar.value > hp ? hpBar.value - ((hpBar.value - hp) * 0.1f) : hp;
        else if (hpBar.value < hp) hpBar.value = hpBar.value < hp ? hpBar.value + ((hpBar.value + hp) * 0.005f) : hp;
    }

    public void SetSliderMaxValue(SliderType type, int maxValue)
    {
        switch (type)
        {
            case SliderType.Life:
                patientLifeSlider.maxValue = maxValue;
                break;

            case SliderType.Skin:
                patientSkinSlider.maxValue = maxValue;
                break;

            case SliderType.Bone:
                patientBoneSlider.maxValue = maxValue;
                break;
        }
    }

    #endregion

    #region Screen

    public void SetPatientInfo(PatientData patientData)
    {
        tierText.text = "{ "+$"{patientData.tier}, {patientData.job}"+" }";
        nameText.text = $"{patientData.name}";
    }
    public void ShowScreen(string text, System.Action action)
    {
        // 텍스트

        panelText.text = text;
        panelCanvasGroup.alpha = 0;

        panelCanvasGroup.gameObject.SetActive(true);

        panelCanvasGroup.DOFade(1, 1).OnComplete(()=>
        {
            panelCanvasGroup.DOFade(0, 1).SetDelay(1f).OnComplete(()=>
            {
                panelCanvasGroup.gameObject.SetActive(false);
                if (action != null) 
                    action();
            });
        });

    }

    public void SetFade(bool isFadeOut, System.Action action)
    {
        if(isFadeOut)
        {
            fade.color = new Color(0, 0, 0, 0);
            fade.gameObject.SetActive(true);

            fade.DOFade(1, 1).OnComplete(()=> 
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

    #endregion

    #region Camera
    public void ZoomCamera(float size)
    {
        Camera.main.DOOrthoSize(size, 3);
    }

    #endregion

    #region Button

    public void SetWeaponButton()
    {
        for (int i = 0; i < 2; i++)
        {
            if (i < DataCarrier.instance.weapons.Count)
            {
                weaponButtons[i].gameObject.SetActive(true);

                weaponButtons[i].GetComponentInChildren<SlotImageAdapter>().ImageChange("WeaponIcon", DataCarrier.instance.weapons[i].name);

            }
            else
            {
                weaponButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChangeWeaponButton(int i)
    {
        print(i + "변경");
        Surgeon.instance.ChangeWeapon(DataCarrier.instance.weapons[i]);
        AudioManager.instance.PlayAudioSource(weaponButtons[i].GetComponentInChildren<AudioSource>());
    }

    #endregion
}
