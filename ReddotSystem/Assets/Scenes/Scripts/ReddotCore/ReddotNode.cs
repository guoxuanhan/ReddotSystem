using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reddot
{
    /// <summary>
    /// 红点的数据节点
    /// </summary>
    public class ReddotNode
    {
        /// <summary>
        /// 红点名称
        /// </summary>
        public string rdName { get; set; }
        
        /// <summary>
        /// 红点计数
        /// </summary>
        public int rdCount { get; private set; }

        /// <summary>
        /// 该红点节点的父节点
        /// </summary>
        public ReddotNode parent;

        /// <summary>
        /// 该红点节点发生变化的回调函数
        /// </summary>
        public ReddotSystem.OnRdCountChange onCountChangeCallback;

        /// <summary>
        /// 存放子红点节点
        /// </summary>
        public Dictionary<string, ReddotNode> dicRdChildren = new Dictionary<string, ReddotNode>();

        #region 内部接口

        /// <summary>
        /// 重新计算红点计数
        /// </summary>
        private void CheckReddotCount()
        {
            // 红点的计数： 子红点的计数和
            int num = 0;
            foreach (var node in this.dicRdChildren.Values)
            {
                num += node.rdCount;
            }

            // 通知红点计数有变化
            if (num != this.rdCount)
            {
                this.rdCount = num;
                NotifyReddotCountChange();
            }
            
            parent?.CheckReddotCount();
        }

        /// <summary>
        /// 红点计数变化的通知
        /// </summary>
        private void NotifyReddotCountChange()
        {
            this.onCountChangeCallback?.Invoke(this);
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 设置红点的数量
        /// </summary>
        /// <param name="rdCount"></param>
        public void SetReddotCount(int rdCount)
        {
            // 只能对非根节点进行操作
            if (this.dicRdChildren.Count > 0)
            {
                Debug.LogWarning("不可直接设定根节点的红点数!");
                return;
            }

            this.rdCount = rdCount;
            
            NotifyReddotCountChange();
            parent?.CheckReddotCount();
        }

        #endregion
    }
}
