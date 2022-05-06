using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRayCastManager : SingleToneMaker<CustomRayCastManager>
{
    //��,��,��,��,�»�,���,����,����
    private Vector3[] mVectorDir = new Vector3[8] { Vector3.up, Vector3.down, Vector3.left, Vector3.right,
            Vector3.up+ Vector3.left, Vector3.up+ Vector3.right, Vector3.down+ Vector3.left, Vector3.down+ Vector3.right};

    //target : target�� ��ġ
    //_x,_y : target��ǥ�κ��� ������ ray�� ��ġ
    //_target: ����ȭ�ȰŸ����� ���̸� ������ ��ġ 
    //__rayDist : ����� ray�� ����
    //_drawRay : ����׿����� ray�� ���������� ����
    //rayPos ray�� ���� ������ ���� ��ǥ���� ������ ��� ����
    //target��ġ���� x,y������ ��ġ�� Ÿ���� ���ϴܿ��� _tatgetDist��ŭ ������ ���̿��� _rayDist��ŭ ���̸� �� �̵����� ������ return
    //��ֹ��� �־� �̵��Ұ����̸� false
    //��ֹ��� ���� �̵������̸� true
    public bool NomarlizeMoveableWithRay(Vector3 _target, int _x, int _y, float _targetDist, float _rayDist, bool _drawRay, ref Vector3 _rayPos)
    {
        //���̸� ��� ���� Ÿ���� ��ġ�� ����
        Vector3 targetPosition = _target;
        targetPosition.x += _x;
        targetPosition.y += _y;
        float distance = Vector3.Distance(targetPosition, _target);
        Vector3 dir = (targetPosition - _target).normalized * distance;
        Vector2 rayTargetPosition = new Vector2(_target.x + dir.x, _target.y + dir.y);

        //rayTargetPosition�� ��ġ�� tile�� �Ѱ���� ��ġ������
        rayTargetPosition = Vector2Int.FloorToInt(rayTargetPosition);
        rayTargetPosition.x += _targetDist;
        rayTargetPosition.y += _targetDist;
        _rayPos = rayTargetPosition;

        for (int i = 0; i < 8; i++)
        {
            RaycastHit2D ray = Physics2D.Raycast(rayTargetPosition, mVectorDir[i], _rayDist, LayerMask.GetMask("Tilemap"));
            if (ray.collider != null)
            {
                if (_drawRay)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Vector3 debugVector = mVectorDir[j] * _rayDist;
                        Debug.DrawRay(new Vector2(rayTargetPosition.x, rayTargetPosition.y), debugVector, Color.red);
                    }
                }
                return false;
            }
        }
        return true;

    }
}
