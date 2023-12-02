using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;


public class MultiplayerUI : MonoBehaviour
{
    [SerializeField] Button startHostBtn;
    [SerializeField] Button clientConnectBtn;
    [SerializeField] Button sendMessageBtn;
    [SerializeField] TMP_InputField chatInput;

    void Awake()
    {
        AssignInputs();
    }

    void AssignInputs()
    {
        startHostBtn.onClick.AddListener( delegate { NetworkManager.Singleton.StartHost(); } );
        clientConnectBtn.onClick.AddListener( delegate { NetworkManager.Singleton.StartClient(); } );
        sendMessageBtn.onClick.AddListener(() => SendMessage());
    }

    private void SendMessage()
    {
        ChatManager.Singleton.SendChatMessage(chatInput.text);
        chatInput.text = "";
    }
}
