using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    [SerializeField] private Button _serverStartBtn;
    [SerializeField] private Button _serverShutDownBtn;
    [SerializeField] private Button _clientConnect;
    [SerializeField] private Button _clientDisconnect;
    [SerializeField] private Button _sendMessage;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextField _textField;
    [SerializeField] private Server _server;
    [SerializeField] private Client _client;

    private void Start()
    {
        _serverStartBtn.onClick.AddListener(() => StartServer());
        _serverShutDownBtn.onClick.AddListener(() => ShutDownServer());
        _clientConnect.onClick.AddListener(() => Connect());
        _clientDisconnect.onClick.AddListener(() => Disconnect());
        _sendMessage.onClick.AddListener(() => SendMessage());
        _client.onMessageReceive += ReceiveMessage;
    }

    private void StartServer() => _server.StartServer();

    private void ShutDownServer() => _server.ShutDownServer();

    private void Connect() => _client.Connect();

    private void Disconnect() => _client.Disconnect();

    private void SendMessage()
    {
        _client.SendMessage(_inputField.text);
        _inputField.text = "";
    }

    public void ReceiveMessage(object message) => _textField.ReceiveMessage(message);
}
