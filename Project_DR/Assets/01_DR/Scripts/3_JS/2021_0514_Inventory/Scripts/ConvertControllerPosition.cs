using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertControllerPosition
{
    float mousePosX = 1018.67f;
    float mousePosY = -217f;

    float controllerPosX = -2.3f;
    float controllerPosY = 0.36f;

    // 마우스와 컨트롤러의 x축 및 y축 범위를 각각 정의합니다.
    float mouseMinX = 0f;
    float mouseMaxX = 1080f; // 예시에서는 가로 해상도를 1920으로 가정합니다.
    float mouseMinY = 0f;
    float mouseMaxY = 550f; // 예시에서는 세로 해상도를 1080으로 가정합니다.

    float controllerMinX = -5f; // 컨트롤러의 x축 최소값
    float controllerMaxX = 5f; // 컨트롤러의 x축 최대값
    float controllerMinY = -1f; // 컨트롤러의 y축 최소값
    float controllerMaxY = 1f; // 컨트롤러의 y축 최대값

    public Vector2 ConvertPosition(Vector2 pos)
    {
        // 마우스 포지션을 컨트롤러의 범위에 맞게 보정합니다.
        float convertedPosX = Mathf.Lerp(controllerMinX, controllerMaxX, Mathf.InverseLerp(mouseMinX, mouseMaxX, mousePosX));
        float convertedPosY = Mathf.Lerp(controllerMinY, controllerMaxY, Mathf.InverseLerp(mouseMinY, mouseMaxY, mousePosY));
        Vector2 convertedPos = new Vector2(convertedPosX, convertedPosY);

        return convertedPos;
    }
}
