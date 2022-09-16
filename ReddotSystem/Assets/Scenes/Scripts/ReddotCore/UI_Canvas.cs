using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reddot
{
    /*
     * 驱动层： 注册监听红点、红点触发，并通知表现层
     */

    /// <summary>
    /// 业务UI
    /// </summary>
    public class UI_Canvas : MonoBehaviour
    {
        /// <summary>
        /// 福利红点
        /// </summary>
        public ReddotItem m_WelfareReddot;

        /// <summary>
        /// 福利->累计充值 红点
        /// </summary>
        public ReddotItem m_WelfareAccumRecharge;

        /// <summary>
        /// 福利->七日目标 红点
        /// </summary>
        public ReddotItem m_WelfareSevendayTarget;
        
        // Start is called before the first frame update
        void Start()
        {
            // 注册红点（最好放在UI OnInit中）
            ReddotManagerComponent.ReddotManager.SetReddotNodeCallback(ReddotDefine.Welfare, WelfareCallback);
            ReddotManagerComponent.ReddotManager.SetReddotNodeCallback(ReddotDefine.Welfare_AccumRecharge, Welfare_AccumRechargeCallback);
            ReddotManagerComponent.ReddotManager.SetReddotNodeCallback(ReddotDefine.Welfare_SevendayTarget, Welfare_SevendayTargetCallback);
            
            // 初始化显示红点信息
            ReddotManagerComponent.ReddotManager.Set(ReddotDefine.Welfare_AccumRecharge, 2);
            ReddotManagerComponent.ReddotManager.Set(ReddotDefine.Welfare_SevendayTarget, 1);
        }

        private void OnDestroy()
        {
            // 取消注册（最好放在UI OnClose中）
            ReddotManagerComponent.ReddotManager.SetReddotNodeCallback(ReddotDefine.Welfare, null);
            ReddotManagerComponent.ReddotManager.SetReddotNodeCallback(ReddotDefine.Welfare_AccumRecharge, null);
            ReddotManagerComponent.ReddotManager.SetReddotNodeCallback(ReddotDefine.Welfare_SevendayTarget, null);
        }

        #region 各个红点节点的回调函数

        void WelfareCallback(ReddotNode node)
        {
            this.m_WelfareReddot.SetReddotState(node.rdCount > 0, node.rdCount);
        }

        void Welfare_AccumRechargeCallback(ReddotNode node)
        {
            this.m_WelfareAccumRecharge.SetReddotState(node.rdCount > 0, node.rdCount);
        }

        void Welfare_SevendayTargetCallback(ReddotNode node)
        {
            this.m_WelfareSevendayTarget.SetReddotState(node.rdCount > 0, node.rdCount);
        }

        #endregion

        #region GM工具按钮点击事件

        public void OnAddRdAccumRechargeBtnClick()
        {
            int count =ReddotManagerComponent.ReddotManager.GetReddotCount(ReddotDefine.Welfare_AccumRecharge);
            ReddotManagerComponent.ReddotManager.Set(ReddotDefine.Welfare_AccumRecharge, count + 1);
        }
        public void OnAddRdSevendayTaragetBtnClick()
        {
            int count =ReddotManagerComponent.ReddotManager.GetReddotCount(ReddotDefine.Welfare_SevendayTarget);
            ReddotManagerComponent.ReddotManager.Set(ReddotDefine.Welfare_SevendayTarget, count + 1);
        }
        public void OnReduceRdAccumRechargeBtnClick()
        {
            int count =ReddotManagerComponent.ReddotManager.GetReddotCount(ReddotDefine.Welfare_AccumRecharge);
            ReddotManagerComponent.ReddotManager.Set(ReddotDefine.Welfare_AccumRecharge, count - 1);
        }
        public void OnReduceRdSevendayTaragetBtnClick()
        {
            int count =ReddotManagerComponent.ReddotManager.GetReddotCount(ReddotDefine.Welfare_SevendayTarget);
            ReddotManagerComponent.ReddotManager.Set(ReddotDefine.Welfare_SevendayTarget, count - 1);
        }

        #endregion
    }
}

