using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Chk()
    {
        // ���� GameObject�� ������ Camera ������Ʈ�� �������� �ڵ�
        Camera cam = GetComponent<Camera>();

        // ���� ī�޶��� ����Ʈ ������ �������� �ڵ�
        Rect viewportRect = cam.rect;

        // ���ϴ� ���� ���� ������ ����ϴ� �ڵ�
        float screenAspectRatio = (float)Screen.width / Screen.height;
        float targetAspectRatio = 16f / 9f; // ���ϴ� ���� ���� ���� (��: 16:9)

        // ȭ�� ���� ���� ������ ���� ����Ʈ ������ �����ϴ� �ڵ�
        if (screenAspectRatio == targetAspectRatio) return;
        if (screenAspectRatio < targetAspectRatio)
        {
            // ȭ���� �� '����'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�
            viewportRect.height = screenAspectRatio / targetAspectRatio;
            viewportRect.y = (1f - viewportRect.height) / 2f;
        }
        else
        {
            // ȭ���� �� '�д�'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�.
            viewportRect.width = targetAspectRatio / screenAspectRatio;
            viewportRect.x = (1f - viewportRect.width) / 2f;
        }

        // ������ ����Ʈ ������ ī�޶� �����ϴ� �ڵ�
        cam.rect = viewportRect;
    }
    private void FixedUpdate()
    {
        Invoke("Chk", 1f);
    }
}
