using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIClasses : MonoBehaviour
{
    [Serializable]
    public class SignUpDataHolder
    {
        public string email, password, phone, instituteId, username, dob, deviceKey;
    }

    [Serializable]
    public class LoginDataHolder
    {
        public string email, password, deviceKey;

    }

    [Serializable]
    public class OtpSend
    {
        public string email;
        public string emailOTP;
    }

    [Serializable]
    public class ForgotPassword
    {
        public string email;
    }

    [Serializable]
    public class ResetPassword
    {
        public string email, emailOTP, password;
    }

    [Serializable]
    public class ApiResponse
    {
        public string status;
        public string message;
        public UserData user;
    }

    [Serializable]
    public class UserData
    {
        public string image;
        public string guid;
        public string username;
        public string dob;
        public string email;
        public string phone;
        public string instituteId;
        public string @class;
        public string physics;
        public string biology;
        public string chemistry;
        public string registrationDate;
        public string deviceKey;
        public string subsplan;
        public object[] institute;
    }
}

