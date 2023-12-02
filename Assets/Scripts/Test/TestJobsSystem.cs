using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;
using static TestJobsSystem;


public class TestJobsSystem : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;

    private void Start()
    {
        //Task1();
        Task2();
    }

    /*�������� ������ ���� IJob, ������� ��������� ������ � ������� NativeArray<int>  
     * � � ���������� ���������� ��� �������� ����� ������ ������ ������� ����.
     * �������� ���������� ���� ������ �� �������� ������ � �������� � ������� ���������.
     */
    private void Task1() {
        NativeArray<int> numberArray = new NativeArray<int>(new int[] { 1, 15, 21, 22, 3, 8 }, Allocator.Persistent);

        JobStruct jobStruct = new JobStruct()
        {
            number = numberArray,
        };

        JobHandle jobHandle = jobStruct.Schedule();
        jobHandle.Complete();

        for (int i = 0; i < numberArray.Length; i++)
            Debug.Log(numberArray[i]);

        numberArray.Dispose();
    }
    
    public struct JobStruct : IJob
    {
        public NativeArray<int> number;

        public void Execute() {
            for (int i = 0; i < number.Length; i++)
                if (number[i] > 10)
                    number[i] = 0;
        }
    }

    /*C������� ������ ���� IJobParallelFor, ������� ����� ��������� ������ � ���� ���� �����������: 
     * Positions � Velocities � ���� NativeArray<Vector3>. ����� �������� ������ FinalPositions ���� NativeArray<Vector3>. 
     * �������� ���, ����� � ���������� ���������� ������ � �������� ������� FinalPositions ���� �������� ����� 
     * ��������������� ��������� �������� Positions � Velocities. �������� ���������� ��������� ������ �� �������� ������ � �������� � ������� ���������.
     */
    private void Task2() {
        NativeArray<Vector3> positionsArray = new NativeArray<Vector3>(new Vector3[] { Vector3.up, Vector3.left }, Allocator.Persistent);
        NativeArray<Vector3> velocitiesArray = new NativeArray<Vector3>(new Vector3[] { Vector3.right, Vector3.down }, Allocator.Persistent);
        NativeArray<Vector3> finalPositionsArray = new NativeArray<Vector3>(new Vector3[] { Vector3.zero, Vector3.zero }, Allocator.Persistent);

        JobParallelStruct jobParallelStruct = new JobParallelStruct() 
        {
            positions = positionsArray,
            velocities = velocitiesArray,
            finalPositions = finalPositionsArray
        };

        JobHandle jobHandle = jobParallelStruct.Schedule(positionsArray.Length, 0);
        jobHandle.Complete();

        for (int i = 0; i < finalPositionsArray.Length; i++)
            Debug.Log($"{i} - {finalPositionsArray[i]}");

        positionsArray.Dispose();
        velocitiesArray.Dispose();
        finalPositionsArray.Dispose();
    }

    public struct JobParallelStruct : IJobParallelFor
    {
        public NativeArray<Vector3> positions;
        public NativeArray<Vector3> velocities;
        public NativeArray<Vector3> finalPositions;

        public void Execute(int index)
        {
            finalPositions[index] = positions[index] + velocities[index];
        }
    }
}
