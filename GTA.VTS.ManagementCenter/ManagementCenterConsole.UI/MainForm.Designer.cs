using System.Windows.Forms;
namespace ManagementCenterConsole.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            Application.Exit();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.Menutool_TransactionM = new DevExpress.XtraBars.BarButtonItem();
            this.Menutool_ManagerM = new DevExpress.XtraBars.BarButtonItem();
            this.Menutool_RightM = new DevExpress.XtraBars.BarButtonItem();
            this.Menutool_MatchM = new DevExpress.XtraBars.BarButtonItem();
            this.Menutool_CounterM = new DevExpress.XtraBars.BarButtonItem();
            this.Menutool_BreedClassM = new DevExpress.XtraBars.BarButtonItem();
            this.Menutool_CommodityM = new DevExpress.XtraBars.BarButtonItem();
            this.Menutool_BourseM = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.AccountM = new DevExpress.XtraBars.BarSubItem();
            this.AccountM_TransactionM = new DevExpress.XtraBars.BarButtonItem();
            this.AccountM_ManagerM = new DevExpress.XtraBars.BarButtonItem();
            this.AccountM_RightM = new DevExpress.XtraBars.BarButtonItem();
            this.AccountM_Exits = new DevExpress.XtraBars.BarButtonItem();
            this.MatchM = new DevExpress.XtraBars.BarSubItem();
            this.MatchM_CenterM = new DevExpress.XtraBars.BarButtonItem();
            this.MatchM_GuideM = new DevExpress.XtraBars.BarButtonItem();
            this.CounterM = new DevExpress.XtraBars.BarSubItem();
            this.CounterM_ConfigM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuCommParaM = new DevExpress.XtraBars.BarSubItem();
            this.MenuCommParaM_BreedClassM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuCommParaM_CommodityM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuCommParaM_BourseM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_TodaySettlementPriceM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuSpotM = new DevExpress.XtraBars.BarSubItem();
            this.MenuSpotM_SpotTradeRulesM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuSpotM_MinVolumeOfBusM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuSpotM_SpotCostsM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuSpotM_UnitConversionM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuSpotM_SpotPositionM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuHKM = new DevExpress.XtraBars.BarSubItem();
            this.MenuHKM_HKCommodityM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuHKM_HKTradeRulesM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuHKM_MinVolumeOfBusM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuHKM_HKCostsM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuHKM_SpotPositionM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM = new DevExpress.XtraBars.BarSubItem();
            this.MenuFuturesM_CommodityFuseM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_FuturesTradeRulesM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_FutureCostsResultM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_PositionLimitV = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_CFBailScaleValue = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_SIFPositionAndBailM = new DevExpress.XtraBars.BarButtonItem();
            this.PersonalM = new DevExpress.XtraBars.BarButtonItem();
            this.MenuHelpM = new DevExpress.XtraBars.BarSubItem();
            this.MenuHelpM_Help = new DevExpress.XtraBars.BarButtonItem();
            this.MenuHelpM_About = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStaticItemLoginName = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItemSyTime = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFutureM = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem14 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem15 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem17 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem18 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem24 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem25 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem26 = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_P = new DevExpress.XtraBars.BarSubItem();
            this.MenuFuturesM_PositionLimitVM = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem27 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem28 = new DevExpress.XtraBars.BarButtonItem();
            this.MenuFuturesM_C = new DevExpress.XtraBars.BarSubItem();
            this.MenuFuturesM_CFBailScaleValueM = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem30 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem31 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem32 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem33 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.xtraTabbedMdiManager1 = new DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(this.components);
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.timerDisplayStime = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowMoveBarOnToolbar = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageList1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.AccountM,
            this.AccountM_TransactionM,
            this.AccountM_ManagerM,
            this.AccountM_RightM,
            this.MatchM,
            this.MatchM_CenterM,
            this.MatchM_GuideM,
            this.barButtonItem6,
            this.CounterM,
            this.MenuCommParaM,
            this.MenuSpotM,
            this.MenuFutureM,
            this.MenuCommParaM_BreedClassM,
            this.MenuCommParaM_CommodityM,
            this.MenuCommParaM_BourseM,
            this.MenuSpotM_MinVolumeOfBusM,
            this.MenuSpotM_SpotCostsM,
            this.MenuSpotM_SpotTradeRulesM,
            this.MenuSpotM_SpotPositionM,
            this.MenuFuturesM,
            this.barButtonItem11,
            this.MenuFuturesM_FuturesTradeRulesM,
            this.barButtonItem14,
            this.barButtonItem15,
            this.barButtonItem17,
            this.barButtonItem18,
            this.MenuFuturesM_CommodityFuseM,
            this.MenuFuturesM_FutureCostsResultM,
            this.barButtonItem24,
            this.barButtonItem25,
            this.barButtonItem26,
            this.MenuFuturesM_P,
            this.barButtonItem27,
            this.MenuFuturesM_PositionLimitVM,
            this.barButtonItem28,
            this.MenuFuturesM_C,
            this.MenuFuturesM_CFBailScaleValueM,
            this.barButtonItem30,
            this.barButtonItem31,
            this.barButtonItem32,
            this.barButtonItem33,
            this.MenuSpotM_UnitConversionM,
            this.PersonalM,
            this.CounterM_ConfigM,
            this.MenuFuturesM_SIFPositionAndBailM,
            this.MenuFuturesM_PositionLimitV,
            this.MenuFuturesM_CFBailScaleValue,
            this.AccountM_Exits,
            this.Menutool_TransactionM,
            this.Menutool_ManagerM,
            this.Menutool_RightM,
            this.Menutool_MatchM,
            this.Menutool_CounterM,
            this.Menutool_BreedClassM,
            this.Menutool_CommodityM,
            this.Menutool_BourseM,
            this.barStaticItemLoginName,
            this.barStaticItemSyTime,
            this.MenuHelpM,
            this.MenuHelpM_Help,
            this.MenuHelpM_About,
            this.MenuHKM,
            this.MenuHKM_HKCommodityM,
            this.MenuHKM_HKTradeRulesM,
            this.MenuHKM_HKCostsM,
            this.barButtonItem1,
            this.barButtonItem2,
            this.MenuHKM_MinVolumeOfBusM,
            this.MenuHKM_SpotPositionM,
            this.MenuFuturesM_TodaySettlementPriceM});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 81;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_TransactionM),
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_ManagerM),
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_RightM),
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_MatchM, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_CounterM, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_BreedClassM, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_CommodityM),
            new DevExpress.XtraBars.LinkPersistInfo(this.Menutool_BourseM)});
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Custom 2";
            // 
            // Menutool_TransactionM
            // 
            this.Menutool_TransactionM.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.Menutool_TransactionM.Caption = "交易员管理";
            this.Menutool_TransactionM.Hint = "交易员管理";
            this.Menutool_TransactionM.Id = 59;
            this.Menutool_TransactionM.ImageIndex = 0;
            this.Menutool_TransactionM.Name = "Menutool_TransactionM";
            this.Menutool_TransactionM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // Menutool_ManagerM
            // 
            this.Menutool_ManagerM.Caption = "管理员管理";
            this.Menutool_ManagerM.Hint = "管理员管理";
            this.Menutool_ManagerM.Id = 60;
            this.Menutool_ManagerM.ImageIndex = 16;
            this.Menutool_ManagerM.Name = "Menutool_ManagerM";
            this.Menutool_ManagerM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // Menutool_RightM
            // 
            this.Menutool_RightM.Caption = "管理员权限组管理";
            this.Menutool_RightM.Hint = "管理员权限组管理";
            this.Menutool_RightM.Id = 61;
            this.Menutool_RightM.ImageIndex = 17;
            this.Menutool_RightM.Name = "Menutool_RightM";
            this.Menutool_RightM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // Menutool_MatchM
            // 
            this.Menutool_MatchM.Caption = "撮合中心配置";
            this.Menutool_MatchM.Hint = "撮合中心配置";
            this.Menutool_MatchM.Id = 62;
            this.Menutool_MatchM.ImageIndex = 18;
            this.Menutool_MatchM.Name = "Menutool_MatchM";
            this.Menutool_MatchM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // Menutool_CounterM
            // 
            this.Menutool_CounterM.Caption = "柜台配置";
            this.Menutool_CounterM.Hint = "柜台配置";
            this.Menutool_CounterM.Id = 63;
            this.Menutool_CounterM.ImageIndex = 11;
            this.Menutool_CounterM.Name = "Menutool_CounterM";
            this.Menutool_CounterM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // Menutool_BreedClassM
            // 
            this.Menutool_BreedClassM.Caption = "品种管理";
            this.Menutool_BreedClassM.Hint = "品种管理";
            this.Menutool_BreedClassM.Id = 64;
            this.Menutool_BreedClassM.ImageIndex = 7;
            this.Menutool_BreedClassM.Name = "Menutool_BreedClassM";
            this.Menutool_BreedClassM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // Menutool_CommodityM
            // 
            this.Menutool_CommodityM.Caption = "代码管理";
            this.Menutool_CommodityM.Hint = "代码管理";
            this.Menutool_CommodityM.Id = 65;
            this.Menutool_CommodityM.ImageIndex = 10;
            this.Menutool_CommodityM.Name = "Menutool_CommodityM";
            this.Menutool_CommodityM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // Menutool_BourseM
            // 
            this.Menutool_BourseM.Caption = "交易所管理";
            this.Menutool_BourseM.Hint = "交易所管理";
            this.Menutool_BourseM.Id = 66;
            this.Menutool_BourseM.ImageIndex = 6;
            this.Menutool_BourseM.Name = "Menutool_BourseM";
            this.Menutool_BourseM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // bar2
            // 
            this.bar2.BarName = "Custom 3";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.AccountM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MatchM),
            new DevExpress.XtraBars.LinkPersistInfo(this.CounterM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuCommParaM, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuSpotM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHKM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM),
            new DevExpress.XtraBars.LinkPersistInfo(this.PersonalM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHelpM)});
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Custom 3";
            // 
            // AccountM
            // 
            this.AccountM.Caption = "账号管理 ";
            this.AccountM.Id = 0;
            this.AccountM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, this.AccountM_TransactionM, "交易员管理"),
            new DevExpress.XtraBars.LinkPersistInfo(this.AccountM_ManagerM),
            new DevExpress.XtraBars.LinkPersistInfo(this.AccountM_RightM),
            new DevExpress.XtraBars.LinkPersistInfo(this.AccountM_Exits)});
            this.AccountM.Name = "AccountM";
            // 
            // AccountM_TransactionM
            // 
            this.AccountM_TransactionM.Caption = "交易员管理";
            this.AccountM_TransactionM.Id = 1;
            this.AccountM_TransactionM.ImageIndex = 0;
            this.AccountM_TransactionM.Name = "AccountM_TransactionM";
            this.AccountM_TransactionM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // AccountM_ManagerM
            // 
            this.AccountM_ManagerM.Caption = "管理员管理";
            this.AccountM_ManagerM.Id = 2;
            this.AccountM_ManagerM.ImageIndex = 16;
            this.AccountM_ManagerM.Name = "AccountM_ManagerM";
            this.AccountM_ManagerM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // AccountM_RightM
            // 
            this.AccountM_RightM.Caption = "管理员权限组管理";
            this.AccountM_RightM.Id = 3;
            this.AccountM_RightM.ImageIndex = 17;
            this.AccountM_RightM.Name = "AccountM_RightM";
            this.AccountM_RightM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // AccountM_Exits
            // 
            this.AccountM_Exits.Caption = "退出";
            this.AccountM_Exits.Id = 58;
            this.AccountM_Exits.ImageIndex = 13;
            this.AccountM_Exits.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X));
            this.AccountM_Exits.Name = "AccountM_Exits";
            this.AccountM_Exits.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.AccountM_Exits_ItemClick);
            // 
            // MatchM
            // 
            this.MatchM.Caption = "撮合中心管理 ";
            this.MatchM.Id = 4;
            this.MatchM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MatchM_CenterM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MatchM_GuideM)});
            this.MatchM.Name = "MatchM";
            // 
            // MatchM_CenterM
            // 
            this.MatchM_CenterM.Caption = "撮合中心配置";
            this.MatchM_CenterM.Id = 5;
            this.MatchM_CenterM.ImageIndex = 18;
            this.MatchM_CenterM.Name = "MatchM_CenterM";
            this.MatchM_CenterM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // MatchM_GuideM
            // 
            this.MatchM_GuideM.Caption = "简单配置向导";
            this.MatchM_GuideM.Id = 6;
            this.MatchM_GuideM.ImageIndex = 19;
            this.MatchM_GuideM.Name = "MatchM_GuideM";
            this.MatchM_GuideM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // CounterM
            // 
            this.CounterM.Caption = "清算柜台管理";
            this.CounterM.Id = 8;
            this.CounterM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.CounterM_ConfigM)});
            this.CounterM.Name = "CounterM";
            // 
            // CounterM_ConfigM
            // 
            this.CounterM_ConfigM.Caption = "柜台配置";
            this.CounterM_ConfigM.Id = 54;
            this.CounterM_ConfigM.ImageIndex = 11;
            this.CounterM_ConfigM.Name = "CounterM_ConfigM";
            this.CounterM_ConfigM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // MenuCommParaM
            // 
            this.MenuCommParaM.Caption = "参数管理";
            this.MenuCommParaM.Id = 9;
            this.MenuCommParaM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuCommParaM_BreedClassM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuCommParaM_CommodityM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuCommParaM_BourseM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_TodaySettlementPriceM)});
            this.MenuCommParaM.Name = "MenuCommParaM";
            // 
            // MenuCommParaM_BreedClassM
            // 
            this.MenuCommParaM_BreedClassM.Caption = "品种管理";
            this.MenuCommParaM_BreedClassM.Id = 12;
            this.MenuCommParaM_BreedClassM.ImageIndex = 7;
            this.MenuCommParaM_BreedClassM.Name = "MenuCommParaM_BreedClassM";
            this.MenuCommParaM_BreedClassM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuCommParaM_CommodityM
            // 
            this.MenuCommParaM_CommodityM.Caption = "代码管理";
            this.MenuCommParaM_CommodityM.Id = 13;
            this.MenuCommParaM_CommodityM.ImageIndex = 10;
            this.MenuCommParaM_CommodityM.Name = "MenuCommParaM_CommodityM";
            this.MenuCommParaM_CommodityM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuCommParaM_BourseM
            // 
            this.MenuCommParaM_BourseM.Caption = "交易所管理";
            this.MenuCommParaM_BourseM.Id = 14;
            this.MenuCommParaM_BourseM.ImageIndex = 6;
            this.MenuCommParaM_BourseM.Name = "MenuCommParaM_BourseM";
            this.MenuCommParaM_BourseM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM_TodaySettlementPriceM
            // 
            this.MenuFuturesM_TodaySettlementPriceM.Caption = "结算价管理";
            this.MenuFuturesM_TodaySettlementPriceM.Id = 80;
            this.MenuFuturesM_TodaySettlementPriceM.ImageIndex = 41;
            this.MenuFuturesM_TodaySettlementPriceM.Name = "MenuFuturesM_TodaySettlementPriceM";
            this.MenuFuturesM_TodaySettlementPriceM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuSpotM
            // 
            this.MenuSpotM.Caption = "现货管理";
            this.MenuSpotM.Id = 10;
            this.MenuSpotM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuSpotM_SpotTradeRulesM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuSpotM_MinVolumeOfBusM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuSpotM_SpotCostsM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuSpotM_UnitConversionM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuSpotM_SpotPositionM)});
            this.MenuSpotM.Name = "MenuSpotM";
            // 
            // MenuSpotM_SpotTradeRulesM
            // 
            this.MenuSpotM_SpotTradeRulesM.Caption = "现货交易规则管理";
            this.MenuSpotM_SpotTradeRulesM.Id = 21;
            this.MenuSpotM_SpotTradeRulesM.ImageIndex = 15;
            this.MenuSpotM_SpotTradeRulesM.Name = "MenuSpotM_SpotTradeRulesM";
            this.MenuSpotM_SpotTradeRulesM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuSpotM_MinVolumeOfBusM
            // 
            this.MenuSpotM_MinVolumeOfBusM.Caption = "最小交易单位管理";
            this.MenuSpotM_MinVolumeOfBusM.Id = 18;
            this.MenuSpotM_MinVolumeOfBusM.ImageIndex = 20;
            this.MenuSpotM_MinVolumeOfBusM.Name = "MenuSpotM_MinVolumeOfBusM";
            this.MenuSpotM_MinVolumeOfBusM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuSpotM_SpotCostsM
            // 
            this.MenuSpotM_SpotCostsM.Caption = "现货交易费用管理";
            this.MenuSpotM_SpotCostsM.Id = 19;
            this.MenuSpotM_SpotCostsM.ImageIndex = 14;
            this.MenuSpotM_SpotCostsM.Name = "MenuSpotM_SpotCostsM";
            this.MenuSpotM_SpotCostsM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuSpotM_UnitConversionM
            // 
            this.MenuSpotM_UnitConversionM.Caption = "现货单位换算管理";
            this.MenuSpotM_UnitConversionM.Id = 52;
            this.MenuSpotM_UnitConversionM.ImageIndex = 28;
            this.MenuSpotM_UnitConversionM.Name = "MenuSpotM_UnitConversionM";
            this.MenuSpotM_UnitConversionM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuSpotM_SpotPositionM
            // 
            this.MenuSpotM_SpotPositionM.Caption = "持仓限制管理";
            this.MenuSpotM_SpotPositionM.Id = 22;
            this.MenuSpotM_SpotPositionM.ImageIndex = 22;
            this.MenuSpotM_SpotPositionM.Name = "MenuSpotM_SpotPositionM";
            this.MenuSpotM_SpotPositionM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuHKM
            // 
            this.MenuHKM.Caption = "港股管理";
            this.MenuHKM.Id = 72;
            this.MenuHKM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHKM_HKCommodityM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHKM_HKTradeRulesM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHKM_MinVolumeOfBusM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHKM_HKCostsM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHKM_SpotPositionM)});
            this.MenuHKM.Name = "MenuHKM";
            // 
            // MenuHKM_HKCommodityM
            // 
            this.MenuHKM_HKCommodityM.Caption = "港股代码管理";
            this.MenuHKM_HKCommodityM.Id = 73;
            this.MenuHKM_HKCommodityM.ImageIndex = 35;
            this.MenuHKM_HKCommodityM.Name = "MenuHKM_HKCommodityM";
            this.MenuHKM_HKCommodityM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuHKM_HKTradeRulesM
            // 
            this.MenuHKM_HKTradeRulesM.Caption = "港股交易规则管理";
            this.MenuHKM_HKTradeRulesM.Id = 74;
            this.MenuHKM_HKTradeRulesM.ImageIndex = 39;
            this.MenuHKM_HKTradeRulesM.Name = "MenuHKM_HKTradeRulesM";
            this.MenuHKM_HKTradeRulesM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuHKM_MinVolumeOfBusM
            // 
            this.MenuHKM_MinVolumeOfBusM.Caption = "最小交易单位管理";
            this.MenuHKM_MinVolumeOfBusM.Id = 78;
            this.MenuHKM_MinVolumeOfBusM.ImageIndex = 20;
            this.MenuHKM_MinVolumeOfBusM.Name = "MenuHKM_MinVolumeOfBusM";
            this.MenuHKM_MinVolumeOfBusM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuHKM_HKCostsM
            // 
            this.MenuHKM_HKCostsM.Caption = "港股交易费用管理";
            this.MenuHKM_HKCostsM.Id = 75;
            this.MenuHKM_HKCostsM.ImageIndex = 37;
            this.MenuHKM_HKCostsM.Name = "MenuHKM_HKCostsM";
            this.MenuHKM_HKCostsM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuHKM_SpotPositionM
            // 
            this.MenuHKM_SpotPositionM.Caption = "持仓限制管理";
            this.MenuHKM_SpotPositionM.Id = 79;
            this.MenuHKM_SpotPositionM.ImageIndex = 22;
            this.MenuHKM_SpotPositionM.Name = "MenuHKM_SpotPositionM";
            this.MenuHKM_SpotPositionM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM
            // 
            this.MenuFuturesM.Caption = "期货管理";
            this.MenuFuturesM.Id = 25;
            this.MenuFuturesM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_CommodityFuseM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_FuturesTradeRulesM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_FutureCostsResultM),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_PositionLimitV),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_CFBailScaleValue),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_SIFPositionAndBailM)});
            this.MenuFuturesM.Name = "MenuFuturesM";
            // 
            // MenuFuturesM_CommodityFuseM
            // 
            this.MenuFuturesM_CommodityFuseM.Caption = "熔断管理";
            this.MenuFuturesM_CommodityFuseM.Id = 34;
            this.MenuFuturesM_CommodityFuseM.ImageIndex = 9;
            this.MenuFuturesM_CommodityFuseM.Name = "MenuFuturesM_CommodityFuseM";
            this.MenuFuturesM_CommodityFuseM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM_FuturesTradeRulesM
            // 
            this.MenuFuturesM_FuturesTradeRulesM.Caption = "期货交易规则管理";
            //this.MenuFuturesM_FutureCostsResultM.Enabled
            this.MenuFuturesM_FuturesTradeRulesM.Id = 27;
            this.MenuFuturesM_FuturesTradeRulesM.ImageIndex = 27;
            this.MenuFuturesM_FuturesTradeRulesM.Name = "MenuFuturesM_FuturesTradeRulesM";
            this.MenuFuturesM_FuturesTradeRulesM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM_FutureCostsResultM
            // 
            this.MenuFuturesM_FutureCostsResultM.Caption = "期货交易费用管理";
            this.MenuFuturesM_FutureCostsResultM.Id = 36;
            this.MenuFuturesM_FutureCostsResultM.ImageIndex = 25;
            this.MenuFuturesM_FutureCostsResultM.Name = "MenuFuturesM_FutureCostsResultM";
            this.MenuFuturesM_FutureCostsResultM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM_PositionLimitV
            // 
            this.MenuFuturesM_PositionLimitV.Caption = "商品期货持仓管理";
            this.MenuFuturesM_PositionLimitV.Id = 56;
            this.MenuFuturesM_PositionLimitV.ImageIndex = 26;
            this.MenuFuturesM_PositionLimitV.Name = "MenuFuturesM_PositionLimitV";
            this.MenuFuturesM_PositionLimitV.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM_CFBailScaleValue
            // 
            this.MenuFuturesM_CFBailScaleValue.Caption = "商品期货保证金管理";
            this.MenuFuturesM_CFBailScaleValue.Id = 57;
            this.MenuFuturesM_CFBailScaleValue.ImageIndex = 8;
            this.MenuFuturesM_CFBailScaleValue.Name = "MenuFuturesM_CFBailScaleValue";
            this.MenuFuturesM_CFBailScaleValue.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM_SIFPositionAndBailM
            // 
            this.MenuFuturesM_SIFPositionAndBailM.Caption = "股指期货持仓和保证金管理";
            this.MenuFuturesM_SIFPositionAndBailM.Id = 55;
            this.MenuFuturesM_SIFPositionAndBailM.ImageIndex = 24;
            this.MenuFuturesM_SIFPositionAndBailM.Name = "MenuFuturesM_SIFPositionAndBailM";
            this.MenuFuturesM_SIFPositionAndBailM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // PersonalM
            // 
            this.PersonalM.Caption = "个人信息";
            this.PersonalM.Id = 53;
            this.PersonalM.Name = "PersonalM";
            this.PersonalM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // MenuHelpM
            // 
            this.MenuHelpM.Caption = "帮助";
            this.MenuHelpM.Id = 69;
            this.MenuHelpM.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHelpM_Help),
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuHelpM_About)});
            this.MenuHelpM.Name = "MenuHelpM";
            // 
            // MenuHelpM_Help
            // 
            this.MenuHelpM_Help.Caption = "帮助主题";
            this.MenuHelpM_Help.Id = 70;
            this.MenuHelpM_Help.ImageIndex = 30;
            this.MenuHelpM_Help.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F1);
            this.MenuHelpM_Help.Name = "MenuHelpM_Help";
            this.MenuHelpM_Help.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.MenuHelpM_Help_ItemClick);
            // 
            // MenuHelpM_About
            // 
            this.MenuHelpM_About.Caption = "关于";
            this.MenuHelpM_About.Id = 71;
            this.MenuHelpM_About.ImageIndex = 31;
            this.MenuHelpM_About.Name = "MenuHelpM_About";
            this.MenuHelpM_About.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButton_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Custom 4";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemLoginName),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemSyTime)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Custom 4";
            // 
            // barStaticItemLoginName
            // 
            this.barStaticItemLoginName.Caption = "barStaticItem1";
            this.barStaticItemLoginName.Id = 67;
            this.barStaticItemLoginName.Name = "barStaticItemLoginName";
            this.barStaticItemLoginName.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticItemSyTime
            // 
            this.barStaticItemSyTime.Caption = "barStaticItem1";
            this.barStaticItemSyTime.Id = 68;
            this.barStaticItemSyTime.Name = "barStaticItemSyTime";
            this.barStaticItemSyTime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "TradeM.png");
            this.imageList1.Images.SetKeyName(1, "AddCFBailScaleV.png");
            this.imageList1.Images.SetKeyName(2, "AddFund.png");
            this.imageList1.Images.SetKeyName(3, "AddFuturesTradeRules.png");
            this.imageList1.Images.SetKeyName(4, "AddPositionLimitV.png");
            this.imageList1.Images.SetKeyName(5, "AddSpotTradeRules.png");
            this.imageList1.Images.SetKeyName(6, "BourseM.png");
            this.imageList1.Images.SetKeyName(7, "BreedClassM.png");
            this.imageList1.Images.SetKeyName(8, "CFBailScaleValueM.png");
            this.imageList1.Images.SetKeyName(9, "CommodityFuseM.png");
            this.imageList1.Images.SetKeyName(10, "CommodityM.png");
            this.imageList1.Images.SetKeyName(11, "CounterEdit.gif");
            this.imageList1.Images.SetKeyName(12, "CounterEdit.png");
            this.imageList1.Images.SetKeyName(13, "Exit.png");
            this.imageList1.Images.SetKeyName(14, "FutureCostsM.png");
            this.imageList1.Images.SetKeyName(15, "FuturesRuleM.png");
            this.imageList1.Images.SetKeyName(16, "ManagerM.png");
            this.imageList1.Images.SetKeyName(17, "ManagerRightM.png");
            this.imageList1.Images.SetKeyName(18, "MatchCenterSet.png");
            this.imageList1.Images.SetKeyName(19, "MatchConfig.gif");
            this.imageList1.Images.SetKeyName(20, "MinUnitM.png");
            this.imageList1.Images.SetKeyName(21, "OneInfo.png");
            this.imageList1.Images.SetKeyName(22, "PositionLimitValueM.png");
            this.imageList1.Images.SetKeyName(23, "RightEdit.png");
            this.imageList1.Images.SetKeyName(24, "SIFPositionAndBailM.png");
            this.imageList1.Images.SetKeyName(25, "SpotCostM.png");
            this.imageList1.Images.SetKeyName(26, "SpotPositionM.png");
            this.imageList1.Images.SetKeyName(27, "SpotRulesM.png");
            this.imageList1.Images.SetKeyName(28, "SpotUnitConversionM.png");
            this.imageList1.Images.SetKeyName(29, "TradeEdit.png");
            this.imageList1.Images.SetKeyName(30, "Help.gif");
            this.imageList1.Images.SetKeyName(31, "About.gif");
            this.imageList1.Images.SetKeyName(32, "AboutLeft.jpg");
            this.imageList1.Images.SetKeyName(33, "Transfer.png");
            this.imageList1.Images.SetKeyName(34, "HKCommodityM.ico");
            this.imageList1.Images.SetKeyName(35, "HKCommodityM.png");
            this.imageList1.Images.SetKeyName(36, "HKSpotCostM.ico");
            this.imageList1.Images.SetKeyName(37, "HKSpotCostM.png");
            this.imageList1.Images.SetKeyName(38, "HKSpotRulesM.ico");
            this.imageList1.Images.SetKeyName(39, "HKSpotRulesM.png");
            this.imageList1.Images.SetKeyName(40, "Transfer.ico");
            this.imageList1.Images.SetKeyName(41, "TodaySetPrice.png");
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "撮合代码分配";
            this.barButtonItem6.Id = 7;
            this.barButtonItem6.Name = "barButtonItem6";
            // 
            // MenuFutureM
            // 
            this.MenuFutureM.Caption = "管理中心期货管理";
            this.MenuFutureM.Id = 11;
            this.MenuFutureM.Name = "MenuFutureM";
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "添加商品期货保证金比例范围";
            this.barButtonItem11.Id = 26;
            this.barButtonItem11.Name = "barButtonItem11";
            // 
            // barButtonItem14
            // 
            this.barButtonItem14.Caption = "添加最小和单笔委托量";
            this.barButtonItem14.Id = 29;
            this.barButtonItem14.Name = "barButtonItem14";
            // 
            // barButtonItem15
            // 
            this.barButtonItem15.Caption = "添加期货持仓限制范围";
            this.barButtonItem15.Id = 30;
            this.barButtonItem15.Name = "barButtonItem15";
            // 
            // barButtonItem17
            // 
            this.barButtonItem17.Caption = "商品期货保证金比例范围管理";
            this.barButtonItem17.Id = 32;
            this.barButtonItem17.Name = "barButtonItem17";
            // 
            // barButtonItem18
            // 
            this.barButtonItem18.Caption = "商品期货保证金比例单值管理";
            this.barButtonItem18.Id = 33;
            this.barButtonItem18.Name = "barButtonItem18";
            // 
            // barButtonItem24
            // 
            this.barButtonItem24.Caption = "商品期货持仓限制单值管理";
            this.barButtonItem24.Id = 39;
            this.barButtonItem24.Name = "barButtonItem24";
            // 
            // barButtonItem25
            // 
            this.barButtonItem25.Caption = "股指期货保证金管理";
            this.barButtonItem25.Id = 40;
            this.barButtonItem25.Name = "barButtonItem25";
            // 
            // barButtonItem26
            // 
            this.barButtonItem26.Caption = "股指期货持仓限制管理";
            this.barButtonItem26.Id = 41;
            this.barButtonItem26.Name = "barButtonItem26";
            // 
            // MenuFuturesM_P
            // 
            this.MenuFuturesM_P.Caption = "持仓管理";
            this.MenuFuturesM_P.Id = 42;
            this.MenuFuturesM_P.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_PositionLimitVM)});
            this.MenuFuturesM_P.Name = "MenuFuturesM_P";
            // 
            // MenuFuturesM_PositionLimitVM
            // 
            this.MenuFuturesM_PositionLimitVM.Caption = "商品期货持仓管理";
            this.MenuFuturesM_PositionLimitVM.Id = 44;
            this.MenuFuturesM_PositionLimitVM.Name = "MenuFuturesM_PositionLimitVM";
            this.MenuFuturesM_PositionLimitVM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // barButtonItem27
            // 
            this.barButtonItem27.Caption = "商品期货持仓限制范围管理";
            this.barButtonItem27.Id = 43;
            this.barButtonItem27.Name = "barButtonItem27";
            // 
            // barButtonItem28
            // 
            this.barButtonItem28.Caption = "股指期货持仓限制管理";
            this.barButtonItem28.Id = 45;
            this.barButtonItem28.Name = "barButtonItem28";
            this.barButtonItem28.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // MenuFuturesM_C
            // 
            this.MenuFuturesM_C.Caption = "保证金管理";
            this.MenuFuturesM_C.Id = 46;
            this.MenuFuturesM_C.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.MenuFuturesM_CFBailScaleValueM)});
            this.MenuFuturesM_C.Name = "MenuFuturesM_C";
            // 
            // MenuFuturesM_CFBailScaleValueM
            // 
            this.MenuFuturesM_CFBailScaleValueM.Caption = "商品期货保证金比例管理";
            this.MenuFuturesM_CFBailScaleValueM.Id = 47;
            this.MenuFuturesM_CFBailScaleValueM.Name = "MenuFuturesM_CFBailScaleValueM";
            this.MenuFuturesM_CFBailScaleValueM.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.Item_ItemClick);
            // 
            // barButtonItem30
            // 
            this.barButtonItem30.Caption = "商品期货保证金比例单值管理";
            this.barButtonItem30.Id = 48;
            this.barButtonItem30.Name = "barButtonItem30";
            // 
            // barButtonItem31
            // 
            this.barButtonItem31.Caption = "股指期货保证金管理";
            this.barButtonItem31.Id = 49;
            this.barButtonItem31.Name = "barButtonItem31";
            // 
            // barButtonItem32
            // 
            this.barButtonItem32.Caption = "添加商品期货保证金比例范围";
            this.barButtonItem32.Id = 50;
            this.barButtonItem32.Name = "barButtonItem32";
            // 
            // barButtonItem33
            // 
            this.barButtonItem33.Caption = "添加期货持仓限制范围";
            this.barButtonItem33.Id = 51;
            this.barButtonItem33.Name = "barButtonItem33";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "转账管理";
            this.barButtonItem1.Id = 76;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 77;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // xtraTabbedMdiManager1
            // 
            this.xtraTabbedMdiManager1.MdiParent = this;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(0, 56);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(792, 494);
            this.pictureEdit1.TabIndex = 5;
            // 
            // timerDisplayStime
            // 
            this.timerDisplayStime.Tick += new System.EventHandler(this.timerDisplayStime_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VE撮合结算系统-管理中心";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem AccountM;
        private DevExpress.XtraBars.BarButtonItem AccountM_TransactionM;
        private DevExpress.XtraBars.BarButtonItem AccountM_ManagerM;
        private DevExpress.XtraBars.BarButtonItem AccountM_RightM;
        private DevExpress.XtraBars.BarSubItem MatchM;
        private DevExpress.XtraBars.BarButtonItem MatchM_CenterM;
        private DevExpress.XtraBars.BarButtonItem MatchM_GuideM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraTabbedMdi.XtraTabbedMdiManager xtraTabbedMdiManager1;
        private DevExpress.XtraBars.BarSubItem MenuCommParaM;
        private DevExpress.XtraBars.BarSubItem MenuSpotM;
        private DevExpress.XtraBars.BarSubItem MenuFutureM;
        private DevExpress.XtraBars.BarButtonItem MenuCommParaM_BreedClassM;
        private DevExpress.XtraBars.BarButtonItem MenuCommParaM_CommodityM;
        private DevExpress.XtraBars.BarButtonItem MenuCommParaM_BourseM;
        private DevExpress.XtraBars.BarButtonItem MenuSpotM_MinVolumeOfBusM;
        private DevExpress.XtraBars.BarButtonItem MenuSpotM_SpotCostsM;
        private DevExpress.XtraBars.BarButtonItem MenuSpotM_SpotTradeRulesM;
        private DevExpress.XtraBars.BarButtonItem MenuSpotM_SpotPositionM;
        private DevExpress.XtraBars.BarSubItem MenuFuturesM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_FuturesTradeRulesM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem14;
        private DevExpress.XtraBars.BarButtonItem barButtonItem15;
        private DevExpress.XtraBars.BarButtonItem barButtonItem17;
        private DevExpress.XtraBars.BarButtonItem barButtonItem18;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_CommodityFuseM;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_FutureCostsResultM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem24;
        private DevExpress.XtraBars.BarButtonItem barButtonItem25;
        private DevExpress.XtraBars.BarButtonItem barButtonItem26;
        private DevExpress.XtraBars.BarSubItem MenuFuturesM_P;
        private DevExpress.XtraBars.BarButtonItem barButtonItem27;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_PositionLimitVM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem28;
        private DevExpress.XtraBars.BarSubItem MenuFuturesM_C;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_CFBailScaleValueM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem30;
        private DevExpress.XtraBars.BarButtonItem barButtonItem31;
        private DevExpress.XtraBars.BarButtonItem barButtonItem32;
        private DevExpress.XtraBars.BarButtonItem barButtonItem33;
        private DevExpress.XtraBars.BarButtonItem MenuSpotM_UnitConversionM;
        private DevExpress.XtraBars.BarButtonItem PersonalM;
        private DevExpress.XtraBars.BarButtonItem CounterM_ConfigM;
        private DevExpress.XtraBars.BarSubItem CounterM;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_SIFPositionAndBailM;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_PositionLimitV;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_CFBailScaleValue;
        private ImageList imageList1;
        private DevExpress.XtraBars.BarButtonItem AccountM_Exits;
        private DevExpress.XtraBars.BarButtonItem Menutool_TransactionM;
        private DevExpress.XtraBars.BarButtonItem Menutool_ManagerM;
        private DevExpress.XtraBars.BarButtonItem Menutool_RightM;
        private DevExpress.XtraBars.BarButtonItem Menutool_MatchM;
        private DevExpress.XtraBars.BarButtonItem Menutool_CounterM;
        private DevExpress.XtraBars.BarButtonItem Menutool_BreedClassM;
        private DevExpress.XtraBars.BarButtonItem Menutool_CommodityM;
        private DevExpress.XtraBars.BarButtonItem Menutool_BourseM;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraBars.BarStaticItem barStaticItemLoginName;
        private DevExpress.XtraBars.BarStaticItem barStaticItemSyTime;
        private Timer timerDisplayStime;
        private DevExpress.XtraBars.BarSubItem MenuHelpM;
        private DevExpress.XtraBars.BarButtonItem MenuHelpM_Help;
        private DevExpress.XtraBars.BarButtonItem MenuHelpM_About;
        private DevExpress.XtraBars.BarSubItem MenuHKM;
        private DevExpress.XtraBars.BarButtonItem MenuHKM_HKCommodityM;
        private DevExpress.XtraBars.BarButtonItem MenuHKM_HKTradeRulesM;
        private DevExpress.XtraBars.BarButtonItem MenuHKM_HKCostsM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem MenuHKM_MinVolumeOfBusM;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem MenuHKM_SpotPositionM;
        private DevExpress.XtraBars.BarButtonItem MenuFuturesM_TodaySettlementPriceM;

    }
}



