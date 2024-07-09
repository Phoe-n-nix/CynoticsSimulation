using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login_API : MonoBehaviour
{
    [SerializeField] private TMP_InputField email, password;
    [SerializeField] private TextMeshProUGUI title, message;
    [SerializeField] private GameObject popup;

    public const string EmailKey = "Email";
    public const string PasswordKey = "Password";
    public const string DeviceKeyKey = "DeviceKey";

    private void Start()
    {
        // Check if email, password, and device key are stored in PlayerPrefs
        if (PlayerPrefs.HasKey(EmailKey) && PlayerPrefs.HasKey(PasswordKey) && PlayerPrefs.HasKey(DeviceKeyKey))
        {
            StartCoroutine(AutoLogin());
        }
    }

    IEnumerator Login()
    {
        string _url = "https://echo.backend.cynotics.in/api/student/login";

        APIClasses.LoginDataHolder loginData = new APIClasses.LoginDataHolder()
        {
            email = email.text,
            password = password.text,
            deviceKey = Andriod_ID.deviceId
        };

        string jsonBody = JsonUtility.ToJson(loginData);
        byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

        Debug.Log(jsonBody);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(_url, "application/json");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data Sent");
            StartCoroutine(GetLoginCreds());

            // Store the email, password, and device key in PlayerPrefs for auto-login
            PlayerPrefs.SetString(EmailKey, email.text);
            PlayerPrefs.SetString(PasswordKey, password.text);
            PlayerPrefs.SetString(DeviceKeyKey, Andriod_ID.deviceId);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Error to send data");
            Debug.Log(request.error);
            popup.SetActive(true);
            title.text = "Error";
            message.text = "Try again please";
        }
    }

    IEnumerator GetLoginCreds()
    {
        UnityWebRequest newRequest = UnityWebRequest.Get("https://echo.backend.cynotics.in/api/student/");
        yield return newRequest.SendWebRequest();

        if (newRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data Retrieved");
            SceneManager.LoadScene("Main Alpha Functionality Pages");
            Debug.Log(newRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error to retrieve data");
            Debug.Log(newRequest.error);
        }
    }

    public IEnumerator AutoLogin()
    {
        yield return new WaitForSeconds(1);
        // Auto-login with the stored email, password, and device key
        string storedEmail = PlayerPrefs.GetString(EmailKey);
        string storedPassword = PlayerPrefs.GetString(PasswordKey);
        string storedDeviceKey = PlayerPrefs.GetString(DeviceKeyKey);
        email.text = storedEmail;
        password.text = storedPassword;
        Andriod_ID.deviceId = storedDeviceKey; // Assuming Andriod_ID.deviceId is a static property
        AttemptLogin();
    }

    public void AttemptLogin()
    {
        StartCoroutine(Login());
    }
}
