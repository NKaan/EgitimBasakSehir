
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
        Debug.Log("Karaktere eklenen gold miktarý : " + count);
    }

    #endregion




    private void OnTriggerEnter(Collider other)
    {
        Triggerable triggerable = other.GetComponentInParent<Triggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.IsTrigger(this))
        {
            //Debug.Log("Bir Hata Oldu");
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
    }

    void Start()
    {
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
