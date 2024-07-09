using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Logout_API : MonoBehaviour
{
    IEnumerator LogOut()
    {
        string _url = "https://echo.backend.cynotics.in/api/student/logout";

        APIClasses.UserData logout = new APIClasses.UserData
        {
            email = Get_Student_Details.email,
        };
        string jsonBody = JsonUtility.ToJson(logout);
        byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

        Debug.Log(jsonBody);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(_url, "application/json");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Logout Success");
            ClearPlayerPrefs();
            SceneManager.LoadScene("Student Login");
        }

        else
        {
            Debug.Log("Logout UnSyuccess");
        }
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(Login_API.EmailKey);
        PlayerPrefs.DeleteKey(Login_API.PasswordKey);
        PlayerPrefs.DeleteKey(Login_API.DeviceKeyKey);
        PlayerPrefs.Save();
    }

    public void LogoutAttempt()
    {
        StartCoroutine(LogOut());
    }
}
