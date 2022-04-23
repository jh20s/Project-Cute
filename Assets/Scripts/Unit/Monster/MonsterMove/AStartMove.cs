using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) 
    {
        mIsWall = _isWall;
        mX = _x;
        mY = _y; 
    }
    // �ش� ��尡 ������ �ƴ��� �Ǵ�
    private bool mIsWall;
    public bool IsWall
    {
        get { return mIsWall; }
    }
    // �ش� ����� �θ���(��� ���κ��� �Դ���)
    private Node mParentNode;
    public Node ParentNode
    {
        get { return ParentNode; }
        set { mParentNode = value; }
    }

    // �ش� ����� x, y��ǥ
    private int mX;
    public int X
    {
        get { return mX; }
    }
    private int mY;
    public int Y
    {
        get { return mY; }
    }
    // G : �������κ��� �̵��ߴ� �Ÿ�
    // H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�
    // F : G + H
    private int mG;
    public int G
    {
        get { return mG; }
    }
    private int mH;
    public int H
    {
        get { return mH; }
    }
    private int mF;
    public int F
    { 
        get{ return mG + mH; }
    }
}

public class AStartMove : MonoBehaviour
{
    // startPos : ������ ��ġ
    // targetPos : �÷��̾��� ��ġ
    [SerializeField]
    private Vector2Int mStartPos, mTargetPos;
    // ���� ��ã�⸦ �Ϸ��� ��帮��Ʈ
    [SerializeField]
    private List<Node> mFinalNodeList;
    // �밢�� �̵�
    [SerializeField]
    private bool mAllowDaigonal;


    private int mSizeX, mSizeY;
    // startPos�� targetPos�� ���������� ������ �簢������ 
    [SerializeField]
    private Node[,] mCurrentNodeArray;
    // ���۳��, ������, ������
    [SerializeField]
    private Node mStartNode, mTargetNode, mCurrentNode;
    // 
    [SerializeField]
    private List<Node> mOpenList, mCloseList;

}
