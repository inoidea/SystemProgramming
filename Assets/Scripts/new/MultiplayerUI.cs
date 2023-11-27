using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;


public class MultiplayerUI : MonoBehaviour
{
    [SerializeField] Button startHostBtn;
    [SerializeField] Button shutdownHostBtn;
    [SerializeField] Button clientConnectBtn;
    [SerializeField] Button clientDisconnectBtn;
    [SerializeField] Button sendMessageBtn;
    [SerializeField] TMP_InputField chatInput;

    void Awake()
    {
        AssignInputs();
    }

    void AssignInputs()
    {
        startHostBtn.onClick.AddListener( delegate { NetworkManager.Singleton.StartHost(); } );
        shutdownHostBtn.onClick.AddListener( delegate { NetworkManager.Singleton.Shutdown(); } );
        clientConnectBtn.onClick.AddListener( delegate { NetworkManager.Singleton.StartClient(); } );
        clientDisconnectBtn.onClick.AddListener(delegate { NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId); });
        sendMessageBtn.onClick.AddListener(() => SendMessage());
    }

    private void SendMessage()
    {
        ChatManager.Singleton.SendChatMessage(chatInput.text);
        chatInput.text = "";
    }
}
