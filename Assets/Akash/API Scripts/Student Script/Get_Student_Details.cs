using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Get_Student_Details : MonoBehaviour
{
    public TMP_InputField[] inputFields;
    public TextMeshProUGUI[] instituteText;
    public TextMeshProUGUI title, message;
    public SpriteRenderer instituteImageRenderer; private Transform instituteImageTransform;
    public TMP_Text fillerEmail;
    public TMP_InputField firstDigit, secondDigit, thirdDigit, fourthDigit;
    public TMP_InputField otpBox;
    public GameObject popup, verifyOTPPage, hamPremium, hamFree, BioPremium, BioFree, Physicspremium, PhysicsFree, ChemistryPremium, ChemistryFree;

    //User
    public static string image, guid, username, dob, email, phone, instituteId, @class, physics, biology, chemistry, registrationDate, deviceKey, subsplan, currentKey;

    //Institute
    public static string _id, instituteGuid, displayId, createdAt, InstitutedisplayId, Institutename, InstituteregistrationDate, lastTransactionDate, paymentStatus,
        numberOfKeys, numberOfStudents, numberOfTeachers, pincode, teachersInfo, adminusername, adminpancard, adminaadharcard, adminemail, adminphone, emailOtpInfo,
        phoneOtpInfo, isEmailVerified, isPhoneVerified, isLoginActive, type, password, Instituteimage, slogan, organizationName, address, Instituteemail, Institutephone;

    void Start()
    {
        StartCoroutine(GetProfileData());
    }

    IEnumerator GetProfileData()
    {
        UnityWebRequest newRequest = UnityWebRequest.Get("https://echo.backend.cynotics.in/api/student/");
        yield return newRequest.SendWebRequest();

        if (newRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Data Retrieved");
            string jsonData = newRequest.downloadHandler.text;
            APIClasses.ApiResponse data = JsonUtility.FromJson<APIClasses.ApiResponse>(jsonData);

            // Access user data
            if (data != null && data.status == "success" && data.user != null)
            {
                image = data.user.image;
                guid = data.user.guid;
                username = data.user.username;
                dob = data.user.dob;
                email = data.user.email;
                phone = data.user.phone;
                instituteId = data.user.instituteId;
                @class = data.user.@class;
                physics = data.user.physics; biology = data.user.biology; chemistry = data.user.chemistry;
                registrationDate = data.user.registrationDate;
                deviceKey = data.user.deviceKey; subsplan = data.user.subsplan; currentKey = data.user.currentKey;

                _id = data.user.institute._id; instituteGuid = data.user.institute.guid; displayId = data.user.institute.displayId;
                createdAt = data.user.institute.createdAt; InstitutedisplayId = data.user.institute.displayId; Institutename = data.user.institute.name;
                InstituteregistrationDate = data.user.institute.registrationDate; lastTransactionDate = data.user.institute.lastTransactionDate;
                paymentStatus = data.user.institute.paymentStatus; numberOfKeys = data.user.institute.numberOfKeys; numberOfStudents = data.user.institute.numberOfStudents;
                numberOfTeachers = data.user.institute.numberOfTeachers; pincode = data.user.institute.pincode; teachersInfo = data.user.institute.teachersInfo;
                adminusername = data.user.institute.adminusername; adminpancard = data.user.institute.adminpancard; adminaadharcard = data.user.institute.adminaadharcard;
                adminemail = data.user.institute.adminemail; adminphone = data.user.institute.adminphone; emailOtpInfo = data.user.institute.emailOtpInfo; phoneOtpInfo = data.user.institute.phoneOtpInfo;
                isEmailVerified = data.user.institute.isEmailVerified; isPhoneVerified = data.user.institute.isPhoneVerified; isLoginActive = data.user.institute.isLoginActive;
                type = data.user.institute.type; password = data.user.institute.password; Instituteimage = data.user.institute.image; slogan = data.user.institute.slogan;
                organizationName = data.user.institute.organizationName; address = data.user.institute.address; Instituteemail = data.user.institute.email; Institutephone = data.user.institute.phone;

                inputFields[0].text = username; inputFields[1].text = email; inputFields[2].text = dob; inputFields[3].text = phone; inputFields[4].text = instituteId;
                instituteText[0].text = Institutename; instituteText[1].text = slogan;
                LoadInstImage();

            }
            else
            {
                Debug.Log("Error: Invalid API response format");
            }
        }
        else
        {
            Debug.Log("Error: Failed to retrieve data");
            Debug.Log(newRequest.error);
        }
    }

    public void LoadInstImage()
    {
        StartCoroutine(LoadImageFromURL(Instituteimage));
    }

    IEnumerator LoadImageFromURL(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        /*if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            int desiredWidth = 400;
            int desiredHeight = 400;

            Rect rect = new Rect(0, 0, Mathf.Min(desiredWidth, texture.width), Mathf.Min(desiredHeight, texture.height));
            Vector2 pivot = new Vector2(0.5f, 0.5f);

            Sprite sprite = Sprite.Create(texture, rect, pivot);
            instituteImageRenderer.sprite = sprite;
        }*/

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            instituteImageRenderer.sprite = sprite;

            /*// Scale the sprite to 80x80x80
            if (instituteImageTransform != null)
            {
                instituteImageTransform.localScale = new Vector3(80f, 80f, 80f);
            }*/
        }
        else
        {
            Debug.Log("Error loading image: " + request.error);
        }
    }

    public void Save()
    {
        if (!inputFields[1].text.Contains("@") || !inputFields[1].text.Contains("."))
        {
            title.text = "Invalid Email Format";
            message.text = "Enter a valid email format";
            popup.SetActive(true);
            return;
        }

        if (!IsValidDateFormat(inputFields[2].text))
        {
            title.text = "Invalid date format";
            message.text = "Enter the date in the format:- DD/MM/YYYY";
            popup.SetActive(true);
            return;
        }

        if (inputFields[3].text.Length != 10 || !System.Text.RegularExpressions.Regex.IsMatch(inputFields[3].text, @"^\d+$"))
        {
            title.text = "Incorrect phone number";
            message.text = "Enter a correct 10-digit phone number";
            popup.SetActive(true);
            return;
        }
        StartCoroutine(SendingOTPToEmail());
        fillerEmail.text = inputFields[1].text;
        Debug.Log(fillerEmail.text);
    }
    public void VerifyOTP()
    {
        /*if (firstDigit.text == string.Empty || secondDigit.text == string.Empty || thirdDigit.text == string.Empty || fourthDigit.text == string.Empty)
        {
            popup.SetActive(true);
            title.text = "Error";
            message.text = "Enter correct otp";
            return;
        }*/
        if (otpBox.text == string.Empty)
        {
            popup.SetActive(true);
            title.text = "Error";
            message.text = "Enter correct otp";
            return;
        }

        StartCoroutine(SendUserEnteredOtp());
    }
    public void Resend()
    {
        StartCoroutine(SendingOTPToEmail());
    }

    IEnumerator UpdateDetails()
    {
        string _url = "https://echo.backend.cynotics.in/api/student/";
        APIClasses.UserData newData = new APIClasses.UserData()
        {
            image = image,
            guid = guid,
            username = inputFields[0].text,
            dob = inputFields[2].text,
            email = inputFields[1].text,
            phone = inputFields[3].text,
            instituteId = inputFields[4].text,
            @class = @class,
            physics = physics,
            biology = biology,
            chemistry = chemistry,
            registrationDate = registrationDate,
            deviceKey = deviceKey,
            subsplan = subsplan,
            currentKey = currentKey,
            institute = new APIClasses.InstituteData
            {
                _id = _id,
                guid = instituteGuid,
                createdAt = createdAt,
                displayId = InstitutedisplayId,
                name = Institutename,
                registrationDate = InstituteregistrationDate,
                lastTransactionDate = lastTransactionDate,
                paymentStatus = paymentStatus,
                numberOfKeys = numberOfKeys,
                numberOfStudents = numberOfStudents,
                numberOfTeachers = numberOfTeachers,
                pincode = pincode,
                teachersInfo = teachersInfo,
                adminusername = adminusername,
                adminpancard = adminpancard,
                adminaadharcard = adminaadharcard,
                adminemail = adminemail,
                adminphone = adminphone,
                emailOtpInfo = emailOtpInfo,
                phoneOtpInfo = phoneOtpInfo,
                isEmailVerified = isEmailVerified,
                isPhoneVerified = isPhoneVerified,
                isLoginActive = isLoginActive,
                type = type,
                password = password,
                image = Instituteimage,
                slogan = slogan,
                organizationName = organizationName,
                address = address,
                email = Instituteemail,
                phone = Institutephone
            }
        };

        string jsonBody = JsonUtility.ToJson(newData);
        byte[] rawBody = Encoding.UTF8.GetBytes(jsonBody);

        Debug.Log(jsonBody);
        UnityWebRequest request = new UnityWebRequest(_url, "PATCH");
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(rawBody);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");
            SceneManager.LoadScene("Main Alpha Functionality Pages");
            
        }
        else
        {
            Debug.Log("error");
        }
    }

    public void HamburgerPremiumFn()
    {
        if (subsplan == "premium")
        {
            hamPremium.SetActive(true);
        }
        else if (subsplan == "free")
        {
            hamFree.SetActive(true);
        }
    }

    public void BiologyPremiumFn()
    {
        /*if (subsplan == "premium")
        {*/
            BioPremium.SetActive(true);
        /*}
        else if (subsplan == "free")
        {
            BioFree.SetActive(true);
        }*/
    }

    public void PhysicsPremiumFn()
    {
        /*if (subsplan == "premium")
        {*/
            Physicspremium.SetActive(true);
        /*}
        else if (subsplan == "free")
        {
            BioFree.SetActive(true);
        }*/
    }

    public void ChemistryPremiumFn()
    {
        /*if (subsplan == "premium")
        {*/
            ChemistryPremium.SetActive(true);
        /*}
        else if (subsplan == "free")
        {
            BioFree.SetActive(true);
        }*/
    }

    private bool IsValidDateFormat(string date)
    {
        System.DateTime parsedDate;
        return System.DateTime.TryParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate);
    }

    IEnumerator SendingOTPToEmail()
    {
        Debug.Log("OTP Sending Started");
        string _url = "https://echo.backend.cynotics.in/api/student/send-otp";

        APIClasses.SignUpDataHolder otpSend = new APIClasses.SignUpDataHolder()
        {
            email = inputFields[1].text
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
            verifyOTPPage.SetActive(true);
        }
        else
        {
            Debug.Log("error for otp");
            Debug.Log(request.error);
        }
    }
    IEnumerator SendUserEnteredOtp()
    {
        string _url = "https://echo.backend.cynotics.in/api/student/verify-otp";

        APIClasses.OtpSend otpHolder = new APIClasses.OtpSend()
        {
            email = inputFields[1].text,
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
            StartCoroutine(VerifyOTP1());
        }
        else
        {
            Debug.Log("Error to send OTP");
            popup.SetActive(true);
            message.text = "Error occured please enter otp again";
            Debug.Log(request.error);
        }

    }
    IEnumerator VerifyOTP1()
    {
        UnityWebRequest newRequest = UnityWebRequest.Get("https://echo.backend.cynotics.in/api/student/verify-otp");
        yield return newRequest.SendWebRequest();

        if (newRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Verified");
            StartCoroutine(UpdateDetails());
            Debug.Log(newRequest.result);
        }
        else
        {
            Debug.Log("Wrong OTP Entered");
            Debug.Log(newRequest.error);
        }
    }
}