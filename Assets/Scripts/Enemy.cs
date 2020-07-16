using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.SceneManagement;


public class Enemy : MonoBehaviour {

    public bool can;

    public bool attack;

    [SerializeField]
    private bool need_add;

    private Vector3 old_pos;

    private Vector3 last_pos;   

    [SerializeField]
    private int to_freedom;

    [SerializeField]
    private int damage;

    [SerializeField]
    private GameObject trap;

    [SerializeField]
    private int period;

    [SerializeField]
    Vector2[] dots_2;

    public Vector3[] dots;

    [SerializeField]
    private float Z;

    [SerializeField]
    private float speed;    

    [SerializeField]
    private float chase_speed;

    [SerializeField]
    public int stage;

    [SerializeField]
    private float distance_chase;

    [SerializeField]
    private float dist_to_catch;

    [SerializeField]
    private GameObject hero;

    [SerializeField]
    private GameObject blood;

    [SerializeField]
    private AudioClip scream;

    public bool chase;
   
    private NavMeshAgent agent;
    
    public bool spotted;
    
    public Vector3 memory;

    [SerializeField]
    private GameObject map;

    [SerializeField]
    private float volume;

    [SerializeField]
    private GameObject blood_partical;
    

    void Awake ()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = chase_speed;        

        period = new System.Random().Next(15,20);

        // Преобразование 2D точек в 3D
        dots = new Vector3[dots_2.Length];
        for (int i = 0; i < dots_2.Length; i++)
            dots[i] = new Vector3(-dots_2[i].y + 1.65f, -Z, dots_2[i].x + 6.745f);

        // начальная позиция
       
        int rnd = new System.Random().Next(0, 2);
        transform.position = dots[rnd == 0 ? 0 : 46];
        if (rnd != 0)
            stage = 47;
        agent.enabled = true;
        //transform.position = new Vector3(-13.473f, 9.82f, -3.306f);

        last_pos = transform.position; //
        old_pos = transform.position;

        Timer.StartNewTimer("check", 5, -1, delegate
           {
               if (old_pos == transform.position)
               {
                   chase = false;
                   spotted = false;
               }
               old_pos = transform.position;
           });
	}
	
    private int direction(Vector3 last_pos, Vector3 pos)
    {
        Vector3 delta = pos - last_pos;
        delta.y = 0;
        transform.localScale = new Vector3(delta.z<0 ? -0.02300004f : 0.02300004f, 1, 0.04800006f);
        if (Math.Abs(transform.position.x - hero.transform.position.x)>= 0.4039993)
        {
            if (delta.x < 0)
                return 3;
            if (delta.x > 0)
                return 2;
            else return 0;
        }
        else
        {            
            if (delta.z != 0)
                return 1;
            else return 0;
        }      
    }

	
	void Update ()
    {
        GetComponent<Animator>().SetBool("walkside", false);
        GetComponent<Animator>().SetBool("walkdown", false);
        GetComponent<Animator>().SetBool("walkup", false);
        
        //if(!spotted && !)


        //transform.localScale = new Vector3(transform.position.z > hero.transform.position.z ? -0.02300004f : 0.02300004f, 1, 0.04800006f);        

        /*if(transform.position.z > hero.transform.position.z)
        {
            AnimatorOverrideController myOverrideController = new AnimatorOverrideController();
            myOverrideController.runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
            myOverrideController["Idle"] = clip;
            GetComponent<Animator>().runtimeAnimatorController = myOverrideController;
        }*/
        

        //if (Input.GetKeyDown(KeyCode.P))
            //enabled = false;
        switch(direction(last_pos, transform.position))
        {
            case 1:
                GetComponent<Animator>().SetBool("walkside",true);
                break;
            case 2:
                GetComponent<Animator>().SetBool("walkdown", true);
                break;
            case 3:                
                GetComponent<Animator>().SetBool("walkup", true);
                break;
        }
                                       //< -------
        //Debug.Log(Math.Abs(transform.position.x) - Math.Abs(hero.transform.position.x));
        if (can)
        {                      

            if (chase || spotted)
            {
                if (gameObject.GetComponent<AudioSource>().clip != scream)
                {
                    gameObject.GetComponent<AudioSource>().clip = scream;
                    gameObject.GetComponent<AudioSource>().Play();                    
                }                                   
                if (map.GetComponent<AudioSource>().clip != map.GetComponent<Audio_Map>().chase)
                    map.GetComponent<Audio_Map>().play(map.GetComponent<Audio_Map>().chase, 2);
                map.GetComponent<AudioSource>().volume = 1.5f / Vector3.Distance(hero.transform.position, transform.position);
            }
            else
            {
                need_add = true;
                gameObject.GetComponent<AudioSource>().clip = null;
                if (map.GetComponent<AudioSource>().clip.name != map.GetComponent<Game>().background.name && map.GetComponent<AudioSource>().clip.name != map.GetComponent<Audio_Map>().opening.name)
                {
                    
                    if (GameObject.Find("volume_minus_Timer") == null)
                        Timer.StartNewTimer("volume_minus", 0.2f, -1, delegate {
                            if (map.GetComponent<AudioSource>().volume != 0)
                            {
                                map.GetComponent<AudioSource>().volume -= 0.01f;
                            }
                            else
                            {
                                Timer.StopTimer("volume_minus"); map.GetComponent<AudioSource>().volume = 1; map.GetComponent<Audio_Map>().play(map.GetComponent<Game>().background, 5); Destroy(GameObject.Find("volume_minus_Timer"));
                            }
                        }
                        );
                }
                if (GameObject.Find("trap_Timer") == null)
                    Timer.StartNewTimer("trap", period, -1, delegate
                    {
                        if(!spotted && !chase && can)
                        {
                            GameObject trap_ = Instantiate(trap, new Vector3(transform.position.x + 0.16f, 8.41f, transform.position.z), new Quaternion());
                            trap_.GetComponent<Trap>().hero = GameObject.Find("hero");
                            trap_.transform.eulerAngles = new Vector3(0, 90, 0);
                        }                       
                    }                                               
                    );
            }            
            if (!chase)
            {                
                if (!(stage < dots.Length))
                    stage = 1;               
                if (MoveToPoint(dots[stage]))
                {
                    stage++;
                    //Debug.Log(stage-1);
                }
                    
            }
            float distance_between = Vector2.Distance(new Vector2(transform.position.z, transform.position.x), new Vector2(hero.transform.position.z, hero.transform.position.x));
            if (distance_between <= distance_chase)
            {
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(transform.position, (hero.transform.position - transform.position).normalized, out hit, Vector3.Distance(transform.position,hero.transform.position));
                //Debug.DrawRay(transform.position, (hero.transform.position - transform.position).normalized);
                if (hit.collider == null)
                {
                    //gameObject.GetComponent<MeshRenderer>().enabled = true;
                    if (!hero.GetComponent<Hero>().hide)
                    {
                        spotted = true;
                        memory = new Vector3(hero.transform.position.x, transform.position.y,hero.transform.position.z);
                        //memory = new Vector3(hero.transform.position.x, transform.position.y, hero.transform.position.z);
                        chase = true;
                        Chase(hero.transform.position); 
                    }     
                    else
                    {
                        chase = false;
                        spotted = false;
                    }
                }
                else
                {
                    spotted = false;                    
                    //gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else spotted = false;
            if (!spotted && chase)
            {
                if (need_add)
                {
                    if (transform.position.z - memory.z > 0)
                        memory.z -= 0.35f;
                    else if (transform.position.z - memory.z < 0)
                        memory.z += 0.35f;

                    if (transform.position.x - memory.x > 0)
                        memory.x -= 0.35f;
                    else if (transform.position.x - memory.x > 0)
                        memory.x += 0.35f;
                    need_add = false;

                   
                 //   Debug.Log(hit2.collider);
                }
               
                //memory = hero.transform.position;

                Chase(memory);

                /*RaycastHit hit2 = new RaycastHit(); // <------ закончить
                Physics.Raycast(memory, new Vector3(0, memory.y - 50, 0), out hit2, 50);
                Debug.DrawRay(memory, new Vector3(0, memory.y - 50, 0), Color.red);                */

                //MoveToPoint(memory);

                //if (memory.x - transform.position.x < 0.009 && memory.z - transform.position.z < 0.009)
                if (memory.x == transform.position.x && memory.z == transform.position.z)                
                    chase = false;

                //Debug.Log(hit2.collider.name);

                //if(memory.x==transform.position.x&&memory.z==transform.position.z)

            }
        }       
        volume = map.GetComponent<AudioSource>().volume;
        last_pos = transform.position;
    }

    /// <summary> 
    /// Передвижение от точи a до точки b со скоростью speed
    /// </summary> 
    /// <param name="a">Начальное положение</param>
    /// <param name="b">Конечное положение положение</param>
    /// <param name="speed">Скорость перемещения</param>
    private bool MoveToPoint(Transform man,Vector3 a, Vector3 b, float speed) // не используется
    {
        
        Vector3 delta = (b-a).normalized;
        delta.y = 0;

        bool m_z = man.transform.position.z - b.z<0;
        bool m_x = man.transform.position.x - b.x < 0;
      
       
        man.transform.position+=(delta * speed);

      
        bool m_z_p = man.transform.position.z - b.z < 0;
        bool m_x_p = man.transform.position.x - b.x < 0;

       
        return man.transform.position == dots[stage] || m_z != m_z_p || m_x != m_x_p;
    }

    private void Chase(Vector3 target) // погоня за игроком
    {
        //transform.localScale = new Vector3(transform.position.z > target.z ? -0.02300004f : 0.02300004f, 1, 0.04800006f);

        //Debug.DrawRay(new Vector3(transform.position.x, hero.transform.position.y, transform.position.z), target - transform.position);

        float dist = Vector3.Distance(transform.position, hero.transform.position);


        //Debug.Log();
        //MoveToPoint(transform, transform.position,hero.transform.position, speed);        

        if (dist > dist_to_catch && !attack)            
            agent.SetDestination(new Vector3(target.x, agent.transform.position.y, target.z));
        else if(!(hero.GetComponent<Hero>().hide))
        {
            
            attack = true;

            GetComponent<NavMeshAgent>().enabled = false;

            GameObject.Find("hero").transform.FindChild("Main Camera").GetComponentInChildren<MeshRenderer>().enabled = true;
            GameObject.Find("attack").transform.position= new Vector3(hero.transform.position.x - 0.089517f, 13, hero.transform.position.z + 0.159029f);
            GameObject.Find("attack").GetComponent<Animator>().SetTrigger("attack");

            //GetComponent<Animator>().runtimeAnimatorController.animationClips[3] = clip;

            
            if (target == hero.transform.position && can == true && !(hero.GetComponent<Hero>().hide))
            {
                //Debug.Log(target + " "+hero.transform.position);
                //GetComponent<Animator>().SetTrigger("attack");
                //transform.localScale = new Vector3(0.047f, 1, 0.051f);
                hero.GetComponent<Hero>().can = false;
                //hero.GetComponent<SpriteRenderer>().enabled = false;
                can = false;                
                if (GameObject.Find("catch_Timer") == null)
                    Timer.StartNewTimer("catch", 1, -1, delegate
                    {
                        GetComponent<NavMeshAgent>().enabled = false;
                        switch (to_freedom)
                        {
                            case 2:
                                
                                hero.GetComponent<Hero>().hp -= damage;
                                //GetComponent<Animator>().SetBool("attack", false);
                                break;
                            case 3:
                                //transform.localScale = new Vector3(0.024f,1,0.048f);
                                //hero.GetComponent<SpriteRenderer>().enabled = true;                                
                                if (hero.GetComponent<Hero>().hp <= 0)
                                {
                                    hero.GetComponent<Hero>().can = false;
                                    SceneManager.LoadScene("Menu");
                                }
                                else
                                {
                                    hero.GetComponent<Hero>().can = true;
                                    Instantiate(blood_partical, new Vector3(hero.transform.position.x + 0.15f, hero.transform.position.y - 3.41f, hero.transform.position.z), new Quaternion(), hero.transform);
                                }
                               
                                break;
                            case 8:
                                if(hero.GetComponent<Hero>().hp > 0)
                                {
                                    attack = false;
                                    can = true;
                                    to_freedom = -1;
                                    Destroy(GameObject.Find("catch_Timer"));
                                    chase = false;
                                    GetComponent<NavMeshAgent>().enabled = true;
                                }                                                         
                                break;
                        }
                        to_freedom++;
                    }
                );
            }                       
        }        
    }

    public bool MoveToPoint(Vector3 target) // передвижение в конечную точку
    {
        if (can)
        {
            //transform.localScale = new Vector3(transform.position.z > target.z ? -0.02300004f : 0.02300004f, 1, 0.04800006f);
            agent.SetDestination(new Vector3(target.x, agent.transform.position.y, target.z));
        }       
                           
        return target.x == transform.position.x && transform.position.z == target.z;        
    }        

    public void disap() // начало анимации атаки
    {               
        //Debug.Log(GetComponent<Audio_Map>().attack);
        map.GetComponent<Audio_Map>().play(map.GetComponent<Audio_Map>().attack, 0);
        //GetComponent<AudioSource>().clip = attack;       
        hero.GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Stop();
        //transform.position = new Vector3(hero.transform.position.x- 0.089517f, 13, hero.transform.position.z+ 0.159029f);    <--------
    }

    public void show() // окончание анимации атаки
    {
        while (Vector3.Distance(transform.position, hero.transform.position) < 5f)
        {
            map.GetComponent<AudioSource>().volume = 1;
            spotted = false;
            chase = false;
            int rnd = new System.Random().Next(0, 78);
            transform.position = new Vector3(dots[rnd].x, 9.82f, dots[rnd].z);
        }

        if (hero.GetComponent<Hero>().hp > 0)
            GameObject.Find("hero").transform.FindChild("Main Camera").GetComponentInChildren<MeshRenderer>().enabled = false;
        map.GetComponent<Audio_Map>().play(map.GetComponent<Game>().background, 1.2f);
        hero.GetComponent<AudioSource>().Play();
        hero.transform.position = new Vector3(hero.transform.position.x, hero.transform.position.y, hero.transform.position.z);
        hero.transform.localScale = new Vector3(transform.localScale.x > 0 ? hero.transform.localScale.x : -hero.transform.localScale.x, hero.transform.localScale.y, hero.transform.localScale.z);
        GameObject blood_ = Instantiate(blood, new Vector3(hero.transform.position.x + 0.15f, 8.41f, hero.transform.position.z), new Quaternion());
        blood_.transform.eulerAngles = new Vector3(0, 90, 0);       
        hero.GetComponent<Hero>().adrenaline = 0;
    }      
}
