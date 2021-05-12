using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ServerCommunication : Singleton<ServerCommunication>
{
    #region [Server Communication]

    /// <summary>
    /// This class is responsible for handling REST API requests to remote server.  To extend this class you just need to add new API methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <param name="callbackOnSuccess"></param>
    /// <param name="callbackOnFail"></param>
    private  void SendRequest<T>(string url, UnityAction<T> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        StartCoroutine(RequestCoroutine(url, callbackOnSuccess, callbackOnFail));
    }

    /// <summary>
    /// This method is used to begin sending request process.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"> API URL </param>
    /// <param name="callbackOnSuccess"> Callback on success. </param>
    /// <param name="callbackOnFail"> Callback on Failure. </param>
    /// <returns></returns>
    private IEnumerator RequestCoroutine<T>(string url, UnityAction<T> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
            callbackOnFail?.Invoke(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            ParseResponse(www.downloadHandler.text, callbackOnSuccess, callbackOnFail);
        }
    }

    /// <summary>
    /// This method finishes request process as we have recived answer from server.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"> Data recieved from server in JSON format. </param>
    /// <param name="callbackOnSuccess"> Callback on Success. </param>
    /// <param name="callbackOnFail"> Callback on Fail. </param>
    private void ParseResponse<T>(string data, UnityAction<T> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        var parsedData = JsonUtility.FromJson<T>(data);
        callbackOnSuccess?.Invoke(parsedData);
    }

    #endregion

    #region API

    public void GetUserID(UnityAction<UserData> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        SendRequest(string.Format(ServerConfig.SERVER_API_URL_FORMAT, ServerConfig.API_GET_USER_ID), callbackOnSuccess, callbackOnFail);
    }

    public void GetLastActiveSession(UnityAction<GameStateData> callbackOnSuccess, UnityAction<string> callbackOnFail)
    {
        SendRequest(string.Format(ServerConfig.SERVER_API_URL_FORMAT, ServerConfig.API_GET_LAST_ACTIVE_SESSION), callbackOnSuccess, callbackOnFail);
    }

    #endregion
}
