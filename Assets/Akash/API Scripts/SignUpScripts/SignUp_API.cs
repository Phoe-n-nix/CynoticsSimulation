using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class SignUp_API : MonoBehaviour
{
    //SignUp Fields
    [SerializeField] public TMP_InputField email; 
    [SerializeField] private TMP_InputField password, phone, instituteId, username, dob, confirmPassword;

    //Pannels
    [SerializeField] private GameObject signUpPannel, phoneOtp, emailOtp, popup;

    //Popup
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI message, emailShown;
    public GameObject OTPInpH;

    public IEnumerator Signup()
    {
        Debug.Log("Signup Started");
        string _url = "https://echo.backend.cynotics.in/api/student/signup";

        APIClasses.SignUpDataHolder signUpData = new APIClasses.SignUpDataHolder()
        {
            email = email.text,
            password = password.text,
            phone = phone.text,
            instituteId = instituteId.text,
            username = username.text,
            dob = dob.text,
            //deviceKey = "111008"
            deviceKey = Permanent_AndroidId.UsableID
        };

        string jsonBody = JsonUtility.ToJson(signUpData);
        byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

        Debug.Log(jsonBody);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(_url, "application/json");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");
            Debug.Log(request.downloadHandler.data);
            signUpPannel.SetActive(false);
            emailOtp.SetActive(true);
            StartCoroutine(SendingOTPToEmail());
            OTPInpH.SetActive(true);
        }
        else
        {
            Debug.Log("error"); //Popup for error
            Debug.Log(request.error);
            message.text = request.error;
            popup.SetActive(true);
        }
    }

    IEnumerator SendingOTPToEmail()
    {
        Debug.Log("OTP Sending Started");
        string _url = "https://echo.backend.cynotics.in/api/student/send-otp";

        APIClasses.SignUpDataHolder otpSend = new APIClasses.SignUpDataHolder()
        {
            email = email.text
        };

        string jsonBody = JsonUtility.ToJson(otpSend);
        byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

        Debug.Log(jsonBody);

        UnityWebRequest request = UnityWebRequest.PostWwwForm(_url, "application/json");

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success Sending OTP");
            Debug.Log(request.downloadHandler.data);
        }
        else
        {
            Debug.Log("error for otp"); //Popup for error
            Debug.Log(request.error);
        }
    }

    public void AttemptSignUp()
    {
        if (email.text == string.Empty || password.text == string.Empty || phone.text == string.Empty || instituteId.text == string.Empty || username.text == string.Empty || dob.text == string.Empty)
        {
            Debug.Log("Fill all fields to continue"); //ShowPopup for fill all fields
            signUpPannel.SetActive(true);
            title.text = "";
            message.text = "Input all fields";
            popup.SetActive(true);
            return;
        }

        if (!email.text.Contains("@") || !email.text.Contains("."))
        {
            Debug.Log("Invalid email format"); //Popup for invalid email
            signUpPannel.SetActive(true);
            title.text = "Invalid Email Format";
            message.text = "Enter a valid email format";
            popup.SetActive(true);
            return;
        }

        if (!IsValidDateFormat(dob.text))
        {
            Debug.Log("Invalid date of birth format"); //Popup for invalid date of birth
            signUpPannel.SetActive(true);
            title.text = "Invalid date format";
            message.text = "Enter the date in the format:- DD/MM/YYYY";
            popup.SetActive(true);
            return;
        }

        if (phone.text.Length != 10 || !System.Text.RegularExpressions.Regex.IsMatch(phone.text, @"^\d+$"))
        {
            Debug.Log("Invalid phone number format"); //Popup for invalid phone number
            signUpPannel.SetActive(true);
            title.text = "Incorrect phone number";
            message.text = "Enter a correct 10-digit phone number";
            popup.SetActive(true);
            return;
        }

        if (password.text != confirmPassword.text)
        {
            Debug.Log("Password Mismatched"); //Popup for password mismatch
            signUpPannel.SetActive(true);
            title.text = "Password mismatch";
            message.text = "Enter the same password in both password fields";
            popup.SetActive(true);
            return;
        }

        StartCoroutine(Signup());
        emailShown.text = email.text;
    }

    private bool IsValidDateFormat(string date)
    {
        System.DateTime parsedDate;
        return System.DateTime.TryParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate);
    }
}
