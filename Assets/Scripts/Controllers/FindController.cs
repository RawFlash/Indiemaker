using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FindController : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public TMP_Dropdown filter;

    public List<User> allCards;

    public List<User> notWatchCard;

    public List<User> dislike;

    public GameObject cardPrefab;

    public CardController cardController;

    public User currentUser;

    public Image notFind;

    private bool isClicking;

    private void Start()
    {
        if (MainWindowController.CurrentUser.cardType == CardType.SearchProject)
        {
            filter.gameObject.SetActive(false);
        }

        filter.options = ManTypeOptions.data;
    }


    public void DownloadCards()
    {
        notFind.gameObject.SetActive(false);

        string tempCardType;
        if (MainWindowController.CurrentUser.cardType == CardType.SearchMan)
        {
            tempCardType = CardType.SearchProject;
        }
        else
        {
            tempCardType = CardType.SearchMan;
        }
        ServerInteraction.GetCardsInfo(tempCardType);
    }

    public void InitCards()
    {
        List<User> tempList = allCards;

        notWatchCard = new List<User>();

        //remove favorites from not watch card
        //remove cards with unll or empty title or description
        foreach (User card in tempList)
        {
            if (!MainWindowController.CurrentUser.IsFavoritesID(card.id) 
                && !string.IsNullOrEmpty(card.cardName) 
                && !string.IsNullOrEmpty(card.cardDescription))
            {
                notWatchCard.Add(card);
            }
        }

        SetupCard();
    }

    public void SetupCard()
    {
        if (cardController)
        {
            Destroy(cardController.gameObject);
        }

        if(notWatchCard.Count > 0)
        {
            if (MainWindowController.CurrentUser.cardType == CardType.SearchProject)
            {
                currentUser = GetRandomProject(notWatchCard);
            }
            else
            {
                currentUser = GetRandomMan(filter.value.ToString(), notWatchCard);;
            }


            if (currentUser == null)
            {
                notFind.gameObject.SetActive(true);
                return;
            }

            cardController = Instantiate(cardPrefab,transform).GetComponent<CardController>();
            cardController.Setup(currentUser);
            notFind.gameObject.SetActive(false);
        }
        else
        {
            notFind.gameObject.SetActive(true);
        }
    }

    public void DestroyCard(User user, bool isLike)
    {
        notWatchCard.Remove(user);

        if (isLike)
        {
            LikeCard(user);
        }
        else
        {
            dislike.Add(user);
        }

        SetupCard();
    }

    public void LikeCard(User card)
    {
        MainWindowController.CurrentUser.AddFavorites(card.id);

        ServerInteraction.UpdateUserInfo(MainWindowController.CurrentUser);
    }

    public User GetRandomMan(string manType, List<User> users)
    {
        List<User> tempList = new();

        foreach (User user in users)
        {
            if (user.manType == manType)
            {
                tempList.Add(user);
            }
        }

        if (tempList.Count == 0)
        {
            return null;
        }

        return tempList[Random.Range(0, tempList.Count)];
    }

    public User GetRandomProject(List<User> users) => users[Random.Range(0, users.Count)];

    public void OnPointerMove(PointerEventData eventData)
    {
        if (cardController && cardController.gameObject && isClicking)
        {
            float newX = Camera.main.ScreenToWorldPoint(eventData.position).x;
            cardController.gameObject.transform.position =
                new Vector3(newX, cardController.gameObject.transform.position.y, cardController.gameObject.transform.position.z);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicking = false;

        if (cardController)
        {
            if (cardController.gameObject.transform.localPosition.x >= 100 || cardController.gameObject.transform.localPosition.x <= -100)
            {
                bool isRight = cardController.gameObject.transform.localPosition.x >= 100;
                cardController.Slide(isRight);
            }
            else
            {
                cardController.gameObject.transform.localPosition = 
                    new Vector3(0, cardController.gameObject.transform.localPosition.y, cardController.gameObject.transform.localPosition.z);
            }
        }
    }
}
