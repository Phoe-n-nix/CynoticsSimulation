using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ForgotPassword : MonoBehaviour
{

    [SerializeField] private GameObject _forgotPassScreen, _otpVerificationScreen, _mainSignInScreen, _resetPassScreen;
    [SerializeField] private TMP_InputField _email, _password, confirmPass;

    [SerializeField] private TMP_InputField firstDigit, secondDigit, thirdDigit, fourthDigit;

    [SerializeField] private TextMeshProUGUI title, message, emailShown;
    [SerializeField] private GameObject popup;
    public GameObject InpHandler;

    IEnumerator SendEmail()
    {
        Debug.Log("Email Sending Started");
        string _url = "https://echo.backend.cynotics.in/api/student/forget-password";

        APIClasses.ForgotPassword forgotEmailHolder = new APIClasses.ForgotPassword()
        {
            email = _email.text,
        };

        string josnBody = JsonUtility.ToJson(forgotEmailHolder);
        byte[] rawBody = Encoding.UTF8.GetBytes(josnBody);

        Debug.Log(josnBody);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(_url, "application/json");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");
            Debug.Log(request.downloadHandler.data);
            _forgotPassScreen.SetActive(false);
            _otpVerificationScreen.SetActive(true);
            emailShown.text = _email.text;
            InpHandler.SetActive(true);
        }
        else
        {
            Debug.Log("error"); //Popup for error
            Debug.Log(request.error);
            popup.SetActive(true);
            title.text = "";
            message.text = "User do not exist";
        }
    }

    IEnumerator SendUserEnteredOtp()
    {
        string _url = "https://echo.backend.cynotics.in/api/student/reset-password";

        APIClasses.ResetPassword otpHolder = new APIClasses.ResetPassword()
        {
            email = _email.text,
            emailOTP = ("" + firstDigit.text + secondDigit.text + thirdDigit.text + fourthDigit.text),
            password = _password.text,
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
            //StartCoroutine(VerifyOTP());
            /*_mainSignInScreen.SetActive(true);
            _resetPassScreen.SetActive(false);*/
            SceneManager.LoadScene("Student Login");
            ClearPlayerPrefs();
        }
        else
        {
            Debug.Log("Error to send OTP");
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
            Debug.Log(newRequest.downloadHandler.text);
            _mainSignInScreen.SetActive(true);
            _resetPassScreen.SetActive(false);
        }
        else
        {
            Debug.Log("Wrong OTP Entered");
            Debug.Log(newRequest.error);
        }
    }

    public void ForgetPassAttempting()
    {
        StartCoroutine(SendEmail());
    }

    public void OTPVerification()
    {
        if(_password.text != confirmPass.text)
        {
            popup.SetActive(true);
            title.text = "Password mismatch";
            message.text = "Enter the same password in both password fields";
            return;
        }

        StartCoroutine(SendUserEnteredOtp());
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(Login_API.EmailKey);
        PlayerPrefs.DeleteKey(Login_API.PasswordKey);
        PlayerPrefs.DeleteKey(Login_API.DeviceKeyKey);
        PlayerPrefs.Save();
    }
}
