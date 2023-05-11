using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class ServerInteraction
{
    const string URLMain = "http://indiemaker.somee.com/api/user/";

    const string URLSignUp = "signup";

    const string URLLogIn = "login";

    const string URLGetInfoUser = "getinfouser";

    const string URLUpdateInfoUser = "updateuser";

    const string URLGetCardsInfo = "getcardsinfo";

    const string URLGetCadrsByIDsInfo = "getcardsbyidsinfo";

    private static UnityAction<UnityWebRequest> responce;


    public static void LogIn(string username, string password)
    {
        MainWindowController.CurrentUser = new()
        {
            login = username,
            password = password
        };
        string json = JsonUtility.ToJson(MainWindowController.CurrentUser);

        responce += LogInResponce;

        LoginController.instance.StartCoroutine(SendRequest(URLMain + URLLogIn, json));
    }

    public static void SignUp(User user)
    {
        MainWindowController.CurrentUser = user;

        string json = JsonUtility.ToJson(user);

        responce += SignUpResponce;

        LoginController.instance.StartCoroutine(SendRequest( URLMain + URLSignUp, json));
    }

    public static void GetUserInfo()
    {
        string json = JsonUtility.ToJson(MainWindowController.CurrentUser);


        responce += GetInfoUserResponce;

        LoginController.instance.StartCoroutine(SendRequest(URLMain + URLGetInfoUser, json));
    }

    public static void UpdateUserInfo(User user)
    {
        string json = JsonUtility.ToJson(MainWindowController.CurrentUser);
        responce += UpdateInfoUserResponce;

        MainWindowController.instance.StartCoroutine(SendRequest(URLMain + URLUpdateInfoUser, json));
    }

    public static void GetCardsInfo(string findTypeCards)
    {
        string json = "{ \"findtype\":\"" + findTypeCards + "\"}";
        responce += GetCardsInfoResponce;

        MainWindowController.instance.StartCoroutine(SendRequest(URLMain + URLGetCardsInfo, json));
    }
    public static void GetCardsByIDsInfo(List<string> ids)
    {
        string json = "{ \"ids\":" + Newtonsoft.Json.JsonConvert.SerializeObject(ids) + "}";
        responce += GetCardsByIDsInfoResponce;

        MainWindowController.instance.StartCoroutine(SendRequest(URLMain + URLGetCadrsByIDsInfo, json));
    }

    static IEnumerator SendRequest(string url, string json)
    {
        using UnityWebRequest www = new(url, "POST");

        Debug.Log("Send request: URL - " + url +"\n" + "Data - " + json);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();

        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            responce?.Invoke(www);
        }
    }
    
    private static void SignUpResponce(UnityWebRequest request)
    {
        responce -= SignUpResponce;
        if (request.downloadHandler.text.Contains("Complete"))
        {
            string id = request.downloadHandler.text.Split(": ")[1];
            Debug.Log(request.downloadHandler.text);
            Debug.Log("ID:"+id);

            MainWindowController.CurrentUser.id = id;

            LoginController.instance.GetUserInfo();
        }
        else if(request.downloadHandler.text == "Incorrect login")
        {
            LoginController.instance.ShowWarning("Такой логин уже занят");
            Debug.LogError(request.downloadHandler.text);
        }
        else
        {
            LoginController.instance.ShowWarning("Неизвестная ошибка");
            Debug.LogError(request.downloadHandler.text);
        }
    }

    private static void LogInResponce(UnityWebRequest request)
    {
        responce -= LogInResponce;

        if (request.downloadHandler.text.Contains("Complete"))
        {
            string id = request.downloadHandler.text.Split(": ")[1];
            Debug.Log(request.downloadHandler.text);
            Debug.Log("ID:" + id);

            MainWindowController.CurrentUser.id = id;

            LoginController.instance.GetUserInfo();
        }
        else if(request.downloadHandler.text == "Incorrect login/pass")
        {
            LoginController.instance.ShowWarning("Неверный логин или пароль");
            Debug.LogError(request.downloadHandler.text);
        }
        else
        {
            LoginController.instance.ShowWarning("Неизвестная ошибка");
            Debug.LogError(request.downloadHandler.text);
        }
    }

    private static void GetInfoUserResponce(UnityWebRequest request)
    {
        responce -= GetInfoUserResponce;

        try
        {
            MainWindowController.CurrentUser = JsonUtility.FromJson<User>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
            Debug.Log("Update: " + MainWindowController.CurrentUser.ToString());


            LoginController.instance.OpenMainWindow();
        }
        catch
        {
            Debug.LogError(request.downloadHandler.text);
        }
    }

    private static void UpdateInfoUserResponce(UnityWebRequest request)
    {
        responce -= UpdateInfoUserResponce;

        if (request.downloadHandler.text.Contains("Complete"))
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    private static void GetCardsInfoResponce(UnityWebRequest request)
    {
        responce -= GetCardsInfoResponce;

        Debug.Log(request.downloadHandler.text);

        MainWindowController.instance.findController.allCards = JsonUtility.FromJson<CardsList>(request.downloadHandler.text).cards;

        MainWindowController.instance.findController.InitCards();
    }

    private static void GetCardsByIDsInfoResponce(UnityWebRequest request)
    {
        responce -= GetCardsByIDsInfoResponce;

        Debug.Log(request.downloadHandler.text);

        MainWindowController.instance.favoritesController.favoritesUsers = JsonUtility.FromJson<CardsList>(request.downloadHandler.text).cards;

        MainWindowController.instance.favoritesController.InitCards();
    }


    /*
    public static RealmController Instance;

    public string RealmAppId = "YOUR_REALM_APP_ID_HERE";

    private Realm _realm;
    private App _realmApp;
    private User _realmUser;
    HttpClientHandler http;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        http = new HttpClientHandler();

        _realmApp = App.Create(new AppConfiguration(RealmAppId)
        {
            HttpClientHandler = http
        });
        _realmApp.Sync.Reconnect();

        
    }

    void OnDisable()
    {
        _realm?.Dispose();
    }

    public async Task<string> Login(string login) 
    {
        if (login != "")
        {


            try
            {
                if (_realmUser == null)
                {
                    _realmUser = await _realmApp.LogInAsync(Credentials.Anonymous());
                    _realm = await Realm.GetInstanceAsync(new PartitionSyncConfiguration("vip.pahan999@gmail.com", _realmUser));
                }
                else
                {
                    
                }
            }
            catch (ClientResetException clientResetEx)
            {
                
                _realm?.Dispose();
                clientResetEx.InitiateClientReset();
            }

            Debug.Log(_realm.ToJson());
            Debug.Log(_realm.All<UserModel>());
            return "1";
        }
        return "";
    }

    public UserModel GetPlayerProfile()
    {
        UserModel _playerProfile = _realm.Find<UserModel>(_realmUser.Id);
        if (_playerProfile == null)
        {
            _realm.Write(() => {
                _playerProfile = _realm.Add(new UserModel(_realmUser.Id));
            });
        }
        return _playerProfile;
    }
    */






}
