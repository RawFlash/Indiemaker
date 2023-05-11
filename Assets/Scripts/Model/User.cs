using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

[Serializable]
public class User
{
    public string id;

    public string login;
    public string password;
    public string vk;
    public string tg;


    /// <summary>
    /// "Search project", "Search man"
    /// </summary>
    public string cardType;

    public string manType;

    public string cardName;
    public string cardDescription;

    public string favoritesID;


    public User(string login, string pass, string vk, string tg, string cardType, string mantype)
    {
        this.login = login;
        password = pass;
        this.vk = vk;
        this.tg = tg;
        this.cardType = cardType;
        this.manType = mantype;
    }

    public User() { }

    public override string ToString()
    {
        return id +" " + login + " " + password +" " + vk + " " + tg + " " + cardType +" " + cardName +" " + cardDescription;
    }

    public void AddFavorites(string idCard)
    {
        if (!string.IsNullOrEmpty(favoritesID))
        {
            favoritesID += ", ";
        }
        favoritesID += idCard;
    }

    public void RemoveFavorites(string idCard)
    {
        List<string> tempList = GetFavorites();

        tempList.Remove(idCard);

        favoritesID = "";

        for(int i = 0; i < tempList.Count; i++)
        {
            favoritesID += tempList[i];

            if (i < tempList.Count - 1)
            {
                favoritesID += ", ";
            }
        }
    }

    public bool IsFavoritesID(string idCard)
    {
        if (GetFavorites() == null)
        {
            return false;
        }
        return GetFavorites().Contains(idCard);
    }

    public List<string> GetFavorites()
    {
        if (string.IsNullOrEmpty(favoritesID))
        {
            return null;
        }

        List<string> listFavoritesID = new(favoritesID.Split(", "));
        return listFavoritesID;
    }
}

public static class ManTypeOptions
{
    public static List<TMP_Dropdown.OptionData> data = new()
    {
        new TMP_Dropdown.OptionData("Продюсер"),
        new TMP_Dropdown.OptionData("Менеджер проекта"),
        new TMP_Dropdown.OptionData("Unity разработчик"),
        new TMP_Dropdown.OptionData("UE разработчик"),
        new TMP_Dropdown.OptionData("Разработчик(прочее)"),
        new TMP_Dropdown.OptionData("3D художник"),
        new TMP_Dropdown.OptionData("2D художник"),
        new TMP_Dropdown.OptionData("VFX художник"),
        new TMP_Dropdown.OptionData("Маркетолог"),
        new TMP_Dropdown.OptionData("Комьюнити менеджер"),
        new TMP_Dropdown.OptionData("Саунд-дизайнер"),
        new TMP_Dropdown.OptionData("Сценарист"),
        new TMP_Dropdown.OptionData("Геймдизайнер"),
        new TMP_Dropdown.OptionData("Аналитик"),
        new TMP_Dropdown.OptionData("QA"),
        new TMP_Dropdown.OptionData("HR"),
    };
}

public class CardsList
{
    public List<User> cards;
    public string message;
}

public static class CardType
{
    public static string SearchMan = "Search man";
    public static string SearchProject = "Search project";
}
