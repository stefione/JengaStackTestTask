using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] Transform _Container;
    [SerializeField] TextMeshPro _GradeText;
    private List<JengaBlock> _blocks;
    public void BuildStack(List<StacksData> stack,string gradeName)
    {
        _blocks = new List<JengaBlock>();
        _GradeText.text = gradeName;
        bool rotate = true;
        float blockHeight = GameDataHolder.Instance.BlocksPrefabs[0].transform.localScale.y;
        float blockWidth = GameDataHolder.Instance.BlocksPrefabs[0].transform.localScale.x;
        float height = blockHeight / 2f;
        int index = 0;
        while (index < stack.Count)
        {
            for (int i = -1; i <= 1 && index < stack.Count; i++)
            {
                int angle = rotate ? 90 : 0;
                Quaternion rotation = Quaternion.Euler(0, angle, 0);
                Vector3 moveDirection = rotation * Vector3.right;
                Vector3 pos = i * moveDirection * blockWidth + Vector3.up * height;
                JengaBlock blockPrefab = GameDataHolder.Instance.BlocksPrefabs.Find(x => x.MasteryType == stack[index].mastery);
                JengaBlock jengaBlock = Instantiate(blockPrefab, _Container);
                jengaBlock.transform.SetLocalPositionAndRotation(pos, rotation);
                jengaBlock.AssignData(stack[index]);
                jengaBlock.SetPhysics(false);
                _blocks.Add(jengaBlock);
                index++;
            }
            rotate = !rotate;
            height += blockHeight;
        }
    }

    public void TestStack()
    {
        List<JengaBlock> remainingBlocks = new List<JengaBlock>();
        for(int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].SetPhysics(true);
            if (_blocks[i].Data.mastery == 0)
            {
                Destroy(_blocks[i].gameObject);
            }
            else
            {
                remainingBlocks.Add(_blocks[i]);
            }
        }
        _blocks = remainingBlocks;
    }
}
