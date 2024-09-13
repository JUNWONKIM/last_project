using UnityEngine;

public class UI_CameraButton : MonoBehaviour
{
    private Camera mainCamera; // ���� ī�޶� ������ ����
    private Vector3 initialPosition; // ī�޶��� �ʱ� ��ġ
    private Quaternion initialRotation; // ī�޶��� �ʱ� ȸ��

    void Start()
    {
        // ���� ī�޶� ������
        mainCamera = Camera.main;

        // �ʱ� ī�޶��� ��ġ�� ȸ���� ����
        initialPosition = mainCamera.transform.position;
        initialRotation = mainCamera.transform.rotation;
    }

    void Update()
    {
        // R Ű�� ������ ī�޶� �ʱ�ȭ �Լ� ȣ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCameraToInitialPosition();
        }

        // T Ű�� ������ �÷��̾�� ī�޶� �̵�
        if (Input.GetKeyDown(KeyCode.T))
        {
            MoveCameraToObjectWithTag("Player");
        }

        // Y Ű�� ������ ������ ī�޶� �̵�
        if (Input.GetKeyDown(KeyCode.Y))
        {
            MoveCameraToObjectWithTag("Boss");
        }
    }

    // ī�޶� �ʱ�ȭ�ϴ� �Լ�
    public void ResetCameraToInitialPosition()
    {
        // ī�޶��� ��ġ�� ȸ���� �ʱⰪ���� ����
        mainCamera.transform.position = initialPosition;
        mainCamera.transform.rotation = initialRotation;
    }

    // Ư�� �±׸� ���� ������Ʈ�� ī�޶� �̵� (ȸ���� ���̴� �������� ����)
    public void MoveCameraToObjectWithTag(string tag)
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag(tag);
        if (targetObject != null)
        {
            // ���� ī�޶��� ȸ���� ���� ������ ������
            Vector3 cameraPosition = mainCamera.transform.position;
            Quaternion cameraRotation = mainCamera.transform.rotation;

            // ī�޶��� ���̸� ����
            Vector3 targetPosition = targetObject.transform.position;
            targetPosition.y = cameraPosition.y;

            // ī�޶��� ���� ����� �������� ����Ͽ� ������Ʈ ��ġ ����
            Vector3 directionToTarget = targetPosition - cameraPosition;
            directionToTarget = Quaternion.Inverse(cameraRotation) * directionToTarget;

            // ���� ��� (Y = 100�� �� Z = -40)
            float baseY = 100f;
            float baseZ = -40f;

            // ���� Y ��ǥ�� ���� Z ��ǥ ���
            float currentY = cameraPosition.y;
            float adjustedZ = baseZ * (currentY / baseY);

            // ī�޶��� ��ġ�� ����
            Vector3 adjustedTargetPosition = cameraPosition + (cameraRotation * directionToTarget);
            adjustedTargetPosition.z = adjustedTargetPosition.z + adjustedZ; // Z ��ǥ�� ����

            // ī�޶��� ��ġ�� ������Ʈ
            mainCamera.transform.position = adjustedTargetPosition;

            // ī�޶��� ȸ���� �������� ����
            mainCamera.transform.rotation = cameraRotation;
        }
        else
        {
            Debug.LogWarning($"No object with tag '{tag}' found.");
        }
    }



}

