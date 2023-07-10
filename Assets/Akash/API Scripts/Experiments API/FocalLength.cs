using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class FocalLength : MonoBehaviour
{
    public TMP_InputField[] answers;
    public SendApiExp sendApi;
    public string dummyData;

    public TMP_InputField inp1, inp2, inp3;
    public void FocalL()
    {
        StartCoroutine(fl());
    }

    IEnumerator fl()
    {
        string apiUrl = "https://echo-admin-backend.vercel.app/api/experiments/";

        flDataSend data = new flDataSend
        {
            experimentName = "Focal length of convex lens",
            moduleName = "Focal length of convex lens",
            user = Get_Student_Details.guid,
            questions = new List<flQuestion>()
        };

        flQuestion flque = new flQuestion
        {
            //Questions
            question =
            "Observation:\r\n 1. Least count of the meter scale =  ____ mm.\r\n" +
            "Observation table 1: \r\n 3 lenses available with focal length values 12,14 and 16\r\n No, Distant object, Distance between lens and screen\r\n" +
            "1, Net, ____" +
            "Observation table 2: \r\n Convex lens from back surface facing the object\r\n No, Distant object, Distance between lens and screen\r\n" +
            "1, Net, ____" +
            "Inference:\r\n 1. First focal length of the convex lens(F1) = ____cm\r\n Second focal length of the convex lens(F2) = ____cm\r\n 3. From 1 and 2 above, is the lens used in the experiment a symmetric lens? Yes/No\r\n",

            //answers
            answer =
            "Observation:\r\n 1. 1mm.\r\n" +
            "Observation table 1: \r\n 16\r\n" +
            "Observation table 2: \r\n 16\r\n" +
            "Inference:\r\n 1. 16cm\r\n \r\n2. 16cm \r\n Yes\r\n",

            //attempted answers
            attemptedanswer = new List<string>()
        };
        data.questions.Add(flque);

        dummyData = "Observation table 1: \r\n 16\r\n" +
            "Observation table 2: \r\n 16\r\n";
        flque.attemptedanswer.Add(dummyData);
        /*foreach (TMP_InputField answerField in answers)
        {
            data.questions[0].attemptedanswer.Add(answerField.text);
        }

        int emptyCount = answers.Length - answers.Length;
        for (int i = 0; i < emptyCount; i++)
        {
            data.questions[0].attemptedanswer.Add("");
        }*/

        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = UnityWebRequest.Post(apiUrl, "application/json");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Experiments data sent successfully!");
            sendApi.SuccessAPISentPopup();
        }
        else
        {
            Debug.Log("Failed to send experiments data. Error: " + request.error);
            sendApi.UnsuccessAPISentPopup();
        }
    }
}

[System.Serializable]
public class flDataSend
{
    public string experimentName;
    public string moduleName;
    public string user;
    public List<flQuestion> questions;
    public string marks;
    public List<flComment> comments;
}

[System.Serializable]
public class flQuestion
{
    public string question;
    public string answer;
    public List<string> attemptedanswer;
}

[System.Serializable]
public class flComment
{
    public string name;
    public string comments;
}
