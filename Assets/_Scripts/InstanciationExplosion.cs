using UnityEngine;

public class InstanciationExplosion : MonoBehaviour {

    Vector3 position;

    public void Boom()
    {
        position = transform.position;
        Instantiate(Resources.Load("Explosion_Res") as GameObject, position, new Quaternion());
    }
}