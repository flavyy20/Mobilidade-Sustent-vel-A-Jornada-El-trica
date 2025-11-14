using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarQueueManager : MonoBehaviour
{
    [System.Serializable]
    public class CarSlot
    {
        public Transform slotPoint;
        public GameObject currentCar;
    }

    [Header("Fila")]
    public List<CarSlot> slots = new List<CarSlot>();

    [Header("Prefabs (A, B, C)")]
    public GameObject prefabCarA;
    public GameObject prefabCarB;
    public GameObject prefabCarC;

    [Header("Ponto de Saída")]
    public Transform exitPoint;

    [Header("Movimento")]
    public float moveDuration = 2.5f;
    public float rotateSpeed = 8f;
    public float arcHeight = 5f;

    private Dictionary<GameObject, float> carHeights = new Dictionary<GameObject, float>();
    private bool isMoving = false;

    void Start()
    {
        // Inicializa preenchendo a fila com carros totalmente aleatórios
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].slotPoint != null)
            {
                slots[i].currentCar = SpawnRandomCarAt(slots[i].slotPoint.position);
            }
        }
    }

    GameObject SpawnRandomCarAt(Vector3 pos)
    {
        int r = Random.Range(0, 3);
        GameObject prefab =
            (r == 0) ? prefabCarA :
            (r == 1) ? prefabCarB :
                       prefabCarC;

        GameObject car = Instantiate(prefab, pos, Quaternion.identity);

        RegisterCar(car);

        return car;
    }

    void RegisterCar(GameObject car)
    {
        if (!carHeights.ContainsKey(car))
            carHeights[car] = car.transform.position.y;
    }

    private void Update()
    {
        // tecla E ainda funciona pra debug
        if (Input.GetKeyDown(KeyCode.E))
            ReleaseFirstCar();
    }

    public void ReleaseFirstCar()
    {
        if (slots[0].currentCar == null || isMoving) return;

        GameObject leavingCar = slots[0].currentCar;
        slots[0].currentCar = null;

        StartCoroutine(MoveCarBezier(leavingCar, leavingCar.transform.position, exitPoint.position));

        MoveQueueForward();
        FillLastSlot();
    }

    public void ReleaseSpecificCar(GameObject car)
    {
        int index = slots.FindIndex(s => s.currentCar == car);
        if (index < 0) return;

        slots[index].currentCar = null;

        StartCoroutine(MoveCarBezier(car, car.transform.position, exitPoint.position));

        MoveQueueForwardFromIndex(index);
        FillLastSlot();
    }

    void MoveQueueForward()
    {
        for (int i = 0; i < slots.Count - 1; i++)
        {
            if (slots[i].currentCar == null && slots[i + 1].currentCar != null)
            {
                GameObject next = slots[i + 1].currentCar;
                slots[i].currentCar = next;
                slots[i + 1].currentCar = null;

                StartCoroutine(MoveCarBezier(next, next.transform.position, slots[i].slotPoint.position));
            }
        }
    }

    void MoveQueueForwardFromIndex(int index)
    {
        for (int i = index; i < slots.Count - 1; i++)
        {
            if (slots[i].currentCar == null && slots[i + 1].currentCar != null)
            {
                GameObject next = slots[i + 1].currentCar;
                slots[i].currentCar = next;
                slots[i + 1].currentCar = null;

                StartCoroutine(MoveCarBezier(next, next.transform.position, slots[i].slotPoint.position));
            }
        }
    }

    void FillLastSlot()
    {
        // se a fase já completou 10 progressos, não cria mais carros
        if (ProgressBarController.Instance.progressSlider.value >= 10)
            return;

        int last = slots.Count - 1;
        if (slots[last].currentCar == null)
        {
            slots[last].currentCar = SpawnRandomCarAt(slots[last].slotPoint.position);
        }
    }

    IEnumerator MoveCarBezier(GameObject car, Vector3 start, Vector3 end)
    {
        if (car == null) yield break;

        isMoving = true;

        float fixedY = carHeights.ContainsKey(car) ? carHeights[car] : start.y;
        start.y = fixedY;
        end.y = fixedY;

        Vector3 mid = (start + end) * 0.5f;
        Vector3 dir = end - start;
        dir.y = 0;
        Vector3 perp = (dir.sqrMagnitude < 0.01f) ? Vector3.right : Vector3.Cross(Vector3.up, dir).normalized;

        Vector3 control = mid + perp * arcHeight;
        control.y = fixedY;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            t = Mathf.Clamp01(t);

            Vector3 pos = (1 - t) * (1 - t) * start +
                           2 * (1 - t) * t * control +
                           t * t * end;

            Vector3 tangent =
                2 * (1 - t) * (control - start) +
                2 * t * (end - control);

            if (tangent.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(tangent.normalized, Vector3.up);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            }

            car.transform.position = pos;
            yield return null;
        }

        if (Vector3.Distance(car.transform.position, exitPoint.position) < 0.5f)
            Destroy(car);

        isMoving = false;
    }
}
