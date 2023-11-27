using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Singleton;

    [SerializeField] EnterName enterNamePrefab;
    [SerializeField] ChatMessage chatMessagePrefab;
    [SerializeField] CanvasGroup chatContent;
    [SerializeField] public Dictionary<ulong, string> users = new Dictionary<ulong, string>();

    void Awake() 
    { 
        ChatManager.Singleton = this;
        EnterName enterName = Instantiate(enterNamePrefab);
        enterName.UserID = OwnerClientId;
    }

    public void SendChatMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        SendChatMessageServerRpc(message);
    }

    void AddMessage(string message)
    {
        ChatMessage chatMessage = Instantiate(chatMessagePrefab, chatContent.transform);

        if (users.TryGetValue(OwnerClientId, out string userName))
            chatMessage.SetMessage(message, userName);
    }

    [ServerRpc(RequireOwnership = false)]
    void SendChatMessageServerRpc(string message)
    {
        ReceiveChatMessageClientRpc(message);
    }

    [ClientRpc]
    void ReceiveChatMessageClientRpc(string message)
    {
        ChatManager.Singleton.AddMessage(message);
    }
}
