using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
    public Wheel wheel;
    // Start is called before the first frame update
    void Start()
    {
        wheel = GetComponent<Wheel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
