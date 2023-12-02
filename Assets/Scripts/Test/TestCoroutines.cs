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

        // Отмена задачи через 3 секунды.
        Task.Delay(3000).ContinueWith(t => cancelTokenSource.Cancel());

        // Task 3.
        WhatTaskFasterAsync(cancelToken, task1, task2);

        cancelTokenSource.Dispose();
    }

    /*Задача: реализовать корутину, которая будет вызываться из метода RecieveHealing, 
     * чтобы юнит получал исцеление 5 жизней каждые полсекунды в течение 3 секунд или до тех пор, 
     * пока количество жизней не станет равным 100. 
     * На юнит не может действовать более одного эффекта исцеления одновременно.
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

    /*Реализовать две задачи: Task1 и Task2. В качестве параметров задачи должны принимать CancellationToken. 
     * Первая задача должна ожидать одну секунду, а после выводить в консоль сообщение о своём завершении. 
     * Вторая задача должна ожидать 60 кадров, а после — выводить сообщение в консоль.
     */
    async Task Task1(CancellationToken cancelToken)
    {
        await Task.Delay(1000, cancelToken);
        Debug.Log("Task1 completed.");
    }

    async Task Task2(CancellationToken cancelToken)
    {
        await Task.Delay(16, cancelToken); // 60 кадров в секунду = 16 мс на кадр
        Debug.Log("Task2 completed.");
    }

    /*Реализовать задачу WhatTaskFasterAsync, которая будет принимать в качестве параметров CancellationToken, 
     * а также две задачи в виде переменных типа Task. Задача должна ожидать выполнения хотя бы одной из задач, 
     * останавливать другую и возвращать результат. Если первая задача выполнена первой, вернуть true, если вторая — false. 
     * Если сработал CancellationToken, также вернуть false.
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
