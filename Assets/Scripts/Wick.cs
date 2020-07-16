using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Wick : MonoBehaviour
{

    [SerializeField]
    private bool turn;

    [SerializeField]
    private GameObject portal;

    [SerializeField]
    private bool can;

    [SerializeField]
    private GameObject hero;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private Vector3 teleport_to;

    [SerializeField]
    private int stage;

    [SerializeField]
    private Vector3[] point;

    //private Vector3[] dots;

    private NavMeshAgent agent;

    [SerializeField]
    private float distance_between;

    [SerializeField]
    private float distance_to;

    [SerializeField]
    private bool on_x;

    [SerializeField]
    public Material[] materials;

    private Vector3 last_pos;

    void Start()
    {
        //gameObject.GetComponent<MeshRenderer>().material = materials[0];
        //dots = GameObject.Find("enemy").GetComponent<Enemy>().dots;
        agent = gameObject.GetComponent<NavMeshAgent>();
       


    }

    void Update()
    {
        
        //direction(transform.position,last_pos);
        distance_between = Vector3.Distance(transform.position, hero.transform.position);


        if (MoveToPoint(point[stage]))
        {
            stage = stage == 1 ? 0 : 1;
            if (on_x)
                transform.localScale = new Vector3(-transform.localScale.x, 1, 0.032f);
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = materials[stage];                
            }
              
        }
        
        // включение/выключение аниматора / аниматор конфликтует с чем-либо / включение / выключение - баг при анимации
        
        if (distance_between <= distance_to)
        {
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(transform.position, (hero.transform.position - transform.position).normalized, out hit, Vector3.Distance(transform.position, hero.transform.position));
            if (hit.collider == null && !(enemy.GetComponent<Enemy>().spotted) && can && enemy.GetComponent<Enemy>().can)
            {
               
                
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F))
                {

                    if (GameObject.Find("teleportation_Timer") == null)
                    {


                        gameObject.GetComponent<Animator>().enabled = true;

                        gameObject.GetComponent<NavMeshAgent>().enabled = false;
                        can = false;
                        teleport_to = new Vector3(hero.transform.position.x, enemy.transform.position.y, hero.transform.position.z);
                        if ((transform.position.z - teleport_to.z < 0 && transform.localScale.x < 0) || transform.position.z - teleport_to.z > 0 && transform.localScale.x > 0)
                        {
                            turn = true;
                            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        }
                        GameObject portal_ = Instantiate(portal, teleport_to, new Quaternion());
                        portal_.transform.eulerAngles = new Vector3(0, 90, 0);
                        GameObject.Find("portal(Clone)").GetComponent<Animator>().SetBool("open portal", true);
                        gameObject.GetComponent<Animator>().SetBool("open portal", true);
                        Timer.StartNewTimer("teleportation", 3, 1, delegate
                        {
                            if (on_x)
                                gameObject.GetComponent<MeshRenderer>().material = materials[0];
                            else
                                gameObject.GetComponent<MeshRenderer>().material = materials[stage];
                            if(!(enemy.GetComponent<Enemy>().spotted || enemy.GetComponent<Enemy>().chase))
                            {
                                enemy.GetComponent<NavMeshAgent>().enabled = false;
                                enemy.GetComponent<Enemy>().chase = true;
                                enemy.transform.position = teleport_to;
                                enemy.GetComponent<Enemy>().memory = new Vector3(hero.transform.position.x, enemy.transform.position.y, hero.transform.position.z);
                                enemy.GetComponent<NavMeshAgent>().enabled = true;
                            }
                            
                            gameObject.GetComponent<NavMeshAgent>().enabled = true;
                            gameObject.GetComponent<Animator>().SetBool("open portal", false);


                            gameObject.GetComponent<Animator>().enabled = false;

                            Destroy(GameObject.Find("portal(Clone)"));
                            Timer.StartNewTimer("cooldown", 1, 1, delegate
                            {
                                if (turn)
                                {
                                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                                    turn = false;
                                }
                                can = true;
                            }
                            );
                            gameObject.GetComponent<Animator>().SetTrigger("end_animation");
                        }
                       );
                    }
                }
            }
            

        }        
        last_pos = transform.position;
    }

    public bool MoveToPoint(Vector3 target)
    {
        if (can)
            agent.SetDestination(new Vector3(target.x, agent.transform.position.y, target.z));
        return target.x == transform.position.x && transform.position.z == target.z;
    }
}

