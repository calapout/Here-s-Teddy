using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rubik : MonoBehaviour {
    public float decalement;

    private float _vitesse = 0.1f;
    private float _min;
    private float _max;
    // Use this for initialization
    void Start () {
        _min = gameObject.transform.position.y;
        _max = _min + decalement;
    }
	
	// Update is called once per frame
	void Update () {
        if ((transform.position.y >= _min && _vitesse < 0) || transform.position.y <= _max && _vitesse > 0) {
            transform.Translate(0, _vitesse * Time.deltaTime, 0, Space.World);
        }
        else {
            _vitesse *= -1;
        }
        Debug.Log(_vitesse);
	}
}
