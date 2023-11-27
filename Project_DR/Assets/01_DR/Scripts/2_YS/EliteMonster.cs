using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EliteMonster : Monster
{
    

    public override IEnumerator MonsterAction()
    {
        while (true)
        {
            switch (state)
            {
                // IDLE 상태 =======================================================
                case State.IDLE:
                    //Debug.Log("IDLE state");
                    nav.isStopped = true;
                    anim.SetBool(hashRun, false);
                    anim.SetBool(hashidle, true);
                    //anim.SetBool(hashWalkingAttack, false);
                    anim.SetBool(hashAttack, false);
                    anim.SetBool(hashAttack2, false);
                    anim.SetBool(hashAttack3, false);
                    anim.SetBool(hashAttack4, false);
                    break;

                // TRACE 상태 =======================================================
                case State.TRACE:
                    //Debug.Log("TRACE state");
                    nav.isStopped = false;
                    nav.SetDestination(playerTr.position);
                    anim.SetBool(hashRun, true);
                    anim.SetBool(hashWalkingAttack, false);
                    anim.SetBool(hashAttackRuning, true);
                    anim.SetBool(hashAttackRuning2, true);
                    anim.SetBool(hashAttackRuning3, true);
                    break;

                // ATTACK 상태 =======================================================
                case State.ATTACK:

                    //Debug.Log("ATTACK state");

                    switch (monsterType)
                    {
                        case Type.HUMAN_ROBOTRED:

                            anim.SetBool(hashWalkingAttack, true);
                            anim.SetBool(hashAttack, true);
                            
                            yield return new WaitForSeconds(0.5f);
                            anim.SetBool(hashidle, true);
                            anim.SetBool(hashAttack, false);
                            anim.SetBool(hashRun, false);
                            yield return new WaitForSeconds(0.3f);
                            break;

                        case Type.HUMAN_GOLEMFIRE:

                            int humanGolemFire = Random.Range(0, 3);

                            switch (humanGolemFire)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack2, true);

                                    yield return new WaitForSeconds(1.3f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    //anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack3, true);
                                    yield return new WaitForSeconds(1.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                            }
                            break;

                        case Type.BEAST_SCORPION:

                            int spider = Random.Range(0, 3);

                            switch (spider)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                            }
                            break;

                        case Type.BEAST_QUEENWORM:

                            int sting = Random.Range(0, 3);

                            switch (sting)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);
                                    yield return new WaitForSeconds(0.8f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;
                                    //case 3:
                                    //    anim.SetBool(hashAttack4, true);
                                    //    yield return new WaitForSeconds(0.8f);
                                    //    anim.SetBool(hashidle, true);
                                    //    anim.SetBool(hashAttack4, false);
                                    //    //anim.SetBool(hashWalkingAttack, false);
                                    //    anim.SetBool(hashRun, false);
                                    //    yield return new WaitForSeconds(0.3f);
                                    //    break;
                            }
                            break;

                        case Type.SIMPLE_TOADSTOOL:

                            int toadStool = Random.Range(0, 3);

                            switch (toadStool)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);
                                    anim.SetBool(hashidle, false);

                                    GameObject instantBullet = Instantiate(monsterBullet, bulletPort.position, bulletPort.rotation);
                                    MonsterBullet bullet = instantBullet.GetComponent<MonsterBullet>();
                                    bulletPort.transform.LookAt(playerTr);
                                    instantBullet.transform.LookAt(playerTr);

                                    yield return new WaitForSeconds(0.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);
                                    anim.SetBool(hashidle, false);


                                    GameObject instantBulletRight = Instantiate(monsterBullet, bulletPortRight.position, bulletPortRight.rotation);
                                    MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                                    bulletPortRight.transform.LookAt(playerTr);
                                    instantBulletRight.transform.LookAt(playerTr);

                                    yield return new WaitForSeconds(0.2f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);
                                    anim.SetBool(hashidle, false);


                                    GameObject instantBulletLeft = Instantiate(monsterBullet, bulletPortLeft.position, bulletPortLeft.rotation);
                                    MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                                    bulletPortLeft.transform.LookAt(playerTr);
                                    instantBulletLeft.transform.LookAt(playerTr);

                                    yield return new WaitForSeconds(0.7f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;
                            }
                            break;

                        case Type.SIMPLE_PHANTOM:

                            int phantom = Random.Range(0, 3);

                            switch (phantom)
                            {
                                case 0:
                                    anim.SetBool(hashWalkingAttack, true);
                                    anim.SetBool(hashAttack, true);

                                    GameObject instantBullet = Instantiate(monsterBullet, bulletPort.position, bulletPort.rotation);
                                    MonsterBullet bullet = instantBullet.GetComponent<MonsterBullet>();
                                    bulletPort.transform.LookAt(playerTr);
                                    instantBullet.transform.LookAt(playerTr);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    //anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 1:
                                    anim.SetBool(hashAttack2, true);

                                    GameObject instantBulletRight = Instantiate(monsterBullet, bulletPortRight.position, bulletPortRight.rotation);
                                    MonsterBullet bulletRight = instantBulletRight.GetComponent<MonsterBullet>();
                                    bulletPortRight.transform.LookAt(playerTr);
                                    instantBulletRight.transform.LookAt(playerTr);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack2, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    //anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;

                                case 2:
                                    anim.SetBool(hashAttack3, true);

                                    GameObject instantBulletLeft = Instantiate(monsterBullet, bulletPortLeft.position, bulletPortLeft.rotation);
                                    MonsterBullet bulletLeft = instantBulletLeft.GetComponent<MonsterBullet>();
                                    bulletPortRight.transform.LookAt(playerTr);
                                    instantBulletLeft.transform.LookAt(playerTr);

                                    yield return new WaitForSeconds(1.0f);
                                    anim.SetBool(hashidle, true);
                                    anim.SetBool(hashAttack3, false);
                                    //anim.SetBool(hashWalkingAttack, false);
                                    anim.SetBool(hashRun, false);
                                    yield return new WaitForSeconds(0.3f);
                                    break;
                            }
                            break;
                    }
                    break;


                case State.DIE:
                    isDie = true;
                    nav.isStopped = true;
                    //Debug.Log("nav.isStopped: " + nav.isStopped);
                    anim.SetTrigger(hashDie);

                    //Destroy(this.gameObject, 1.3f); //damageable 쪽에서 처리
                    yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }


    }
}
