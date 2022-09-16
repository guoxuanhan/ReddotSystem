using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reddot
{
    public static class ReddotDefine
    {
        /// <summary>
        /// 红点树的根节点：全局有且仅有一个
        /// </summary>
        public const string Root = "Root";
        
        // -------------------- 业务红点 --------------------
        // 福利活动
        public const string Welfare = "Root/Welfare";
        // 福利活动下的"累计充值"活动
        public const string Welfare_AccumRecharge = "Root/Welfare/AccumRecharge";
        // 福利活动下的"七日目标"活动
        public const string Welfare_SevendayTarget = "Root/Welfare/SevendayTarget";
    }

    /// <summary>
    /// 红点系统
    /// </summary>
    public class ReddotSystem
    {
        /// <summary>
        /// 红点树变化通知委托
        /// </summary>
        public delegate void OnRdCountChange(ReddotNode node);

        /// <summary>
        /// 红点数的Root根节点
        /// </summary>
        private ReddotNode m_RootNode;

        /// <summary>
        /// 红点树路径表（每次定义ReddotDefine后，这里也需要添加）
        /// </summary>
        private static List<string> lstReddotTree = new List<string>
        {
            ReddotDefine.Root,
            
            ReddotDefine.Welfare,
            ReddotDefine.Welfare_AccumRecharge,
            ReddotDefine.Welfare_SevendayTarget,
        };

        #region 内部接口
        
        /// <summary>
        /// 初始化红点树
        /// </summary>
        private void InitReddotTree()
        {
            //结构层： 根据红点是否显示或显示数，自定义红点的表现方式
            this.m_RootNode = new ReddotNode {rdName = ReddotDefine.Root};

            foreach (string path in lstReddotTree)
            {
                string[] treeNodeArray = path.Split('/');
                int nodeCount = treeNodeArray.Length;
                ReddotNode curNode = this.m_RootNode;

                if (treeNodeArray[0] != this.m_RootNode.rdName)
                {
                    Debug.LogError("根节点必须为Root, 请检查：" + treeNodeArray[0]);
                    continue;
                }

                if (nodeCount > 1)
                {
                    for (int i = 1; i < nodeCount; i++)
                    {
                        if (!curNode.dicRdChildren.ContainsKey(treeNodeArray[i]))
                        {
                            curNode.dicRdChildren.Add(treeNodeArray[i], new ReddotNode());
                        }

                        curNode.dicRdChildren[treeNodeArray[i]].rdName = treeNodeArray[i];
                        curNode.dicRdChildren[treeNodeArray[i]].parent = curNode;

                        curNode = curNode.dicRdChildren[treeNodeArray[i]];
                    }
                }
            }
        }

        #endregion

        #region 外部接口

        public ReddotSystem()
        {
            InitReddotTree();
            Debug.Log("红点系统构造完毕!");
        }

        /// <summary>
        /// 设置红点树变化的回调
        /// </summary>
        /// <param name="strNode">红点路径：必须是ReddotDefine</param>
        /// <param name="callback"></param>
        public void SetReddotNodeCallback(string strNode, OnRdCountChange callback)
        {
            var nodeList = strNode.Split('/');
            if (nodeList.Length == 1)
            {
                if (nodeList[0] != ReddotDefine.Root)
                {
                    Debug.LogError("Get Wrong Root Node! current is " + nodeList[0]);
                    return;
                }
            }

            var node = this.m_RootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                if (!node.dicRdChildren.ContainsKey(nodeList[i]))
                {
                    Debug.LogError("Does Not Contain child Node: " + nodeList[i]);
                    return;
                }

                node = node.dicRdChildren[nodeList[i]];

                // 找到叶节点，设置回调
                if (i == nodeList.Length - 1)
                {
                    node.onCountChangeCallback = callback;
                    return;
                }
            }
        }

        public void Set(string nodePath, int rdCount = 1)
        {
            string[] nodeList = nodePath.Split('/');
            if (nodeList.Length == 1)
            {
                if (nodeList[0] != ReddotDefine.Root)
                {
                    Debug.Log("Get Wrong RootNod！ current is " + nodeList[0]);
                    return;
                }
            }
            
            // 遍历子红点
            ReddotNode node = this.m_RootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                // 父红点的子红点集合中，必须要包含当前节点
                if (node.dicRdChildren.ContainsKey(nodeList[i]))
                {
                    node = node.dicRdChildren[nodeList[i]];
                    
                    // 设置叶红点的红点树
                    if (i == nodeList.Length - 1)
                    {
                        node.SetReddotCount(Math.Max(0, rdCount));
                    }
                }
                else
                {
                    Debug.LogError($"{node.rdName}的子红点字典内无 Key={nodeList[i]}, 检查 RedDotSystem.InitReddotTree()");
                    return;
                }
            }
        }

        /// <summary>
        /// 获取红点计数
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public int GetReddotCount(string nodePath)
        {
            string[] nodeList = nodePath.Split('/');

            int count = 0;
            if (nodeList.Length >= 1)
            {
                // 遍历子红点
                ReddotNode node = this.m_RootNode;
                for (int i = 1; i < nodeList.Length; i++)
                {
                    // 父红点的子红点集合中，必须有当前节点
                    if (node.dicRdChildren.ContainsKey(nodeList[i]))
                    {
                        node = node.dicRdChildren[nodeList[i]];
                        if (i == nodeList.Length - 1)
                        {
                            count = node.rdCount;
                            break;
                        }
                    }
                }
            }

            return count;
        }

        #endregion
    }
}
