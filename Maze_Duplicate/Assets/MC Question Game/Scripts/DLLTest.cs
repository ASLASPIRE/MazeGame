using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DLLTest : MonoBehaviour
{
    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public IList<string> Roles { get; set; }
    }
    // Start is called before the first frame update
    void Start()
    {
        string json = @"{
            'Email': 'james@example.com',
            'Active': true,
            'Roles': [
                'User',
                'Admin'
            ]
        }";


        Account account = JsonConvert.DeserializeObject<Account>(json);
        Debug.Log(account.Email);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
