using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using DG.Tweening;

public enum HitType { Miss, Hit, Perfect}

public enum PatientType { Common, Noble, King }

public enum HitPointType { Arm, Leg }

public class Patient : MonoBehaviour
{
    Animator animator;

    [Header("스텟")]
    public PatientData patientData;
    public float life;
    public float skinHp, boneHp, maxLife, maxSkinHp, maxBoneHp;
    public HitPointType hitPointType;

    [Header("스프라이트")]
    public SpriteRenderer[] bodyRenderers;
    public SpriteResolver headResolver, hatResolver, jacketResolver;
    public GameObject jacketWhite;
    public Transform[] hitboxes;

    SpriteRenderer hatRenderer, jacketRenderer;


    [Header("피격음")]
    public AudioSource headAudioSource;
    public AudioClip[] screamClips;
    public ScreamText screamText;

    bool isBleed = false, isFinish = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        hatRenderer = hatResolver.GetComponent<SpriteRenderer>();
        jacketRenderer = jacketResolver.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
    }

    #region Set

    public void Set(PatientData patientData)
    {
        this.patientData = patientData;
        print(patientData.name);

        hitPointType = (HitPointType)Random.Range(0, 2);
        animator.SetInteger("hitPointType", (int)hitPointType);

        maxLife = patientData.life;
        maxSkinHp = patientData.skin;
        maxBoneHp = patientData.bone;

        life = maxLife;
        skinHp = maxSkinHp;
        boneHp = maxBoneHp;

        UIManager.instance.SetSliderMaxValue(SliderType.Life, (int)maxLife);
        UIManager.instance.SetSliderMaxValue(SliderType.Skin, (int)maxSkinHp);
        UIManager.instance.SetSliderMaxValue(SliderType.Bone, (int)maxBoneHp);

        UIManager.instance.SetPatientInfo(patientData);

        SetSprite(patientData.tier);

        StartCoroutine(CoSetHitBox());

        animator.SetInteger("hitType", (int)HitType.Perfect);
        animator.SetTrigger("hit");
    }

    void SetSprite(string tier)
    {
        jacketWhite.SetActive(false);

        var color = Color.white;

        float[] colorValue = new float[] { Random.Range(0.4f, 0.7f), Random.Range(0.2f, 0.5f), Random.Range(0, 1f) };

        Color[] colors = new Color[]{
                new Color(colorValue[0] + Random.Range(-0.1f, 0.1f), colorValue[0] + Random.Range(-0.1f, 0.1f), colorValue[0] + Random.Range(-0.1f, 0.1f)),
                new Color(colorValue[1] + Random.Range(-0.1f, 0.1f), colorValue[1] + Random.Range(-0.1f, 0.1f), colorValue[1] + Random.Range(-0.1f, 0.1f)),
                new Color(colorValue[2] + Random.Range(-0.1f, 0.1f), colorValue[2] + Random.Range(-0.1f, 0.1f), colorValue[2] + Random.Range(-0.1f, 0.1f)) };

        headResolver.SetCategoryAndLabel("Head", "Head" + +Random.Range(0, 5));

        switch (tier)
        {
            case "왕족":

                jacketResolver.SetCategoryAndLabel("Jacket", "KingJacket");
                jacketRenderer.color = color;

                hatResolver.SetCategoryAndLabel("Hat", "Crown");
                hatRenderer.color = color;

                break;

            case "귀족":

                foreach (SpriteRenderer i in bodyRenderers)
                    i.color = colors[0];

                jacketWhite.SetActive(true);

                jacketResolver.SetCategoryAndLabel("Jacket", "Jacket");
                jacketRenderer.color = colors[1];

                hatResolver.SetCategoryAndLabel("Hat", "HatG" + Random.Range(0, 3));
                hatRenderer.color = new Color(0.2f, 0.2f, 0.2f);

                break;

            case "주민":

                foreach (SpriteRenderer i in bodyRenderers)
                    i.color = colors[0];

                hatResolver.SetCategoryAndLabel("Hat", "Hat" + Random.Range(0, 3));
                hatRenderer.color = colors[1];

                jacketResolver.SetCategoryAndLabel("Jacket", "None");

                break;
        }
    }

    IEnumerator CoSetHitBox()
    {
        hitboxes[0].gameObject.SetActive(false);
        hitboxes[1].gameObject.SetActive(false);

        yield return new WaitForSeconds(1);

        hitboxes[0].gameObject.SetActive((int)hitPointType == 0);
        hitboxes[1].gameObject.SetActive((int)hitPointType == 1);

        hitboxes[(int)hitPointType].GetComponent<SpriteRenderer>().color = Color.clear;
        hitboxes[(int)hitPointType].GetComponent<SpriteRenderer>().DOColor(Color.white, 1);

        hitboxes[(int)hitPointType].localScale = new Vector3(patientData.size, 0.8f, 1);

        //hitboxes[(int)hitPointType].position = new Vector3(hitboxes[(int)hitPointType].position.x + Random.Range(-0.2f, 0.2f), 0.5f, 0);

        hitboxes[(int)hitPointType].DOMoveX(hitboxes[(int)hitPointType].position.x + Random.Range(-0.2f, 0.2f), 1f);

        print(hitboxes[(int)hitPointType].position);
    }

    #endregion

    #region Status

    public void Hit(HitType hitType)
    {
        if (isFinish) return;

        headAudioSource.clip = screamClips[Random.Range(0, screamClips.Length)];
        AudioManager.instance.PlayAudioSource(GetComponent<AudioSource>());

        screamText.Scream();

        animator.SetInteger("pattern", Random.Range(0,4));


        if (hitType == HitType.Perfect)
        {
            print("정확한 절단!");
            DecreaseHp();
        }
        else if (hitType == HitType.Hit)
        {
            print("적절한 절단!");

            AudioManager.instance.PlayAudioSource(headAudioSource);

            DecreaseHp();
            DecreaseLife();

            Camera.main.DOShakePosition(0.1f, 0.1f);
        }
        else if (hitType == HitType.Miss)
        {
            print("실수!");
            DecreaseLife();

            AudioManager.instance.PlayAudioSource(headAudioSource);

            animator.SetInteger("hitType", (int)HitType.Miss);
            animator.SetTrigger("hit");

            Camera.main.DOShakePosition(0.3f, 0.2f);
        }

        if (!isBleed) Bleed();

        CheckState();
    }


    void DecreaseHp()
    {
        float chop = Surgeon.instance.curWeapon.chop;
        float cruch = Surgeon.instance.curWeapon.crush;

        if (skinHp <= 0)
        {
            boneHp = boneHp - cruch < 0 ? 0 : boneHp - cruch;
        }
        else
        {
            skinHp = skinHp - chop < 0 ? 0 : skinHp - chop;
        }
    }

    void DecreaseLife(float damage = 1)
    {
        print(damage);
        life = life - damage < 0 ? 0 : life - damage;

        CheckState();
    }

    public void CheckState()
    {
        if (isFinish) return;

        if(life <= 0)
        {
            Dead();

            AmputationManager.instance.FinishAmputation(false);
        }

        if(skinHp <= 0 && boneHp <= 0)
        {
            Success();

            AmputationManager.instance.FinishAmputation(true);
        }
    }

    void Dead()
    {
        animator.SetBool("isDead", true);
        animator.SetTrigger("dead");
        isFinish = true;
    }

    void Success()
    {
        animator.SetBool("isDead", true);
        animator.SetTrigger("success");
        isFinish = true;
    }

    void Bleed()
    {
        isBleed = true;

        hitboxes[(int)hitPointType].GetComponent<ParticleSystem>().Play();

        StartCoroutine(CoBleed());
    }

    IEnumerator CoBleed()
    {
        if (isFinish) yield break;

        DecreaseLife(patientData.blood);

        yield return new WaitForSeconds(1);

        StartCoroutine(CoBleed());
    }

    #endregion

}
