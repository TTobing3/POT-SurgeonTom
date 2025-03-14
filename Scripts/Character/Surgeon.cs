using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D.Animation;

public class Surgeon : MonoBehaviour
{
    public static Surgeon instance;

    public List<Sprite> motions; // [0] up [1] down [2] stand
    public List<GameObject> hands;

    public WeaponData curWeapon;

    SpriteRenderer spriteRenderer;

    Vector3 defaultScale;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultScale = transform.localScale;
    }

    void Start()
    {
        
    }

    public void AnimateCutDown(bool isCutDown)
    {
        DOTween.Kill(transform);

        transform.localScale = defaultScale;

        foreach (GameObject i in hands) i.SetActive(false);

        if(isCutDown)
        {
            spriteRenderer.sprite = motions[1];
            hands[1].SetActive(true);
            hands[1].GetComponentInChildren<SpriteResolver>().SetCategoryAndLabel("WeaponDown", curWeapon.name);

            transform.DOScaleY(0.85f, 2).SetEase(Ease.Unset);
        }
        else
        {
            spriteRenderer.sprite = motions[0];
            hands[0].SetActive(true);
            hands[0].GetComponentInChildren<SpriteResolver>().SetCategoryAndLabel("WeaponUp", curWeapon.name);
        }
    }


    public void AnimateStand()
    {
        print("stand");
        DOTween.Kill(transform);

        foreach (GameObject i in hands) i.SetActive(false);

        StartCoroutine(CoAnimateStand(true));
    }

    IEnumerator CoAnimateStand(bool isInhale)
    {
        foreach (GameObject i in hands) i.SetActive(false);
        spriteRenderer.sprite = motions[2];

        if (isInhale)
        {
            transform.DOScaleY(defaultScale.y, 1).SetEase(Ease.Unset);
        }
        else
        {
            transform.DOScaleY(defaultScale.y - 0.02f, 1).SetEase(Ease.Unset);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(CoAnimateStand(!isInhale));
    }


    public void ChangeWeapon(WeaponData weaponData)
    {
        curWeapon = weaponData;
    }
}
