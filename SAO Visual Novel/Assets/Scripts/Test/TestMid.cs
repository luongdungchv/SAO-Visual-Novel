using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMid : MonoBehaviour
{
    public GameObject pref1;
    public GameObject pref2;
    public List<Vector2> vectors;

    private void Start()
    {
        Vector2 sum = Vector2.zero;
        vectors.ForEach(n =>
        {
            sum += n;
            Instantiate(pref1, n, Quaternion.identity);
        });
        sum /= vectors.Count;
        Instantiate(pref2, sum, Quaternion.identity);
    }
}
