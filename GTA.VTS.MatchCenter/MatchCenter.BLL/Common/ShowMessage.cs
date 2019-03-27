using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatchCenter.BLL.Common
{
    /// <summary>
    /// Title:显示信息功能类
    /// Desc.:显示信息功能类,包括把信息显示到ListBox中和把信息写入到文档中，或者记录异常信息
    /// Create By:李健华
    /// Create Date:2009-09-17
    /// </summary>
    public class ShowMessage : Singleton<ShowMessage>
    {
        #region 单一进入模式
        /// <summary>
        /// 费用价格计算功能类功能单一进入模式
        /// </summary>
        public static ShowMessage Instanse
        {
            get
            {
                return singletonInstance;
            }
        }
        #endregion

        #region 显示信息控件定义
        /// <summary>
        /// 显示撮合信息listbox
        /// </summary>
        public ListBox ListBoxShowMatchMessage
        {
            get;
            set;
        }
        /// <summary>
        /// 显示报盘信息listbox
        /// </summary>
        public ListBox ListBoxShowOfferMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 当前程序的主窗体
        /// </summary>
        public Form AppForm
        {
            get;
            set;
        }
        #endregion

        #region 统计报盘和撮合速度事件
        /// <summary>
        /// 统计撮合速度事件
        /// </summary>
        public event EventHandler<EventArgs> ProcessEvent;
        /// <summary>
        /// 统计报盘速度事件
        /// </summary>
        public event EventHandler<EventArgs> ProcessWorkEvent;
        #endregion

        #region 显示信息
        /// <summary>
        /// 把撮合的信息显示到指定的ListBox列表中
        /// </summary>
        /// <param name="mesg">要显示的信息</param>
        public void ShowMatchMessage(string mesg)
        {
            if (ProcessEvent != null)
            {
                ProcessEvent(this, null);
            }
            Utils.ShowMessageToUIEvent(mesg, ListBoxShowMatchMessage);
        }
        /// <summary>
        /// 把报盘的信息显示到指定的ListBox列表中
        /// </summary>
        /// <param name="mesg">要显示的信息</param>
        public void ShowOfferMessage(string mesg)
        {
            if (ProcessWorkEvent != null)
            {
                ProcessWorkEvent(this, null);
            }
            Utils.ShowMessageToUIEvent(mesg, ListBoxShowOfferMessage);
        }
        /// <summary>
        /// 把要显示的信息显示到指定的Form的Title中
        /// </summary>
        /// <param name="mesg">要显示的信息</param>
        public void ShowFormTitleMessage(string mesg)
        {
            Utils.ShowMessageToFormTitleEvent(mesg, AppForm);
        }

        #endregion
    }
    /// <summary>
    /// Title:信息记录变量实体类
    /// Desc.:主要生成异常信息变量和跟踪信息变量
    /// Create BY:李健华
    /// Create Date:2009-09-17
    /// Update By:董鹏
    /// Update Date:2009-12-18
    /// Desc.:修改了CH_D004，使得在其中可以加入货品类别
    /// Update By:董鹏
    /// Update Date:2010-01-27
    /// Desc.:增加了CH_D04，并把CH_D02改为了股指期货
    /// </summary>
    public static class GenerateInfo
    {
        #region 异常信息
        /// <summary>
        /// 异常信息CH_E001:WCF服务通道阻塞！
        /// </summary>
        public static string CH_E001 = "CH_E001:WCF服务通道阻塞！";
        /// <summary>
        /// CH_E002:成交回报WCF服务通道阻塞！
        /// </summary>
        public static string CH_E002 = "CH_E002:成交回报WCF服务通道阻塞！";
        /// <summary>
        /// CH_E003:数据库操作异常
        /// </summary>
        public static string CH_E003 = "CH_E003:数据库操作异常!";
        /// <summary>
        /// CH_E004:委托回报分发异常
        /// </summary>
        public static string CH_E004 = "CH_E004:[委托回报分发异常]";
        /// <summary>
        /// CH_E005:向指定列表控件{0}显示信息异常,信息为：{1}
        /// </summary>
        public static string CH_E005 = "CH_E005:向指定列表控件{0}显示信息异常,信息为：{1}";
        /// <summary>
        /// CH_E006:从管理中心获取数据受阻
        /// </summary>
        public static string CH_E006 = "CH_E006:从管理中心获取数据受阻";
        /// <summary>
        /// 接收委托下单时无法获取行情
        /// </summary>
        public static string CH_E007 = "接收委托下单时无法获取行情!";
        /// <summary>
        ///  根据商品代码{0}获取所属交易所接收交易时间异常为null
        /// </summary>
        public static string CH_E008 = "根据商品代码{0}获取所属交易所接收交易时间异常为null";
        #endregion

        #region 记录跟踪信息
        /// <summary>
        /// 现货标识--现货
        /// </summary>
        public static string CH_D01 = "现货";
        /// <summary>
        /// 期货标识--股指期货
        /// </summary>
        public static string CH_D02 = "股指期货";
        /// <summary>
        /// 港股标识--港股
        /// </summary>
        public static string CH_D03 = "港股";

        /// <summary>
        /// Desc: 期货标识--商品期货
        /// Create By: 董鹏
        /// Create Date: 2010-01-27
        /// </summary>
        public static string CH_D04 = "商品期货";

        /// <summary>
        /// 接收到的委托信息--CH_D001:{0}--单号:{1} 代码:{2} 时间:{3} 数量:{4} 类型:{5}
        /// </summary>
        public static string CH_D001 = "CH_D001:{0}委托--单号:{1} 代码:{2} 时间:{3} 数量:{4} 类型:{5}";
        /// <summary>
        /// 成交记录信息--CH_D002:{0}成交--单号:{1} 代码:{2} 时间:{3} 数量:{4} 类型:{5} 价格:{6}
        /// </summary>
        public static string CH_D002 = "CH_D002:{0}成交--单号:{1} 代码:{2} 时间:{3} 数量:{4} 类型:{5} 价格:{6}";
        /// <summary>
        /// CH_D003:{0}不在交易时间之内
        /// </summary>
        public static string CH_D003 = "CH_D003:{0}不在交易时间之内";
        /// <summary>
        /// 说明：进入撮合机下单方法
        /// </summary>
        public static string CH_D004 = "CH_D004:【进入{0}撮合机下单方法】";
        /// <summary>
        /// 接收撤单委托单成功信息--CH_D005:接收撤单委托单成功--单号:{0} 代码:{1} 时间:{2}
        /// </summary>
        public static string CH_D005 = "CH_D005:接收撤单委托单成功--单号:{0} 代码:{1} 时间:{2}";
        /// <summary>
        /// 撤单接收成功--CH_D006:撤单接收成功
        /// </summary>
        public static string CH_D006 = "CH_D006:撤单接收成功";
        /// <summary>
        /// CH-0007:委托单价格不在交易价格范围之内,价格为:{0}
        /// </summary>
        public static string CH_D007 = "CH-0007:委托单价格不在交易价格范围之内,价格为:{0}";
        /// <summary>
        /// 提供对服务方法的执行上下文对象为空!
        /// </summary>
        public static string CH_D008 = "CH_D008:提供对服务方法的执行上下文对象为空!";
        /// <summary>
        /// 撤单成功信息CH_D009:{0}撤单成功[委托id={1},委托代码={2},撤单时间={3},委托单号码={4},撤单数量={5}]
        /// </summary>
        public static string CH_D009 = "CH_D009:{0}撤单成功[委托ID={1},委托代码={2},撤单时间={3},委托单号码={4},撤单数量={5}]";
        /// <summary>
        /// CH_D010:市价委托无法获取到行情!
        /// </summary>
        public static string CH_D010 = "CH_D010:市价委托无法获取到行情!";
        /// <summary>
        /// 改接收成功--CH_D011:改单接收成功
        /// </summary>
        public static string CH_D011 = "CH_D011:改单接收成功";
        /// <summary>
        /// 接收改单委托单成功信息--CH_D012:接收撤单委托单成功--单号:{0} 代码:{1} 时间:{2}
        /// </summary>
        public static string CH_D012 = "CH_D012:接收改单委托单成功--单号:{0} 代码:{1} 时间:{2}";
        #endregion




    }
}
