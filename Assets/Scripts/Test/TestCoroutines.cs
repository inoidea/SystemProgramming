using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class TestCoroutines : MonoBehaviour
{
    private int _health;

    void Start()
    {
        // Task 1.
        //StartCoroutine(ReceiveHealing());

        // Task 2.
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken cancelToken = cancelTokenSource.Token;

        Task task1 = Task1(cancelToken);
        Task task2 = Task2(cancelToken);

        // ������ ������ ����� 3 �������.
        Task.Delay(3000).ContinueWith(t => cancelTokenSource.Cancel());

        // Task 3.
        WhatTaskFasterAsync(cancelToken, task1, task2);

        cancelTokenSource.Dispose();
    }

    /*������: ����������� ��������, ������� ����� ���������� �� ������ RecieveHealing, 
     * ����� ���� ������� ��������� 5 ������ ������ ���������� � ������� 3 ������ ��� �� ��� ���, 
     * ���� ���������� ������ �� ������ ������ 100. 
     * �� ���� �� ����� ����������� ����� ������ ������� ��������� ������������.
     */
    IEnumerator ReceiveHealing()
    {
        float healTimer = 0f;

        while (_health < 100 && healTimer < 3f)
        {
            yield return new WaitForSeconds(0.5f);

            _health += 5;
            healTimer += 0.5f;

            Debug.Log($"Current health: {_health}");
        }
    }

    /*����������� ��� ������: Task1 � Task2. � �������� ���������� ������ ������ ��������� CancellationToken. 
     * ������ ������ ������ ������� ���� �������, � ����� �������� � ������� ��������� � ���� ����������. 
     * ������ ������ ������ ������� 60 ������, � ����� � �������� ��������� � �������.
     */
    async Task Task1(CancellationToken cancelToken)
    {
        await Task.Delay(1000, cancelToken);
        Debug.Log("Task1 completed.");
    }

    async Task Task2(CancellationToken cancelToken)
    {
        await Task.Delay(16, cancelToken); // 60 ������ � ������� = 16 �� �� ����
        Debug.Log("Task2 completed.");
    }

    /*����������� ������ WhatTaskFasterAsync, ������� ����� ��������� � �������� ���������� CancellationToken, 
     * � ����� ��� ������ � ���� ���������� ���� Task. ������ ������ ������� ���������� ���� �� ����� �� �����, 
     * ������������� ������ � ���������� ���������. ���� ������ ������ ��������� ������, ������� true, ���� ������ � false. 
     * ���� �������� CancellationToken, ����� ������� false.
     */
    async Task<bool> WhatTaskFasterAsync(CancellationToken cancelToken, Task task1, Task task2)
    {
        bool result;
        Task completedTask = await Task.WhenAny(task1, task2);

        if (completedTask == task1)
        {
            if (cancelToken.IsCancellationRequested)
                result = false;
            else
            {
                cancelToken.ThrowIfCancellationRequested();
                Debug.Log("Cancel Task2");
                result = true;
            }
        }
        else
        {
            if (!cancelToken.IsCancellationRequested)
            {
                cancelToken.ThrowIfCancellationRequested();
                Debug.Log("Cancel Task1");
            }
            
            result = false;
        }

        Debug.Log($"Result {result}");

        return result;
    }
}
