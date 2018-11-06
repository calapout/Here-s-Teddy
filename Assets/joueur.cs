using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class joueur : MonoBehaviour {

    private Rigidbody _RB;
    private Animator _animator;
    private AudioSource _AS;

    public float multiplicateurGravite;
    public float multiplicateurSautMin;
    public float distanceRaycast;
    public float vitesseDeplacementSaut;

    //debugger
    public bool debugVelocite;
    public bool debugX;
    public bool debugY;
    public bool debugZ;
    public bool debugRaycast;



	// Use this for initialization
	void Start () {
        _RB = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _AS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
