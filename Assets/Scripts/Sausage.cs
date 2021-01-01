using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class Sausage : MonoBehaviour
{
    [SerializeField] private ObiActor actor;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] private GameManager gameManager;

    public Vector3 pos { get { return transform.position; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        actor = GetComponent<ObiActor>();

    }

    private void Update()
    {
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, 0); //Постоянный 0 на оси z
        transform.position = currentPos;
    }

    public void Push (Vector4 force)
    {
        for (int i = 0; i < actor.solverIndices.Length; i++)
        {
            //Временное решение для заморозки оси z
            //Vector4 zparticle = new Vector4(actor.solver.positions[i].x, actor.solver.positions[i].y, 0);
            //actor.solver.positions[i] = zparticle;

            actor.solver.velocities[i] += force;
        }
    }
}