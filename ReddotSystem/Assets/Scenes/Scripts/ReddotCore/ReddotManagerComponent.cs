using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reddot
{
    /// <summary>
    /// 红点管理组件
    /// </summary>
    public static class ReddotManagerComponent
    {
        private static ReddotSystem m_ReddotManager;

        public static ReddotSystem ReddotManager
        {
            get
            {
                // 应该放在项目的初始化逻辑中，此处为demo临时写法
                if (m_ReddotManager == null)
                {
                    m_ReddotManager = new ReddotSystem();
                }

                return m_ReddotManager;
            }
        }
    }
}
