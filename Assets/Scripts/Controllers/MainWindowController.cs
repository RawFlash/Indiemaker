using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindowController : MonoBehaviour
{
    public static MainWindowController instance;

    public FindController findController;

    public Accountcontroller accountController;

    public FavoritesController favoritesController;

    public Window currentWindow;

    public static User CurrentUser 
    {
        get
        {
            _currentUser ??= new User();
            return _currentUser;
        }
        set
        {
            _currentUser = value;
        }
    }

    private static User _currentUser;

    public Button accountOpen, findOpen, favoritesOpen;

    private void Start()
    {
        instance = this;

        accountOpen.onClick.AddListener(() => OpenWindow(Window.Account));
        findOpen.onClick.AddListener(() => OpenWindow(Window.Find));
        favoritesOpen.onClick.AddListener(() => OpenWindow(Window.Favorites));

        findController.DownloadCards();
    }

    private void OpenWindow(Window newWindow)
    {
        if (newWindow == currentWindow)
        {
            return;
        }

        findController.gameObject.SetActive(false);
        accountController.gameObject.SetActive(false);
        favoritesController.gameObject.SetActive(false);

        switch (newWindow)
        {
            case Window.Account:
                accountController.gameObject.SetActive(true);
                break;

            case Window.Find:
                findController.DownloadCards();
                findController.gameObject.SetActive(true);
                break;

            case Window.Favorites:
                favoritesController.DownloadFavoritesCards();
                favoritesController.gameObject.SetActive(true);
                break;
        }

        currentWindow = newWindow;
    }
}

public enum Window
{
    Account,
    Find,
    Favorites
}