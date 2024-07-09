using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class OTP_Verification : MonoBehaviour
{
    [SerializeField] private TMP_InputField firstDigit, secondDigit, thirdDigit, fourthDigit, email;

    [SerializeField] private TextMeshProUGUI title, message;
    [SerializeField] private GameObject popup;

    [SerializeField] private TMP_InputField otpBox;

    public SignUpFB SignUpFB;

    IEnumerator SendUserEnteredOtp()
    {
        string _url = "https://echo.backend.cynotics.in/api/student/verify-otp";

        APIClasses.OtpSend otpHolder = new APIClasses.OtpSend()
        {
            email = email.text,
            emailOTP = otpBox.text /*("" + firstDigit.text + secondDigit.text + thirdDigit.text + fourthDigit.text)*/
        };

        string jsonBody = JsonUtility.ToJson(otpHolder);
        byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

        Debug.Log(jsonBody);
        
        UnityWebRequest request = UnityWebRequest.PostWwwForm(_url, "application/json");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success In Sending The OTP");
            StartCoroutine(VerifyOTP());
            SignUpFB.SignUpLogFB();
        }
        else
        {
            Debug.Log("Error to send OTP");
            popup.SetActive(true);
            message.text = "Error occured please enter otp again";
            Debug.Log(request.error);
        }
        
    }

    IEnumerator VerifyOTP()
    {
        UnityWebRequest newRequest = UnityWebRequest.Get("https://echo.backend.cynotics.in/api/student/verify-otp");
        yield return newRequest.SendWebRequest();

        if (newRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Verified");
            SceneManager.LoadScene("Student Login");
            Debug.Log(newRequest.result);
        }
        else
        {
            Debug.Log("Wrong OTP Entered");
            Debug.Log(newRequest.error);
        }
    }

    public void AttemptVerification()
    {
        /*if(firstDigit.text == string.Empty || secondDigit.text == string.Empty || thirdDigit.text == string.Empty || fourthDigit.text == string.Empty)
        {
            popup.SetActive(true);
            title.text = "All fields required";
            message.text = "Enter all fields correctly to proceed";
            return;
        }*/

        if (otpBox.text == string.Empty)
        {
            popup.SetActive(true);
            title.text = "All fields required";
            message.text = "Enter all fields correctly to proceed";
            return;
        }

        StartCoroutine(SendUserEnteredOtp());
        Debug.Log(otpBox);
    }
}
