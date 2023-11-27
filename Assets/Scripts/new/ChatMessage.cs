using TMPro;
using Unity.Netcode;
using UnityEngine;


public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;

    public void SetMessage(string message, string userName)
    {
        textField.text = $"<color=black>{userName}</color>: {message}";
    }
}
