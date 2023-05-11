using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavoritesController : MonoBehaviour
{
    public List<User> favoritesUsers;

    public GameObject cardParent, favoritesCardPrefab;

    public GameObject notFind;

    public CardController showCardController;

    public Button closeCardController;

    private void Start()
    {
        closeCardController.onClick.AddListener(() => showCardController.gameObject.transform.parent.gameObject.SetActive(false));
    }

    public void DownloadFavoritesCards()
    {
        if (MainWindowController.CurrentUser.GetFavorites() != null && MainWindowController.CurrentUser.GetFavorites().Count != 0)
        {
            ServerInteraction.GetCardsByIDsInfo(MainWindowController.CurrentUser.GetFavorites());
        }
        else
        {
            InitCards();
        }
    }

    public void InitCards()
    {
        for(int i = 0; i < cardParent.transform.childCount;i++)
        {
            Destroy(cardParent.transform.GetChild(0).gameObject);
        }

        if (favoritesUsers != null)
        {
            foreach (var item in favoritesUsers)
            {
                FavoritesCardController cardController = Instantiate(favoritesCardPrefab, cardParent.transform).GetComponent<FavoritesCardController>();
                cardController.Init(item);
            }
        }

        notFind.SetActive(favoritesUsers == null || favoritesUsers.Count == 0);
    }

    public void ShowCard(User user)
    {
        showCardController.gameObject.transform.parent.gameObject.SetActive(true);
        showCardController.Setup(user);
    }

    public void Remove(User user)
    {
        favoritesUsers.Remove(user);
        InitCards();
        MainWindowController.CurrentUser.RemoveFavorites(user.id);
        ServerInteraction.UpdateUserInfo(MainWindowController.CurrentUser);
    }
}
