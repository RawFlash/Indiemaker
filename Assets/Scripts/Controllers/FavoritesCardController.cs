using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FavoritesCardController : MonoBehaviour
{
    public TMP_Text title;

    public Button remove, select;

    public User user;

    public void Init(User user)
    {
        this.user = user;
        title.text = user.cardName;

        remove.onClick.RemoveAllListeners();
        remove.onClick.AddListener(Remove);

        select.onClick.AddListener(() => MainWindowController.instance.favoritesController.ShowCard(user));
    }

    public void Remove()
    {
        MainWindowController.instance.favoritesController.Remove(user);
    }
}
