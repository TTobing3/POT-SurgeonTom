using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    Button button;
    Image image;

    public Sprite[] sprites;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        button.onClick.AddListener(Mute);
    }

    private void Start()
    {
        if (DataCarrier.instance.isMute) image.sprite = sprites[1];
    }

    void Mute()
    {
        AudioManager.instance.Mute();

        if (DataCarrier.instance.isMute) image.sprite = sprites[1];
        else image.sprite = sprites[0];
    }
}
