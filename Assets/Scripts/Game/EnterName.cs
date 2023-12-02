using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EnterName : MonoBehaviour
{
    [SerializeField] TMP_InputField chatInput;
    [SerializeField] Button enterBtn;

    public ulong UserID {  get; set; }

    private void Awake()
    {
        enterBtn.onClick.AddListener(() => AddUser(chatInput.text));
    }

    private void AddUser(string userName)
    {
        var users = ChatManager.Singleton.users;

        if (!users.ContainsKey(UserID))
            users.Add(UserID, userName);

        gameObject.SetActive(false);
    }
}