using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PaymentGateway : MonoBehaviour
{
    private string paymentUrl;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Onclick()
    {
        StartCoroutine(PaymentGatewayx());
    }

    IEnumerator PaymentGatewayx()
    {
        string _url = "https://echo.backend.cynotics.in/api/upgradeStatus/" + Get_Student_Details.guid + "/" + Get_Student_Details.phone;

        Debug.Log(_url);

        string jsonBody = JsonUtility.ToJson("");
        byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

        Debug.Log(jsonBody);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(_url, "application/json");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        /*if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");
            string response = request.downloadHandler.text;
            Debug.Log(response);

            //System.IO.File.WriteAllText("Assets/debug.log", response);
        }
        else
        {
            Debug.Log("UnSuccess");
        }*/

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");
            string responseText = request.downloadHandler.text;
            Debug.Log(responseText);

            // Extract the URL from the response text
            paymentUrl = ExtractUrlFromResponse(responseText);

            // Open the URL in the user's browser
            if (!string.IsNullOrEmpty(paymentUrl))
            {
                Application.OpenURL(paymentUrl);
                Application.Quit();
            }
            else
            {
                Debug.LogWarning("Failed to extract a valid URL from the response.");
            }
        }
        else
        {
            Debug.Log("UnSuccess");
        }
    }

    private string ExtractUrlFromResponse(string responseText)
    {
        int startIndex = responseText.IndexOf("https://");
        if (startIndex != -1)
        {
            int endIndex = responseText.IndexOf("\"", startIndex);
            if (endIndex != -1)
            {
                return responseText.Substring(startIndex, endIndex - startIndex);
            }
        }
        return string.Empty;
    }
}
