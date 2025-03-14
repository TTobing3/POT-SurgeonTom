using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ScreamText : MonoBehaviour
{
    public GameObject screamTextPrefab;
    private readonly List<GameObject> screamTextPool = new();

    string[] screamTexts = new string[] {
    "��", "Ū", "����", "�ƾ�", "����", "����", "����", "���̱�", "Ū��", "�Ǿ�",
    "ũ��", "��!", "��!", "��", "��", "����!", "���!", "ũ��", "ũ��!", "��!"};

    // Scream �Լ�: Ǯ���� ��� ������ ������Ʈ�� ��ȯ�ϰų� ���� ����
    public void Scream()
    {
        var screamObject = screamTextPool.FirstOrDefault(obj => !obj.activeSelf);

        if (screamObject == null)
        {
            screamObject = Instantiate(screamTextPrefab);
            screamObject.name = "ScreamTextPrefab";
            screamObject.transform.SetParent(transform);
            screamTextPool.Add(screamObject);
        }

        screamObject.SetActive(true);

        //
        var size = Random.Range(0.5f, 1.5f);

        screamObject.transform.position = Vector3.zero;
        screamObject.transform.rotation = Quaternion.identity;
        screamObject.transform.localScale = new Vector3(size, size, size);
        screamObject.GetComponent<TextMeshPro>().color = Color.white; ;

        //

        var time = Random.Range(0.2f, 0.5f);

        //screamObject.transform.DOScale(0, time).SetEase(Ease.OutCubic);
        screamObject.transform.DOMoveX(transform.position.x - Random.Range(0.2f, 1f), time).SetEase(Ease.OutCubic);
        screamObject.transform.DOMoveX(transform.position.y - Random.Range(1, 2f), time).SetEase(Ease.OutCubic);

        screamObject.transform.DORotate(new Vector3(0, 0, Random.Range(45, 180f)), time).SetEase(Ease.OutCubic).OnComplete(()=>screamObject.SetActive(false));

        screamObject.GetComponent<TextMeshPro>().text = screamTexts[Random.Range(0, screamTexts.Length)];
        screamObject.GetComponent<TextMeshPro>().DOFade(0, time);

    }

}
