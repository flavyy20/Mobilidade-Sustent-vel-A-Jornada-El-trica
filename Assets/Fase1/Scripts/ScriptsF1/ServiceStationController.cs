using UnityEngine;

public class ServiceStationController : MonoBehaviour
{
    public static ServiceStationController Instance;

    [Header("Referências")]
    public CarQueueManager queueManager;
    public Transform player;

    [Header("Configurações")]
    public float interactionRange = 3f;

    [HideInInspector] public int selectedType = 0;
    [HideInInspector] public int selectedRecharge = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectType(int type)
    {
        selectedType = type;
        Debug.Log($"Tipo selecionado: {type}");
    }

    public void SelectRecharge(int recharge)
    {
        selectedRecharge = recharge;
        Debug.Log($"Recarga selecionada: {recharge}");
    }

    private void Update()
    {
        CheckNearbyCars();
    }

    void CheckNearbyCars()
    {
        foreach (var slot in queueManager.slots)
        {
            if (slot.currentCar == null) continue;

            GameObject car = slot.currentCar;
            float distance = Vector3.Distance(car.transform.position, player.position);

            if (distance <= interactionRange)
            {
                VehicleInteraction vehicle = car.GetComponent<VehicleInteraction>();

                if (vehicle != null)
                {
                    // Tipo A: Tipo1 + Recarga1
                    if (selectedType == 1 && selectedRecharge == 1 && vehicle.tipo == VehicleInteraction.TipoVeiculo.A)
                    {
                        Debug.Log("Carro tipo A atendido!");
                        queueManager.ReleaseSpecificCar(car);
                        ProgressBarController.Instance.IncrementProgress();
                        ResetSelections();
                        return;
                    }

                    // Tipo B: apenas Recarga2
                    else if (selectedRecharge == 2 && vehicle.tipo == VehicleInteraction.TipoVeiculo.B)
                    {
                        Debug.Log("Carro tipo B atendido!");
                        queueManager.ReleaseSpecificCar(car);
                        ProgressBarController.Instance.IncrementProgress();
                        ResetSelections();
                        return;
                    }

                    // Tipo C: Tipo2 + Recarga3
                    else if (selectedType == 2 && selectedRecharge == 3 && vehicle.tipo == VehicleInteraction.TipoVeiculo.C)
                    {
                        Debug.Log("Carro tipo C atendido!");
                        queueManager.ReleaseSpecificCar(car);
                        ProgressBarController.Instance.IncrementProgress();
                        ResetSelections();
                        return;
                    }

                    else
                    {
                        Debug.Log("Combinação incorreta: atendimento não realizado.");
                    }
                }
            }
        }
    }

    void ResetSelections()
    {
        selectedType = 0;
        selectedRecharge = 0;
    }
}
