using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public TMP_Text title, description;
    public Button vk, tg;

    private User user;

    private bool isCanSlide, isShowCloseButton;

    public void Setup(User user)
    {
        this.user = user;
        title.text = user.cardName;
        description.text = user.cardDescription;

        vk.onClick.RemoveAllListeners();
        vk.onClick.AddListener(()=> Application.OpenURL(user.vk));

        tg.onClick.RemoveAllListeners();
        tg.onClick.AddListener(() =>  Application.OpenURL(user.tg));
    }

    public void SetIsCanSlide(bool newStatus)
    {

    }

    public void SetIsShowCloseButton(bool newStatus)
    {

    }

    public void Slide(bool isRight)
    {
        StartCoroutine(SlideCoroutine(isRight));
    }

    private IEnumerator SlideCoroutine(bool isRight)
    {
        float Xpos = -1500;
        if (isRight)
        {
            Xpos = -Xpos;
        }

        float step = 20;

        while ((transform.localPosition.x <= Xpos && isRight) || (transform.localPosition.x >= Xpos && !isRight))
        {
            if (isRight)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + step, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x - step, transform.localPosition.y, transform.localPosition.z);
            }

            yield return new WaitForSeconds(0.01f);
        }

        MainWindowController.instance.findController.DestroyCard(user, isRight);


        Destroy(gameObject);
    }
}
