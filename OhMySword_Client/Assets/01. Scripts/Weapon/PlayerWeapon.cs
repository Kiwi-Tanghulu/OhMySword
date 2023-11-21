using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Range(0.001f,0.2f)]
    [SerializeField] private float scoreSize; // ���ھ�� ������� ����! ��) 0.01�̶�� 1���� 0.01�� Ŀ�� 100���̶�� 1�� Ŀ����

    [SerializeField] private float sizeUpSpeed = 1f;

    [SerializeField] private Transform swordPivot;
    private int swordSize; // ���߿� �ʿ��� �� �־...
    public int SwordSize => swordSize;

    private int currentSwordLevel = 0;
    public int CurrentSwordLevel => currentSwordLevel;

    private ushort currentScore = 0;
    private ushort nextScore = 0;
    private bool isGrowing = false;

    private BoxCollider col;
    [SerializeField] private TrailRenderer trail;

    private List<Collider> attacked = new();

    private Transform ownerHitbox;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();

        ownerHitbox = transform.root.Find("Hips/Hitbox");
        playerAttack = transform.root.GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        SetCollision(false);
        SetTrail(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if(other.CompareTag("Other_Hitbox") && other.transform.root.TryGetComponent<IDamageable>(out IDamageable id))
        {
            if (other.transform == ownerHitbox)
                return;
            if (attacked.Contains(other))
                return;

            Debug.Log("Success Attack");
            id.OnDamage(1, transform.root.gameObject, Vector3.zero);
            attacked.Add(other);
        }

        attacked.Clear();
    }

    public void SetCollision(bool value)
    {
        col.enabled = value;
    }

    public void SetTrail(bool value)
    {
        if (value)
        {
            trail.Clear();
            trail.enabled = true;
        }
        else
        {
            trail.enabled = false;
            trail.Clear();
        }
    }

    public void SetSwordSize()
    {
        swordPivot.localScale = new Vector3(1f, swordPivot.localScale.y + (scoreSize * currentScore), 1f);
        col.center = new Vector3(-0.2f, 0.5f + swordPivot.localScale.y, 0.45f);
        col.size = new Vector3(0.3f, 2.4f + swordPivot.localScale.y * 2, 0.6f);
        trail.widthMultiplier = (currentScore / 100f) + 1;
        
        
    }
    public void SetScore(ushort value, bool isStart)
    {
        nextScore = value;
        if (isStart)
        {
            currentScore = value;
            SetSwordSize();
            playerAttack.SetAttackDelay(currentScore / 500f);
        }
        else
        {
            if (currentScore != nextScore && isGrowing == false)
                StartCoroutine(GrowUpSword());
        }
    }

    public ushort GetScore() => nextScore;

    private IEnumerator GrowUpSword()
    {
        float checkTime = 0f;
        isGrowing = true;
        while (true)
        {
            if(checkTime >= 1f)
            {
                currentScore++;
                checkTime = 0f;
            }
            if(currentScore == nextScore)
                break;

            swordPivot.localScale = new Vector3(1f, swordPivot.localScale.y + (scoreSize * Time.deltaTime * sizeUpSpeed), 1f);
            col.center = new Vector3(-0.2f, 0.5f + swordPivot.localScale.y, 0.45f);
            col.size = new Vector3(0.3f, 2.4f + swordPivot.localScale.y * 2, 0.6f);
            checkTime += Time.deltaTime * sizeUpSpeed;
            trail.widthMultiplier = (currentScore / 100f) + 1;
            playerAttack.SetAttackDelay(currentScore / 500f);

            yield return null;
        }
        isGrowing = false;
    }
}
