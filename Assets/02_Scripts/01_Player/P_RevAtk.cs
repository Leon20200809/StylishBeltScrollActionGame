using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class P_RevAtk : MonoBehaviour
{
    bool isDead;

    public Slider hpSlider;
    public int maxHp;
    public int hp;

    PlayerController playerController;
    PlayerUIManager playerUI;
    Animator animator;
    Rigidbody rb;
    [SerializeField]
    Vector3 distination;

    public Transform gameOver;
    public GameObject gameoverImagePrefab;

    

    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
        {
            return;
        }
        //ダメージソース取得
        other.gameObject.TryGetComponent(out E_DamagerBase damager);

        // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
        Vector3 distination = (other.transform.position - transform.position).normalized;
        distination = new Vector3(distination.x, 0f, 0f).normalized;

        Debug.Log(other.gameObject.name);
        Debug.Log(other.gameObject.layer);

        //タグ判定
        if (other.CompareTag("E_Weapon"))
        {
            Damage(damager.atkPow_L);
            animator.SetTrigger("Rev-Atk");
            GenerateEffect(other.gameObject);
            Debug.Log("L-Atk食らった");
        }
        else if (other.CompareTag("E_ParryAtk"))
        {
            Damage(damager.atkPow_H);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            Debug.Log("H-Atk食らった");
            transform.DOLocalMove(distination * -2f, 0.5f).SetRelative();
        }
        else if (other.CompareTag("E_Magic"))
        {
            Damage(damager.atkPow_M);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -2f, 0.5f).SetRelative();

            Debug.Log("ダウン");
        }
        else if (other.CompareTag("E_SP_Atk"))
        {
            Damage(damager.atkPow_S);
            animator.SetTrigger("Rev-Down");
            GenerateEffect(other.gameObject);
            transform.DOLocalMove(distination * -5f, 0.5f).SetRelative();

            Debug.Log("大ダウン");
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="AtkPow"></param>
    void Damage(int AtkPow)
    {
        //HP減らす
        hp -= AtkPow;

        //撃破処理
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            animator.SetTrigger("Dead");
            SoundManager.instance.PlayVOICE(SoundManager.VOICE_Type.U8);
            StartCoroutine(Kankaku());

            //SceneManager.LoadScene("GameScene");
        }

        // TODO UIに現在のHPを反映
        UpdateHP(hp);
    }


    public void UpdateHP(int hp)
    {
        hpSlider.DOValue((float)hp / maxHp, 0.5f);
    }

    public void RecoverHP(int reHp)
    {
        hp += reHp;
        if (hp >= maxHp)
        {
            hp = maxHp;
        }
        UpdateHP(hp);
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        hpSlider = playerController.gameManager.uiManager.hpSlider;
        gameOver = playerController.gameManager.uiManager.transform;
        rb = GetComponent<Rigidbody>();
        maxHp = GameData.instance.charaDataList[0].hp;
        hp = maxHp;
        UpdateHP(hp);
    }

    public GameObject hitEffectPrefab;
    public Vector3 effecOfset;

    /// <summary>
    /// HITエフェクト再生
    /// </summary>
    /// <param name="other"></param>
    public void GenerateEffect(GameObject other)
    {
        SoundManager.instance.PlaySE(SoundManager.SE_Type.KatanaHit);
        GameObject effect = Instantiate(hitEffectPrefab, transform.position + effecOfset, transform.rotation);
        Destroy(effect, 2f);
    }

    IEnumerator Kankaku() //コルーチンメソッド変数名Kankaku
    {
        //(1.5秒の間を設ける)
        yield return new WaitForSeconds(3f);
        Debug.Log("Wait");
        GameObject gameoverObject = Instantiate(gameoverImagePrefab, gameOver, false);
    }


}
