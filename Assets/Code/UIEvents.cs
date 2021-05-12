using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    public Text infoText;

    public void MainTap()
    {
        CallAPI();
    }

    public void CallAPI()
    {
        infoText.text = "";
        ServerCommunication.Instance.GetUserID(APICallSucceed, APICallFailed);
    }

    private void APICallSucceed(UserData userData)
    {
        infoText.text = string.Format("User ID: {0}\nLast Active: {1}", userData.id, userData.lastactive);
    }

    private void APICallFailed(string errorMessage)
    {
        infoText.text = string.Format("Error\n{0}", errorMessage);
    }
}
