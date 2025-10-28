using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarQueueManager : MonoBehaviour
{
    [System.Serializable]
    public class CarSlot
    {
        public Transform slotPoint;   // Ponto de parada (A, B, C, etc.)
        public GameObject currentCar; // Carro que ocupa esse ponto
    }

    public List<CarSlot> slots = new List<CarSlot>();
    public Transform exitPoint;
    public float moveDuration = 2.5f;
    public float rotateSpeed = 8f;
    public float arcHeight = 5f;

    private Dictionary<GameObject, float> carHeights = new Dictionary<GameObject, float>();
    private bool isMoving = false;

    void RegisterCar(GameObject car)
    {
        if (car == null) return;
        if (!carHeights.ContainsKey(car))
            carHeights[car] = car.transform.position.y;
    }

    // Chamada pela tecla E (debug/teste)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ReleaseFirstCar();
    }

    public void ReleaseFirstCar()
    {
        if (slots.Count == 0 || slots[0].currentCar == null || isMoving) return;

        GameObject leavingCar = slots[0].currentCar;
        slots[0].currentCar = null;

        RegisterCar(leavingCar);
        StartCoroutine(MoveCarBezier(leavingCar, leavingCar.transform.position, exitPoint.position));

        for (int i = 0; i < slots.Count - 1; i++)
        {
            if (slots[i].currentCar == null && slots[i + 1].currentCar != null)
            {
                GameObject nextCar = slots[i + 1].currentCar;
                slots[i].currentCar = nextCar;
                slots[i + 1].currentCar = null;

                RegisterCar(nextCar);
                StartCoroutine(MoveCarBezier(nextCar, nextCar.transform.position, slots[i].slotPoint.position));
            }
        }
    }

    public void ReleaseSpecificCar(GameObject car)
    {
        if (car == null) return;

        int index = slots.FindIndex(s => s.currentCar == car);
        if (index == -1) return;

        StartCoroutine(MoveCarBezier(car, car.transform.position, exitPoint.position));
        slots[index].currentCar = null;

        for (int i = index; i < slots.Count - 1; i++)
        {
            if (slots[i].currentCar == null && slots[i + 1].currentCar != null)
            {
                GameObject nextCar = slots[i + 1].currentCar;
                slots[i].currentCar = nextCar;
                slots[i + 1].currentCar = null;
                StartCoroutine(MoveCarBezier(nextCar, nextCar.transform.position, slots[i].slotPoint.position));
            }
        }
    }

    IEnumerator MoveCarBezier(GameObject car, Vector3 start, Vector3 end)
    {
        if (car == null) yield break;
        isMoving = true;

        RegisterCar(car);
        float fixedY = carHeights.ContainsKey(car) ? carHeights[car] : car.transform.position.y;

        start.y = fixedY;
        end.y = fixedY;

        Vector3 mid = (start + end) * 0.5f;
        Vector3 dir = (end - start);
        dir.y = 0;
        Vector3 perp = (dir.sqrMagnitude < 0.0001f) ? Vector3.right : Vector3.Cross(Vector3.up, dir).normalized;

        Vector3 controlPoint = mid + perp * arcHeight;
        controlPoint.y = fixedY;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = Mathf.Clamp01(elapsed / moveDuration);
            Vector3 pos = Mathf.Pow(1 - t, 2) * start +
                          2f * (1 - t) * t * controlPoint +
                          Mathf.Pow(t, 2) * end;

            Vector3 tangent = 2f * (1 - t) * (controlPoint - start) + 2f * t * (end - controlPoint);
            tangent.y = 0f;

            if (tangent.sqrMagnitude < 0.0001f)
            {
                tangent = (end - start);
                tangent.y = 0f;
                if (tangent.sqrMagnitude < 0.0001f) tangent = perp;
            }

            Quaternion targetRot = Quaternion.LookRotation(tangent.normalized, Vector3.up);
            car.transform.rotation = Quaternion.Slerp(car.transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            car.transform.position = pos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Vector3 finalPos = end;
        finalPos.y = fixedY;
        car.transform.position = finalPos;

        if (exitPoint != null && Vector3.Distance(finalPos, exitPoint.position) < 0.5f)
            Destroy(car);

        isMoving = false;
    }
}
