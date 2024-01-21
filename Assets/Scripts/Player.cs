
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Controller")]
    public CharacterController characterController;

    [Header("Camera")]
    public Transform cameraTarget;

    [SerializeField]
    [Header("Inventory")]
    private int gold;


    [Header("Events")]
    public UnityEvent<int> OnAddGold = new UnityEvent<int>();
    
    #region Inventory

    public void AddGold(int count = 1)
    {
        gold += count;
        OnAddGold.Invoke(gold);
        
    }

    #endregion




    private void OnTriggerEnter(Collider other)
    {
        PlayerTriggerable triggerable = other.GetComponentInParent<PlayerTriggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.OnPlayerTriggerEnter(this, other))
        {
            //Debug.Log("Bir Hata Oldu");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerTriggerable triggerable = other.GetComponentInParent<PlayerTriggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.OnPlayerTriggerExit(this, other))
        {
            //Debug.Log("Bir Hata Oldu");
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        OnAddGold.AddListener((_gold) => PlayerPrefs.SetInt("PlayerGold", _gold));
    }


    void Start()
    {
        gold = PlayerPrefs.GetInt("PlayerGold");
        OnAddGold.Invoke(gold);
    }

    void Update()
    {

    }

    public void SetPos(Vector3 newPos)
    {
        characterController.enabled = false;
        transform.position = newPos;
        characterController.enabled = true;
    }


}
