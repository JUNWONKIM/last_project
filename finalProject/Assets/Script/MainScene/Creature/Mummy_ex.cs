using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy_ex : MonoBehaviour
{
    public float damageAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    void OnTriggerEnter(Collider other)
    {
        PlayerHP playerHP = other.gameObject.GetComponent<PlayerHP>();
        if (playerHP != null)
        {
            playerHP.hp -= damageAmount; // �÷��̾��� ü���� ����
        }
        
    }   
}
