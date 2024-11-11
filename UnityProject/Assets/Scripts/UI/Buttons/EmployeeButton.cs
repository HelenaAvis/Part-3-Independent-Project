using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeButton : MonoBehaviour
{

    private GameManager gameManager;
    public Employee employee;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Clicked()
    {
        gameManager.OpenEmailInspection(employee);
    }
}
