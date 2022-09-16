using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reddot
{
    /*
     * 表现层：根据红点是否显示或显示数量，自定义红点的表现方式 
     */
    
    /// <summary>
    /// UGUI红点物体脚本
    /// </summary>
    public class ReddotItem : MonoBehaviour
    {
        [Header("红点父节点")] [SerializeField] public GameObject m_ReddotObj;

        [Header("红点数文本")] [SerializeField] private Text m_ReddotCountText;

        /// <summary>
        /// 设置红点状态
        /// </summary>
        /// <param name="isShow"></param>
        /// <param name="reddotCount"></param>
        public void SetReddotState(bool isShow, int reddotCount = -1)
        {
            if (isShow)
            {
                this.m_ReddotObj.gameObject.SetActive(true);
                if (this.m_ReddotCountText)
                {
                    this.m_ReddotCountText.text = reddotCount >= 0 ? reddotCount.ToString() : "";
                }
            }
            else
            {
                this.m_ReddotObj.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            this.m_ReddotObj = null;
            this.m_ReddotCountText = null;
        }
    }
}

