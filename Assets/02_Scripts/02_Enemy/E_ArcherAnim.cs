using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ArcherAnim : MonoBehaviour
{
    public Animator bowAnim;
    public E_BowShot e_BowShot;

    public void BowAnim()
    {
        bowAnim.SetTrigger("BowShot");
    }

    public void ArrowShot()
    {
        e_BowShot.Bowshot();
    }
}
