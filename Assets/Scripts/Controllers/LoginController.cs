using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public static LoginController instance;

    public TMP_InputField loginText, passText, vktext, tgtext, loginSignUpText, passSignUpText;
    public TMP_Text warning, or;


    public TMP_Dropdown cardType, manType;

    public Button login, showSignUp, hideSignUp, signup;

    public GameObject signUpPanel;

    private void Start()
    {
        instance = this;
        login.onClick.AddListener(CheckLogin);
        showSignUp.onClick.AddListener(() => signUpPanel.SetActive(true));
        hideSignUp.onClick.AddListener(() => signUpPanel.SetActive(false));
        signup.onClick.AddListener(CheckSignup);


        manType.options = ManTypeOptions.data;
    }

    private void CheckLogin()
    {
        ServerInteraction.LogIn(loginText.text, passText.text);
    }

    public void ShowWarning(string message)
    {
        warning.gameObject.SetActive(true);
        warning.text = message;
    }

    public void UpdateManType()
    {
        manType.gameObject.SetActive(cardType.value == 0);
    }

    private void CheckSignup()
    {
        if (loginSignUpText.text == "")
        {
            warning.gameObject.SetActive(true);
            warning.text = "¬ведите логин";
            return;
        }
        if (passSignUpText.text == "")
        {
            warning.gameObject.SetActive(true);
            warning.text = "¬ведите пароль";
            return;
        }

        if (vktext.text == "" && tgtext.text == "")
        {
            warning.gameObject.SetActive(true);
            warning.text = "¬ведите хот€ бы одну ссылку на соц. сети";
            return;
        }

        string tempCardType;

        if (cardType.value == 0)
        {
            tempCardType = CardType.SearchProject;
        }
        else
        {
            tempCardType = CardType.SearchMan;
        }

        User user = new(loginSignUpText.text, passSignUpText.text, vktext.text, tgtext.text, tempCardType, manType.value.ToString());

        ServerInteraction.SignUp(user);
    }

    public void GetUserInfo()
    {
        ServerInteraction.GetUserInfo();
    }

    public void OpenMainWindow()
    {
        SceneManager.LoadScene("Main");
    }
}
