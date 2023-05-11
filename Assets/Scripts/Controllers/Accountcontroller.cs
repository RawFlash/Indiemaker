using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Accountcontroller : MonoBehaviour
{
    public TMP_InputField VK, TG, CardName, CardDescription;
    public Button save;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        VK.text = MainWindowController.CurrentUser.vk;
        TG.text = MainWindowController.CurrentUser.tg;
        CardName.text = MainWindowController.CurrentUser.cardName;
        CardDescription.text = MainWindowController.CurrentUser.cardDescription;


        save.onClick.RemoveAllListeners();
        save.onClick.AddListener(Save);
    }

    private void Save()
    {
        MainWindowController.CurrentUser.vk = VK.text;
        MainWindowController.CurrentUser.tg = TG.text;
        MainWindowController.CurrentUser.cardName = CardName.text;
        MainWindowController.CurrentUser.cardDescription = CardDescription.text;


        ServerInteraction.UpdateUserInfo(MainWindowController.CurrentUser);
    }
}
