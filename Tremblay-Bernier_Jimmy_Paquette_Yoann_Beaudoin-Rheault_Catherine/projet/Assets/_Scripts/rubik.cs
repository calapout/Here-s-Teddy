using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Classe permettant de faire monter et décendre un cube rubik
 * @author Jimmy Tremblay-Bernier
 */
public class rubik : MonoBehaviour {
    // variables publiques
    public float decalement;

    // variables privée
    private float _vitesse = 0.1f;
    private float _min;
    private float _max;

    // évênnement de départ
    void Start () {
        _min = gameObject.transform.position.y;
        _max = _min + decalement;
    }

    // boucle de mise à jour
    void Update () {
        if ((transform.position.y >= _min && _vitesse < 0) || transform.position.y <= _max && _vitesse > 0) {
            transform.Translate(0, _vitesse * Time.deltaTime, 0, Space.World);
        }
        else {
            _vitesse *= -1;
        }
	}
}
