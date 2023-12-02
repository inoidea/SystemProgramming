using TMPro;
using UnityEngine;


public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;

    public void SetMessage(string message, string userName)
    {
        textField.text = $"<color=green>{userName}</color>: {message}";
    }
}
