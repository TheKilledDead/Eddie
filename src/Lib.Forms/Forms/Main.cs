// <eddie_source_header>
// This file is part of Eddie/AirVPN software.
// Copyright (C)2014-2016 AirVPN (support@airvpn.org) / https://airvpn.org
//
// Eddie is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Eddie is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Eddie. If not, see <http://www.gnu.org/licenses/>.
// </eddie_source_header>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using Eddie.Common;
using Eddie.Core;
using Eddie.Forms.Controls;

namespace Eddie.Forms.Forms
{
	public partial class Main : Eddie.Forms.Form
	{
		private Controls.ToolTip m_toolTip;
		private Controls.TabNavigator m_tabMain;
		private Skin.Button m_cmdAbout;
		private Skin.Button m_cmdPreferences;
		private Controls.ChartSpeed m_pnlCharts;
		private Controls.MenuButton m_cmdMainMenu;
		private Controls.ProgressInfinite m_imgProgressInfinite;
		private System.Windows.Forms.NotifyIcon m_windowsNotifyIcon;
		private Forms.WindowReport m_windowReport = null;

		private ListViewServers m_listViewServers;
		private ListViewAreas m_listViewAreas;

		private Dictionary<string, ListViewItemStats> m_statsItems = new Dictionary<string, ListViewItemStats>();

		private int m_windowMinimumWidth = 700;
		private int m_windowMinimumHeight = 370;
		private int m_windowDefaultWidth = 800;
		private int m_windowDefaultHeight = 450;
		private bool m_lockCoordUpdate = false;
		private int m_topHeaderHeight = 30;

		private bool m_formReady = false;
		private bool m_closing = false;
		private bool m_windowStateSetByShortcut = false;

		private Icon m_iconNormal;
		private Icon m_iconGray;
		private Bitmap m_bitmapNetlockStatusOff;
		private Bitmap m_bitmapNetlockStatusOn;
		private Bitmap m_bitmapStatusGreen;
		private Bitmap m_bitmapStatusYellow;
		private Bitmap m_bitmapStatusRed;
		private Bitmap m_bitmapNetlockOn;
		private Bitmap m_bitmapNetlockOff;

		public Main()
		{
			Eddie.Forms.Skin.SkinReference.Load(Engine.Instance.Storage.Get("gui.skin"));

			OnPreInitializeComponent();
			InitializeComponent();
			OnInitializeComponent();

			m_iconNormal = Eddie.Forms.Properties.Resources.icon1;
			m_iconGray = Eddie.Forms.Properties.Resources.icon_gray1;
			m_bitmapNetlockStatusOff = Eddie.Forms.Properties.Resources.netlock_status_off;
			m_bitmapNetlockStatusOn = Eddie.Forms.Properties.Resources.netlock_status_on;
			m_bitmapStatusGreen = Eddie.Forms.Properties.Resources.status_green;
			m_bitmapStatusYellow = Eddie.Forms.Properties.Resources.status_yellow;
			m_bitmapStatusRed = Eddie.Forms.Properties.Resources.status_red;
			m_bitmapNetlockOn = Eddie.Forms.Properties.Resources.netlock_on;
			m_bitmapNetlockOff = Eddie.Forms.Properties.Resources.netlock_off;

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
		}

		private delegate void DeInitDelegate();
		public void DeInit()
		{
			if (this.InvokeRequired)
			{
				DeInitDelegate inv = new DeInitDelegate(this.DeInit);

				this.Invoke(inv, new object[] { });
			}
			else
			{
				if (m_windowsNotifyIcon != null)
				{
					m_windowsNotifyIcon.Dispose();
					m_windowsNotifyIcon = null;
				}

				//DoDispose();

				m_closing = true;
				Close();
			}
		}

		/*
		private void DoDispose()
		{
			// Workaround experiment for Mono bug.
			// Mono bug https://bugs.debian.org/cgi-bin/bugreport.cgi?bug=742774
			// Mono bug https://bugs.debian.org/cgi-bin/bugreport.cgi?bug=727651

			DoDispose(lstLogs.ContextMenuStrip);
			lstLogs.ContextMenuStrip = null;

			DoDispose(m_listViewServers.ContextMenuStrip);
			m_listViewServers.ContextMenuStrip = null;

			DoDispose(m_listViewAreas.ContextMenuStrip);
			m_listViewAreas.ContextMenuStrip = null;

			DoDispose(this.ContextMenuStrip);
			this.ContextMenuStrip = null;

			DoDispose(this.mnuStatus);
			DoDispose(this.mnuConnect);
			DoDispose(this.mnuHomePage);
			DoDispose(this.mnuUser);
			DoDispose(this.mnuPorts);
			DoDispose(this.mnuSpeedTest);
			DoDispose(this.mnuSettings);
			DoDispose(this.mnuDevelopers);
			DoDispose(this.mnuDevelopersManText);
			DoDispose(this.mnuDevelopersManBBCode);
			DoDispose(this.mnuDevelopersUpdateManifest);
			DoDispose(this.mnuDevelopersDefaultManifest);
			DoDispose(this.mnuDevelopersReset);
			DoDispose(this.mnuTools);
			DoDispose(this.mnuToolsPortForwarding);
			DoDispose(this.mnuToolsNetworkMonitor);
			DoDispose(this.mnuAbout);
			DoDispose(this.mnuRestore);
			DoDispose(this.mnuExit);
			DoDispose(this.mnuStatus);
			DoDispose(this.mnuConnect);
			DoDispose(this.mnuHomePage);
			DoDispose(this.mnuUser);
			DoDispose(this.mnuPorts);
			DoDispose(this.mnuSpeedTest);
			DoDispose(this.mnuSettings);
			DoDispose(this.mnuDevelopers);
			DoDispose(this.mnuDevelopersManText);
			DoDispose(this.mnuDevelopersManBBCode);
			DoDispose(this.mnuDevelopersUpdateManifest);
			DoDispose(this.mnuDevelopersDefaultManifest);
			DoDispose(this.mnuDevelopersReset);
			DoDispose(this.mnuTools);
			DoDispose(this.mnuToolsPortForwarding);
			DoDispose(this.mnuToolsNetworkMonitor);
			DoDispose(this.mnuAbout);
			DoDispose(this.mnuRestore);
			DoDispose(this.mnuExit);
			DoDispose(this.mnuServersConnect);
			DoDispose(this.mnuServersWhiteList);
			DoDispose(this.mnuServersBlackList);
			DoDispose(this.mnuServersUndefined);
			DoDispose(this.mnuAreasWhiteList);
			DoDispose(this.mnuAreasBlackList);
			DoDispose(this.mnuAreasUndefined);
			DoDispose(this.mnuServersRefresh);
		}
		*/

		private void DoDispose(IDisposable o)
		{
			if (o != null)
			{
				o.Dispose();
				o = null;
			}
		}

		private void DoDispose(System.Windows.Forms.ToolStripMenuItem o)
		{
			o.Dispose();
			o = null;
		}

		public override void OnInitializeComponent()
		{
			base.OnInitializeComponent();
		}

		public override void OnApplySkin()
		{
			base.OnApplySkin();

			Skin.Apply(mnuMain);
			Skin.Apply(mnuServers);
			Skin.Apply(mnuAreas);
			Skin.Apply(mnuLogsContext);

			cmdLogin.Font = Skin.FontBig;
			cmdConnect.Font = Skin.FontBig;
			cmdLockedNetwork.Font = Skin.FontBig;
			cmdDisconnect.Font = Skin.FontBig;
			cmdCancel.Font = Skin.FontBig;

			mnuMain.Font = Skin.FontNormal;
			mnuServers.Font = Skin.FontNormal;
			mnuAreas.Font = Skin.FontNormal;
			mnuLogsContext.Font = Skin.FontNormal;

			if (m_tabMain != null)
			{
				m_tabMain.TabsFont = Skin.FontBig;
				m_tabMain.ComputeSizes();
			}

			lblWait1.Font = Skin.FontBig;
			lblWait2.Font = Skin.FontNormal;
			lblConnectedServerName.Font = Skin.FontBig;
			txtConnectedDownload.Font = Skin.FontMonoBig;
			txtConnectedUpload.Font = Skin.FontMonoBig;

			mnuMain.ImageScalingSize = Skin.MenuImageSize;
			mnuServers.ImageScalingSize = Skin.MenuImageSize;
			mnuAreas.ImageScalingSize = Skin.MenuImageSize;
			mnuLogsContext.ImageScalingSize = Skin.MenuImageSize;

			GuiUtils.FixHeightVs(this.txtLogin, lblLogin);
			GuiUtils.FixHeightVs(this.txtPassword, lblPassword);
			GuiUtils.FixHeightVs(this.cboKey, lblKey);

			GuiUtils.FixHeightVs(this.cboScoreType, this.chkShowAll);
			GuiUtils.FixHeightVs(this.cboScoreType, this.lblScoreType);
			GuiUtils.FixHeightVs(this.cboScoreType, this.chkLockLast);

			GuiUtils.FixHeightVs(this.cboSpeedResolution, this.lblSpeedResolution);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}

		public void LoadPhase()
		{
			m_lockCoordUpdate = true;

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			MinimumSize = new Size(m_windowMinimumWidth, m_windowMinimumHeight);

			KeyPreview = true;  // 2.10.1

			m_formReady = false;

			Visible = false;

			//base.OnLoad(e);

			CommonInit("");

			Icon = m_iconGray;

			if (Platform.Instance.IsWindowsSystem())
			{
				m_windowsNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
				m_windowsNotifyIcon.Icon = m_iconGray;
				m_windowsNotifyIcon.Text = Constants.Name;
				m_windowsNotifyIcon.Visible = Engine.Instance.Storage.GetBool("gui.tray_show");
				m_windowsNotifyIcon.BalloonTipTitle = Constants.Name;

				m_windowsNotifyIcon.MouseDoubleClick += new MouseEventHandler(notifyIcon_MouseDoubleClick);
				//m_windowNotifyIcon.Click += new EventHandler(notifyIcon_Click);				
				m_windowsNotifyIcon.ContextMenuStrip = mnuMain;
			}



			m_tabMain = new TabNavigator();
			m_tabMain.TitleRightBottom = Constants.VersionDesc;
			m_tabMain.ImportTabControl(tabMain);
			if (m_tabMain.Pages.Count != 0)
			{
				m_tabMain.Pages[0].Icon = "maintab_overview";
				m_tabMain.Pages[1].Icon = "maintab_providers";
				m_tabMain.Pages[2].Icon = "maintab_servers";
				m_tabMain.Pages[3].Icon = "maintab_countries";
				m_tabMain.Pages[4].Icon = "maintab_speed";
				m_tabMain.Pages[5].Icon = "maintab_stats";
				m_tabMain.Pages[6].Icon = "maintab_logs";

				m_tabMain.TabsFont = Skin.FontBig;
				m_tabMain.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
				m_tabMain.TabSwitch += tabMain_TabSwitch;
				this.Controls.Add(m_tabMain);

				m_cmdAbout = new Eddie.Forms.Skin.Button();
				m_cmdAbout.Name = "cmdAbout";
				m_cmdAbout.BackColor = Form.Skin.GetColor("color.tab.normal.background");
				m_cmdAbout.BackgroundImageLayout = ImageLayout.Stretch;
				m_cmdAbout.FlatStyle = FlatStyle.Flat;
				m_cmdAbout.Left = m_tabMain.GetTabsRect().Width / 2 - 40;
				m_cmdAbout.Top = m_tabMain.Height - 60;
				m_cmdAbout.Width = 32;
				m_cmdAbout.Height = 32;
				m_cmdAbout.Image = global::Eddie.Forms.Properties.Resources.tab_about;
				m_cmdAbout.ImageHover = global::Eddie.Forms.Properties.Resources.tab_about_hover;
				m_cmdAbout.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
				m_cmdAbout.Click += mnuAbout_Click;
				m_cmdAbout.ImageInflatePerc = 0;
				m_cmdAbout.DrawBorder = false;
				m_tabMain.Controls.Add(m_cmdAbout);

				m_cmdPreferences = new Eddie.Forms.Skin.Button();
				m_cmdPreferences.Name = "cmdPreferences";
				m_cmdPreferences.BackColor = Color.FromArgb(112, 184, 253);
				m_cmdPreferences.Left = m_tabMain.GetTabsRect().Width / 2 + 40;
				m_cmdPreferences.Top = m_tabMain.Height - 60;
				m_cmdPreferences.Width = 32;
				m_cmdPreferences.Height = 32;
				m_cmdPreferences.Image = global::Eddie.Forms.Properties.Resources.tab_preferences;
				m_cmdPreferences.ImageHover = global::Eddie.Forms.Properties.Resources.tab_preferences_hover;
				m_cmdPreferences.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
				m_cmdPreferences.Click += mnuSettings_Click;
				m_cmdPreferences.ImageInflatePerc = 0;
				m_cmdPreferences.DrawBorder = false;
				m_tabMain.Controls.Add(m_cmdPreferences);
			}

			m_imgProgressInfinite = new ProgressInfinite();
			this.pnlWaiting.Controls.Add(m_imgProgressInfinite);

			chkRemember.BackColor = Color.Transparent;

			if (Platform.IsWindows())
			{
				// TOFIX: Under Mono crash...
				m_toolTip = new Controls.ToolTip();

				Controls.Add(m_toolTip);
			}

			m_pnlCharts = new ChartSpeed();
			m_pnlCharts.Left = holSpeedChart.Left;
			m_pnlCharts.Top = holSpeedChart.Top;
			m_pnlCharts.Width = holSpeedChart.Width;
			m_pnlCharts.Height = holSpeedChart.Height;
			m_pnlCharts.Anchor = holSpeedChart.Anchor;
			holSpeedChart.Visible = false;
			if (m_tabMain.Pages.Count != 0)
				m_tabMain.Pages[4].Controls.Add(m_pnlCharts);


			m_cmdMainMenu = new MenuButton();
			m_cmdMainMenu.Left = 0;
			m_cmdMainMenu.Top = 0;
			m_cmdMainMenu.Click += cmdMenu_Click;
			Controls.Add(m_cmdMainMenu);


			// Providers
			foreach (Core.Provider provider in Engine.Instance.ProvidersManager.Providers)
			{
				Controls.ListViewItemProvider itemProvider = new Controls.ListViewItemProvider();
				itemProvider.Provider = provider;
				itemProvider.Update();

				lstProviders.Items.Add(itemProvider);
			}
			lstProviders.ResizeColumnsAuto();

			m_listViewServers = new ListViewServers();
			m_listViewServers.ContextMenuStrip = mnuServers;
			m_listViewServers.Dock = DockStyle.Fill;
			m_listViewServers.ResizeColumnString(0, 20);
			m_listViewServers.ResizeColumnString(1, 5);
			m_listViewServers.ResizeColumnString(2, 20);
			m_listViewServers.ResizeColumnString(3, 8);
			m_listViewServers.ResizeColumnString(4, 20);
			m_listViewServers.ResizeColumnString(5, 5);
			m_listViewServers.AllowColumnReorder = true;
			pnlServers.Controls.Add(m_listViewServers);

			m_listViewAreas = new ListViewAreas();
			m_listViewAreas.ContextMenuStrip = mnuAreas;
			m_listViewAreas.Dock = DockStyle.Fill;
			m_listViewAreas.ResizeColumnString(0, 14);
			m_listViewAreas.ResizeColumnString(1, 8);
			m_listViewAreas.ResizeColumnString(2, 20);
			m_listViewAreas.ResizeColumnString(3, 6);
			m_listViewAreas.AllowColumnReorder = true;
			pnlAreas.Controls.Add(m_listViewAreas);

			m_listViewServers.MouseDoubleClick += new MouseEventHandler(m_listViewServers_MouseDoubleClick);
			m_listViewServers.SelectedIndexChanged += new EventHandler(m_listViewServers_SelectedIndexChanged);
			m_listViewAreas.SelectedIndexChanged += new EventHandler(m_listViewAreas_SelectedIndexChanged);

			lstStats.ImageIconResourcePrefix = "stats_";
			lstStats.ResizeColumnMax(1);

			lstLogs.ImageIconResourcePrefix = "log_";
			lstLogs.ResizeColumnString(0, 2);
			lstLogs.ResizeColumnString(1, LogEntry.GetDateForListSample());
			lstLogs.ResizeColumnMax(2);

			chkShowAll.Checked = false;
			chkLockLast.Checked = Engine.Storage.GetBool("servers.locklast");
			cboScoreType.Text = Engine.Storage.Get("servers.scoretype");


			//ApplySkin();

			bool forceMinimized = false;
			if (Engine.Storage.GetBool("gui.start_minimized"))
				forceMinimized = true;
			if ((m_windowStateSetByShortcut) && (WindowState == FormWindowState.Minimized))
				forceMinimized = true;
			bool forceMaximized = false;
			if ((m_windowStateSetByShortcut) && (WindowState == FormWindowState.Maximized))
				forceMaximized = true;
			SetFormLayout(Engine.Storage.Get("gui.window.main"), forceMinimized, forceMaximized, Engine.AllowMinimizeInTray(), new Size(m_windowDefaultWidth, m_windowDefaultHeight));
			m_listViewServers.SetUserPrefs(Engine.Storage.Get("gui.list.servers"));
			m_listViewAreas.SetUserPrefs(Engine.Storage.Get("gui.list.areas"));
			lstLogs.SetUserPrefs(Engine.Storage.Get("gui.list.logs"));

			foreach (StatsEntry statsEntry in Engine.Stats.List)
			{
				ListViewItemStats statsEntryItem = new ListViewItemStats();
				statsEntryItem.Entry = statsEntry;
				statsEntryItem.Text = statsEntry.Caption;
				statsEntryItem.ImageKey = statsEntry.Icon;

				lstStats.Items.Add(statsEntryItem);
				m_statsItems[statsEntry.Key] = statsEntryItem;

				OnStatsChange(statsEntry); // Without this, glitch in listview under Linux                
			}

			lstStats.ResizeColumnAuto(0);

			cboSpeedResolution.Items.Clear();
			cboSpeedResolution.Items.Add(Messages.WindowsMainSpeedResolution1);
			cboSpeedResolution.Items.Add(Messages.WindowsMainSpeedResolution2);
			cboSpeedResolution.Items.Add(Messages.WindowsMainSpeedResolution3);
			cboSpeedResolution.Items.Add(Messages.WindowsMainSpeedResolution4);
			cboSpeedResolution.Items.Add(Messages.WindowsMainSpeedResolution5);
			cboSpeedResolution.SelectedIndex = 0;

			// Tooltips
			cmdConnect.Text = Messages.CommandConnect;
			lblConnectSubtitle.Text = Messages.CommandConnectSubtitle;
			cmdDisconnect.Text = Messages.CommandDisconnect;
			cmdCancel.Text = Messages.CommandCancel;

			if (m_toolTip != null)
			{
				m_toolTip.Connect(this.cboScoreType, Messages.TooltipServersScoreType);
				m_toolTip.Connect(this.chkLockLast, Messages.TooltipServersLockCurrent);
				m_toolTip.Connect(this.chkShowAll, Messages.TooltipServersShowAll);
				m_toolTip.Connect(this.cboScoreType, Messages.TooltipServersScoreType);
				m_toolTip.Connect(this.chkLockLast, Messages.TooltipServersLockCurrent);
				m_toolTip.Connect(this.chkShowAll, Messages.TooltipServersShowAll);
				m_toolTip.Connect(this.cmdServersConnect, Messages.TooltipServersConnect);
				m_toolTip.Connect(this.cmdServersUndefined, Messages.TooltipServersUndefined);
				m_toolTip.Connect(this.cmdServersBlackList, Messages.TooltipServersBlackList);
				m_toolTip.Connect(this.cmdServersWhiteList, Messages.TooltipServersWhiteList);
				m_toolTip.Connect(this.cmdServersRename, Messages.TooltipServersRename);
				m_toolTip.Connect(this.cmdServersMore, Messages.TooltipServersMore);
				m_toolTip.Connect(this.cmdServersRefresh, Messages.TooltipServersRefresh);
				m_toolTip.Connect(this.cmdAreasUndefined, Messages.TooltipAreasUndefined);
				m_toolTip.Connect(this.cmdAreasBlackList, Messages.TooltipAreasBlackList);
				m_toolTip.Connect(this.cmdAreasWhiteList, Messages.TooltipAreasWhiteList);
				m_toolTip.Connect(this.cmdLogsCommand, Messages.TooltipLogsCommand);
				m_toolTip.Connect(this.cmdLogsClean, Messages.TooltipLogsClean);
				m_toolTip.Connect(this.cmdLogsCopy, Messages.TooltipLogsCopy);
				m_toolTip.Connect(this.cmdLogsSave, Messages.TooltipLogsSave);
				m_toolTip.Connect(this.cmdLogsSupport, Messages.TooltipLogsSupport);

				Controls.SetChildIndex(m_toolTip, 0);
			}

			// Start
			if (Engine.Storage.GetBool("remember"))
			{
				chkRemember.Checked = true;
				txtLogin.Text = Engine.Storage.Get("login");
				txtPassword.Text = Engine.Storage.Get("password");
			}

			m_lockCoordUpdate = false;

			Resizing();

			VisibilityChange();

			// base.OnLoad(e); // Removed in 2.11.9

			m_formReady = true;

			Engine.OnRefreshUi();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
		}

		protected override void WndProc(ref Message m)
		{
			/*WM_SIZE*/
			if (m.Msg == 0x0005)
			{
				// This will be set to true if the shortcut uses the Maximized or Minimized
				// options because then it runs before OnLoad.
				m_windowStateSetByShortcut = true;
			}
			else
				base.WndProc(ref m);
		}

		protected override void OnKeyDown(KeyEventArgs e) // 2.10.1
		{
			base.OnKeyDown(e);

			if (e.Control && e.KeyCode == Keys.M)
			{
				OnShowMenu();
			}

			if (e.Control && e.KeyCode == Keys.A)
			{
				OnShowAbout();
			}

			if (e.Control && e.KeyCode == Keys.P)
			{
				OnShowPreferences();
			}

			if (e.Control && e.Alt && e.KeyCode == Keys.O)
			{
				m_tabMain.SelectTab("overview");
			}

			if (e.Control && e.Alt && e.KeyCode == Keys.S)
			{
				m_tabMain.SelectTab("servers");
			}

			if (e.Control && e.Alt && e.KeyCode == Keys.C)
			{
				m_tabMain.SelectTab("countries");
			}

			if (e.Control && e.Alt && e.KeyCode == Keys.V)
			{
				m_tabMain.SelectTab("speed");
			}

			if (e.Control && e.Alt && e.KeyCode == Keys.I)
			{
				m_tabMain.SelectTab("stats");
			}

			if (e.Control && e.Alt && e.KeyCode == Keys.L)
			{
				m_tabMain.SelectTab("logs");
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				// Another Mono Workaround. On some system, randomly ignore resized controls.
				if (Platform.IsUnix())
				{
					if (m_tabMain.Width != ClientSize.Width)
					{
						Resizing();
					}
				}

				Skin.GraphicsCommon(e.Graphics);

				int iconHeight = m_topHeaderHeight;
				int iconDistance = 3;

				//Rectangle rectHeader = new Rectangle(m_cmdMainMenu.Width, 0, ClientSize.Width - m_cmdMainMenu.Width, m_topHeaderHeight);
				//Rectangle rectHeaderText = new Rectangle(m_cmdMainMenu.Width, 0, ClientSize.Width - m_cmdMainMenu.Width - iconDistance - iconHeight, m_topHeaderHeight);
				Rectangle rectHeader = new Rectangle(0, 0, ClientSize.Width, m_topHeaderHeight);
				Rectangle rectHeaderText = new Rectangle(0, 0, ClientSize.Width - iconDistance - iconHeight, m_topHeaderHeight);

				Form.DrawImage(e.Graphics, Skin.MainBackImage, new Rectangle(0, 0, ClientSize.Width, m_topHeaderHeight));

				Image iconFlag = null;
				if (Engine.CurrentServer != null)
				{
					string iconFlagCode = Engine.CurrentServer.CountryCode;
					if (imgCountries.Images.ContainsKey(iconFlagCode))
						iconFlag = imgCountries.Images[iconFlagCode];

					if (iconFlag != null)
					{
						rectHeaderText.Width -= iconHeight;
						rectHeaderText.Width -= iconDistance;
					}
				}

				if (Engine.IsWaiting())
				{
					DrawImage(e.Graphics, GuiUtils.GetResourceImage("topbar_yellow"), rectHeader);
					Form.DrawStringOutline(e.Graphics, Engine.WaitMessage, Skin.FontBig, Skin.ForeBrush, rectHeaderText, GuiUtils.StringFormatRightMiddle);
				}
				else if (Engine.IsConnected())
				{
					string serverName = Engine.CurrentServer.DisplayName;

					DrawImage(e.Graphics, GuiUtils.GetResourceImage("topbar_green"), rectHeader);

					Form.DrawStringOutline(e.Graphics, MessagesFormatter.Format(MessagesUi.TopBarConnected, serverName), Skin.FontBig, Skin.ForeBrush, rectHeaderText, GuiUtils.StringFormatRightMiddle);
				}
				else
				{
					DrawImage(e.Graphics, GuiUtils.GetResourceImage("topbar_red"), rectHeader);
					if ((Engine.Instance.NetworkLockManager != null) && (Engine.Instance.NetworkLockManager.IsActive()))
					{
						Form.DrawStringOutline(e.Graphics, MessagesUi.TopBarNotConnectedLocked, Skin.FontBig, Skin.ForeBrush, rectHeaderText, GuiUtils.StringFormatRightMiddle);
					}
					else
					{
						Form.DrawStringOutline(e.Graphics, MessagesUi.TopBarNotConnectedExposed, Skin.FontBig, Skin.ForeBrush, rectHeaderText, GuiUtils.StringFormatRightMiddle);
					}
				}

				Rectangle rectNetLock = new Rectangle(rectHeader.Right - iconHeight, 0, iconHeight, iconHeight);
				Image iconNetLock = null;
				if ((Engine.Instance.NetworkLockManager != null) && (Engine.Instance.NetworkLockManager.IsActive()))
				{
					iconNetLock = m_bitmapNetlockStatusOn;
				}
				else
				{
					iconNetLock = m_bitmapNetlockStatusOff;
				}
				DrawImageContain(e.Graphics, iconNetLock, rectNetLock, 20);

				if (iconFlag != null)
				{
					Rectangle rectFlag = new Rectangle(rectHeader.Right - iconHeight - iconDistance - iconHeight, 0, iconHeight, iconHeight);
					DrawImageContain(e.Graphics, iconFlag, rectFlag, 20);
				}

				DrawImage(e.Graphics, GuiUtils.GetResourceImage("topbar_shadow"), rectHeader);
			}
			catch
			{
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (m_closing)
				return;

			e.Cancel = true;

			if (Engine.AskExitConfirm())
			{
				if (AskYesNo(Messages.ExitConfirm) != true)
				{
					Engine.OnExitRejected();
					return;
				}
			}

			Eddie.Forms.Engine engine = Engine.Instance as Eddie.Forms.Engine;

			if (engine.FormMain != null)
			{
				engine.Storage.Set("gui.window.main", engine.FormMain.GetFormLayout());
				engine.Storage.Set("gui.list.servers", m_listViewServers.GetUserPrefs());
				engine.Storage.Set("gui.list.areas", m_listViewAreas.GetUserPrefs());
				engine.Storage.Set("gui.list.logs", lstLogs.GetUserPrefs());
			}

			Engine.RequestStop();

			base.OnClosing(e);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if ((FormWindowState.Minimized == WindowState) && (Engine.AllowMinimizeInTray()) && (ShowInTaskbar))
			{
				ShowInTaskbar = false;
				Hide();
				EnabledUi();

				VisibilityChange();
			}
			else
			{
				if (ShowInTaskbar == false)
					ShowInTaskbar = true;
			}

			Resizing();
		}

		#region UI Controls Events

		void notifyIcon_Click(object sender, EventArgs e)
		{
			OnShowMenu();
		}

		void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Restore();
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			OnShowAbout();
		}

		private void mnuRestore_Click(object sender, EventArgs e)
		{
			OnMenuRestore();
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			OnMenuExit();
		}

		private void mnuPorts_Click(object sender, EventArgs e)
		{
			Core.UI.App.OpenUrl("https://airvpn.org/ports/");
		}

		private void mnuUser_Click(object sender, EventArgs e)
		{
			Core.UI.App.OpenUrl("https://airvpn.org/client/");
		}

		private void mnuHomePage_Click(object sender, EventArgs e)
		{
			Core.UI.App.OpenUrl(Core.UI.App.Manifest["links"]["help"]["website"].Value as string);
		}

		private void mnuSettings_Click(object sender, EventArgs e)
		{
			OnShowPreferences();
		}

		private void mnuStatus_Click(object sender, EventArgs e)
		{
			OnMenuStatus();
		}

		private void mnuConnect_Click(object sender, EventArgs e)
		{
			OnMenuConnect();
		}

		private void chkRemember_CheckedChanged(object sender, EventArgs e)
		{
			Engine.Storage.SetBool("remember", chkRemember.Checked);
		}

		private void cmdLogin_Click(object sender, EventArgs e)
		{
			if (Engine.IsLogged() == false)
				Login();
			else
				Logout();
		}

		private void txtLogin_TextChanged(object sender, EventArgs e)
		{
			//Engine.OnRefreshUi();
			EnabledUi();
		}

		private void txtPassword_TextChanged(object sender, EventArgs e)
		{
			//Engine.OnRefreshUi();
			EnabledUi();
		}


		private void cmdConnect_Click(object sender, EventArgs e)
		{
			Connect();
		}


		private void cmdLockedNetwork_Click(object sender, EventArgs e)
		{
			if (Engine.Instance.NetworkLockManager.IsActive())
				NetworkLockDeactivation();
			else
				NetworkLockActivation();
		}

		private void cmdDisconnect_Click(object sender, EventArgs e)
		{
			Disconnect();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Disconnect();
		}

		private void cmdMenu_Click(object sender, EventArgs e)
		{
			OnShowMenu();
		}

		private void mnuToolsPortForwarding_Click(object sender, EventArgs e)
		{
			/*
			Forms.PortForwarding Dlg = new PortForwarding();
			Dlg.ShowDialog();
			*/
		}

		private void mnuToolsNetworkMonitor_Click(object sender, EventArgs e)
		{
			/*
			Forms.NetworkMonitor Dlg = new NetworkMonitor();
			Dlg.ShowDialog();
			*/
		}

		private void mnuDevelopersNetworkMonitor_Click(object sender, EventArgs e)
		{
			/*
			 * NetworkMonitor dlg = new NetworkMonitor();
			dlg.Show();
			*/
		}


		private void mnuDevelopersManText_Click(object sender, EventArgs e)
		{
			Forms.TextViewer Dlg = new TextViewer();
			Dlg.Title = "Man";
			Dlg.Body = Engine.Instance.Storage.GetMan("text");
			Dlg.ShowDialog(this);
		}

		private void mnuDevelopersManBBCode_Click(object sender, EventArgs e)
		{
			Forms.TextViewer Dlg = new TextViewer();
			Dlg.Title = "Man";
			Dlg.Body = Engine.Instance.Storage.GetMan("bbc");
			Dlg.ShowDialog(this);
		}

		private void mnuDevelopersReset_Click(object sender, EventArgs e)
		{
			Dictionary<string, ConnectionInfo> servers;
			lock (Engine.Connections)
				servers = new Dictionary<string, ConnectionInfo>(Engine.Connections);
			foreach (ConnectionInfo infoServer in servers.Values)
			{
				infoServer.PingTests = 0;
				infoServer.PingFailedConsecutive = 0;
				infoServer.Ping = -1;
				infoServer.LastPingTest = 0;
				infoServer.LastPingResult = 0;
				infoServer.LastPingSuccess = 0;
			}

			Engine.OnRefreshUi();
		}

		private void lstProviders_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnabledUi();
		}

		private void lstProviders_DoubleClick(object sender, EventArgs e)
		{
			if (cmdProviderEdit.Enabled)
				cmdProviderEdit_Click(sender, e);
		}

		private void cmdProviderAdd_Click(object sender, EventArgs e)
		{
			WindowProviderAdd dlg = new WindowProviderAdd();
			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				Core.Provider provider = Engine.Instance.ProvidersManager.AddProvider(dlg.Provider, null);
				Engine.Instance.ProvidersManager.Refresh();

				Controls.ListViewItemProvider itemProvider = new Controls.ListViewItemProvider();
				itemProvider.Provider = provider;
				itemProvider.Update();

				lstProviders.Items.Add(itemProvider);

				lstProviders.ResizeColumnsAuto();

				EnabledUi();
			}
		}

		private void cmdProviderRemove_Click(object sender, EventArgs e)
		{
			if (lstProviders.SelectedItems.Count > 0)
			{
				Controls.ListViewItemProvider item = lstProviders.SelectedItems[0] as Controls.ListViewItemProvider;

				Engine.Instance.ProvidersManager.Remove(item.Provider);

				lstProviders.Items.Remove(item);

				Engine.Instance.ProvidersManager.Refresh();

				EnabledUi();
			}
		}

		private void cmdProviderEdit_Click(object sender, EventArgs e)
		{
			if (lstProviders.SelectedItems.Count != 1)
				return;

			Controls.ListViewItemProvider item = lstProviders.SelectedItems[0] as Controls.ListViewItemProvider;

			Eddie.Forms.Form form = null;
			if (item.Provider is Core.Providers.OpenVPN)
			{
				WindowProviderEditOpenVPN dlg = new WindowProviderEditOpenVPN();
				form = dlg;
				dlg.Provider = item.Provider as Core.Providers.OpenVPN;
			}
			else if (item.Provider is Core.Providers.Service)
			{
				WindowProviderEditManifest dlg = new WindowProviderEditManifest();
				form = dlg;
				dlg.Provider = item.Provider as Core.Providers.Service;
			}
			if (form != null)
			{
				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{
					item.Update();

					Engine.Instance.ProvidersManager.Refresh();

					EnabledUi();
				}
			}
		}

		void m_listViewServers_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			ConnectManual();
		}

		private void mnuServersConnect_Click(object sender, EventArgs e)
		{
			ConnectManual();
		}

		private void cmdServersConnect_Click(object sender, EventArgs e)
		{
			mnuServersConnect_Click(sender, e);
		}

		private void DeselectServersListItem()
		{
			if (Platform.Instance.IsWindowsSystem() == false)
			{
				// To avoid a Mono ListView crash, reproducible by whitelisting servers
				foreach (ListViewItemServer item in m_listViewServers.Items)
				{
					item.Focused = false;
					item.Selected = false;
				}
			}
		}

		private void mnuServersWhitelist_Click(object sender, EventArgs e)
		{
			foreach (ListViewItemServer item in m_listViewServers.SelectedItems)
			{
				item.Info.UserList = ConnectionInfo.UserListType.WhiteList;
			}
			Engine.UpdateSettings();
			DeselectServersListItem();
			//m_listViewServers.UpdateList();
			Engine.OnRefreshUi();
		}

		private void mnuServersBlacklist_Click(object sender, EventArgs e)
		{
			foreach (ListViewItemServer item in m_listViewServers.SelectedItems)
			{
				item.Info.UserList = ConnectionInfo.UserListType.BlackList;
			}
			Engine.UpdateSettings();
			DeselectServersListItem();
			//m_listViewServers.UpdateList();
			Engine.OnRefreshUi();
		}

		private void mnuServersUndefined_Click(object sender, EventArgs e)
		{
			foreach (ListViewItemServer item in m_listViewServers.SelectedItems)
			{
				item.Info.UserList = ConnectionInfo.UserListType.None;
			}
			Engine.UpdateSettings();
			DeselectServersListItem();
			//m_listViewServers.UpdateList();			
			Engine.OnRefreshUi();
		}

		private void mnuServersMore_Click(object sender, EventArgs e)
		{
			if (m_listViewServers.SelectedItems.Count == 1)
			{
				ListViewItemServer item = m_listViewServers.SelectedItems[0] as ListViewItemServer;
				WindowConnection dlg = new WindowConnection();
				dlg.Connection = item.Info;
				dlg.ShowDialog(this);
			}
		}

		private void mnuServersRename_Click(object sender, EventArgs e)
		{
			if (m_listViewServers.SelectedItems.Count == 1)
			{
				ListViewItemServer item = m_listViewServers.SelectedItems[0] as ListViewItemServer;

				WindowConnectionRename dlg = new WindowConnectionRename();
				dlg.Body = item.Info.DisplayName;
				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					item.Info.DisplayName = dlg.Body;
					item.Info.Provider.OnChangeConnection(item.Info);
					item.Update();
				}
			}
		}

		private void mnuServersRefresh_Click(object sender, EventArgs e)
		{
			mnuServersRefresh.Enabled = false;
			cmdServersRefresh.Enabled = false;

			Engine.Instance.RefreshInvalidateConnections();
		}

		private void cmdServersWhiteList_Click(object sender, EventArgs e)
		{
			mnuServersWhitelist_Click(sender, e);
		}

		private void cmdServersBlackList_Click(object sender, EventArgs e)
		{
			mnuServersBlacklist_Click(sender, e);
		}

		private void cmdServersUndefined_Click(object sender, EventArgs e)
		{
			mnuServersUndefined_Click(sender, e);
		}

		private void cmdServersRename_Click(object sender, EventArgs e)
		{
			mnuServersRename_Click(sender, e);
		}

		private void cmdServersMore_Click(object sender, EventArgs e)
		{
			mnuServersMore_Click(sender, e);
		}

		private void cmdServersRefresh_Click(object sender, EventArgs e)
		{
			mnuServersRefresh_Click(sender, e);
		}

		private void mnuAreasWhiteList_Click(object sender, EventArgs e)
		{
			foreach (ListViewItemArea item in m_listViewAreas.SelectedItems)
			{
				item.Info.UserList = AreaInfo.UserListType.WhiteList;
			}
			Engine.UpdateSettings();
			//m_listViewAreas.UpdateList();
			//m_listViewServers.UpdateList();
			Engine.OnRefreshUi();
		}

		private void mnuAreasBlackList_Click(object sender, EventArgs e)
		{
			foreach (ListViewItemArea item in m_listViewAreas.SelectedItems)
			{
				item.Info.UserList = AreaInfo.UserListType.BlackList;
			}
			Engine.UpdateSettings();
			//m_listViewAreas.UpdateList();
			//m_listViewServers.UpdateList();
			Engine.OnRefreshUi();
		}

		private void mnuAreasUndefined_Click(object sender, EventArgs e)
		{
			foreach (ListViewItemArea item in m_listViewAreas.SelectedItems)
			{
				item.Info.UserList = AreaInfo.UserListType.None;
			}
			Engine.UpdateSettings();
			//m_listViewAreas.UpdateList();
			//m_listViewServers.UpdateList();
			Engine.OnRefreshUi();
		}


		private void cmdAreasWhiteList_Click(object sender, EventArgs e)
		{
			mnuAreasWhiteList_Click(sender, e);
		}

		private void cmdAreasBlackList_Click(object sender, EventArgs e)
		{
			mnuAreasBlackList_Click(sender, e);
		}

		private void cmdAreasUndefined_Click(object sender, EventArgs e)
		{
			mnuAreasUndefined_Click(sender, e);
		}

		private void chkShowAll_CheckedChanged(object sender, EventArgs e)
		{
			m_listViewServers.ShowAll = chkShowAll.Checked;
			//m_listViewServers.UpdateList();
			Engine.OnRefreshUi();
		}

		void m_listViewServers_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnabledUi();
		}

		void m_listViewAreas_SelectedIndexChanged(object sender, EventArgs e)
		{
			EnabledUi();
		}

		private void chkLockCurrent_CheckedChanged(object sender, EventArgs e)
		{
			Engine.Storage.SetBool("servers.locklast", chkLockLast.Checked);
		}

		private void lstStats_DoubleClick(object sender, EventArgs e)
		{
			if (lstStats.SelectedItems.Count != 1)
				return;

			ListViewItemStats item = lstStats.SelectedItems[0] as ListViewItemStats;

			Core.UI.App.OpenStats(item.Entry.Key.ToLowerInvariant());
		}

		private void tabMain_TabSwitch()
		{
			// Under Linux, to close context men�.
			if (mnuAreas.Visible)
				mnuAreas.Close();
			if (mnuServers.Visible)
				mnuServers.Close();
		}

		private void cboScoreType_SelectionChangeCommitted(object sender, EventArgs e)
		{
			Engine.Storage.Set("servers.scoretype", cboScoreType.Text);

			OnRefreshUi(Core.Engine.RefreshUiMode.Full);
		}

		private void mnuContextCopyAll_Click(object sender, EventArgs e)
		{
			LogsDoCopy(false);
		}

		private void mnuContextSaveAll_Click(object sender, EventArgs e)
		{
			LogsDoSave(false);
		}

		private void mnuContextCopySelected_Click(object sender, EventArgs e)
		{
			LogsDoCopy(true);
		}

		private void mnuContextSaveSelected_Click(object sender, EventArgs e)
		{
			LogsDoSave(true);
		}

		private void cmdLogsClean_Click(object sender, EventArgs e)
		{
			lstLogs.Items.Clear();
		}

		private void cmdLogsSave_Click(object sender, EventArgs e)
		{
			LogsDoSave(false);
		}

		private void cmdLogsCopy_Click(object sender, EventArgs e)
		{
			LogsDoCopy(false);
		}

		private void cmdLogsSupport_Click(object sender, EventArgs e)
		{
			Engine.GenerateSystemReport();
		}

		private void cmdLogsCommand_Click(object sender, EventArgs e)
		{
			WindowCommand Dlg = new WindowCommand();
			if (Dlg.ShowDialog(this) == DialogResult.OK)
			{
				Core.UI.App.RunCommandString(Dlg.Command);
			}
		}

		private void cboSpeedResolution_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_pnlCharts.Switch(cboSpeedResolution.SelectedIndex);
		}

		private void cboKey_SelectedIndexChanged(object sender, EventArgs e)
		{
			Engine.Instance.Storage.Set("key", cboKey.SelectedItem as string);
		}

		#endregion

		public bool AllowMinimizeInTray()
		{
			return (m_windowsNotifyIcon != null);
		}

		public void VisibilityChange()
		{
			(Engine.Instance as Eddie.Forms.Engine).OnChangeMainFormVisibility(this.Visible);

			if (Engine.Storage.GetBool("gui.tray_minimized"))
			{
				mnuRestore.Visible = true;
				mnuRestoreSep.Visible = true;

				if (this.Visible)
					mnuRestore.Text = Messages.WindowsMainHide;
				else
					mnuRestore.Text = Messages.WindowsMainShow;
			}
			else
			{
				mnuRestore.Visible = false;
				mnuRestoreSep.Visible = false;
			}
		}

		public void Resizing()
		{
			if (m_lockCoordUpdate)
				return;

			if (m_pnlCharts == null)
				return;

			if (Engine.Storage == null)
				return;

			Graphics g = this.CreateGraphics();

			m_topHeaderHeight = GuiUtils.GetFontSize(g, Skin.FontBig, MessagesUi.TopBarNotConnectedExposed).Height;
			if (m_topHeaderHeight < 30)
				m_topHeaderHeight = 30;

			m_tabMain.Left = 0;
			m_tabMain.Top = m_topHeaderHeight;
			m_tabMain.Width = this.ClientSize.Width;
			m_tabMain.Height = this.ClientSize.Height - m_topHeaderHeight;

			if (m_cmdMainMenu != null)
			{
				//m_cmdMainMenu.Width = m_topHeaderHeight * 150 / 30;
				m_cmdMainMenu.Width = m_topHeaderHeight;
				m_cmdMainMenu.Height = m_topHeaderHeight;
			}

			Size tabPageRectangle = m_tabMain.GetPageRect().Size;

			// Welcome items
			pnlWelcome.Left = tabPageRectangle.Width / 2 - pnlWelcome.Width / 2;
			pnlWelcome.Top = tabPageRectangle.Height / 2 - pnlWelcome.Height / 2;
			pnlConnected.Left = tabPageRectangle.Width / 2 - pnlConnected.Width / 2;
			pnlConnected.Top = tabPageRectangle.Height / 2 - pnlConnected.Height / 2;

			// Waiting items
			pnlWaiting.Left = 0;
			pnlWaiting.Top = 0;
			pnlWaiting.Width = tabPageRectangle.Width;
			pnlWaiting.Height = tabPageRectangle.Height;
			int imgProgressTop = (tabPageRectangle.Height / 2) - (13 / 2);
			Size imgProgressSize = new Size(208, 13);
			if (m_imgProgressInfinite != null)
			{
				m_imgProgressInfinite.Size = imgProgressSize;
				m_imgProgressInfinite.Left = (tabPageRectangle.Width / 2) - (208 / 2);
				m_imgProgressInfinite.Top = (tabPageRectangle.Height / 2) - (13 / 2);
			}
			cmdCancel.Width = tabPageRectangle.Width * 2 / 3;
			cmdCancel.Left = tabPageRectangle.Width / 2 - cmdCancel.Width / 2;
			cmdCancel.Top = tabPageRectangle.Height - cmdCancel.Height - 20;
			lblWait1.Left = 0;
			lblWait1.Top = 0;
			lblWait1.Width = tabPageRectangle.Width;
			lblWait1.Height = imgProgressTop - 10;
			lblWait2.Left = 0;
			lblWait2.Top = imgProgressTop + imgProgressSize.Height + 10;
			lblWait2.Width = tabPageRectangle.Width;
			lblWait2.Height = tabPageRectangle.Height - lblWait2.Top - 10 - cmdCancel.Height - 20;

			Invalidate();
		}

		public void Restore()
		{
			Show();
			WindowState = FormWindowState.Normal;
			Activate();
			EnabledUi();

			VisibilityChange();
		}

		public void ShowMenu()
		{
			Point p = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
			mnuMain.Show(p);
			/*
            if (mnuMain.Visible)
                mnuMain.Close();
            else
            {
                Point p = new Point(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
                mnuMain.Show(p);
            } 
			*/
		}

		public void Login()
		{
			Engine.Storage.Set("login", txtLogin.Text);
			Engine.Storage.Set("password", txtPassword.Text);

			if (TermsOfServiceCheck(false) == false)
				return;

			Engine.Login();
		}

		public void Logout()
		{
			Engine.Logout();
		}

		public void Connect()
		{
			m_tabMain.SelectTab(0);

			if ((Engine.CanConnect() == true) && (Engine.IsConnected() == false) && (Engine.IsWaiting() == false))
				Engine.Connect();
		}

		public void ConnectManual()
		{
			if (m_listViewServers.SelectedItems.Count == 1)
			{
				Eddie.Forms.Controls.ListViewItemServer listViewItem = m_listViewServers.SelectedItems[0] as Eddie.Forms.Controls.ListViewItemServer;

				if (listViewItem.Info.CanConnect())
				{
					Engine.NextServer = listViewItem.Info;

					Connect();
				}
			}
		}

		public void Disconnect()
		{
			cmdCancel.Enabled = false;

			Engine.Disconnect();
		}

		/* -----------------------------------------
         * Logging
         ---------------------------------------- */
		private delegate void LogDelegate(LogEntry l);

		public void Log(LogEntry l)
		{
			if (this.InvokeRequired)
			{

				LogDelegate inv = new LogDelegate(this.Log);

				this.BeginInvoke(inv, new object[] { l });
			}
			else
			{
				lock (this)
				{
					string Msg = l.Message;

					if (l.Type > LogType.Realtime)
					{
						ListViewItemLog Item = new ListViewItemLog();
						Item.ImageKey = l.Type.ToString().ToLowerInvariant();
						Item.Text = "";
						Item.SubItems.Add(l.GetDateForList());
						Item.SubItems.Add(l.GetMessageForList());
						Item.ToolTipText = l.Message;
						Item.Info = l;

						lstLogs.Items.Add(Item);
						Item.EnsureVisible();

						if (lstLogs.Items.Count >= Engine.Storage.GetInt("gui.log_limit"))
							lstLogs.Items.RemoveAt(0);
					}

					if (l.Type == LogType.Fatal)
					{
						MessageBox.Show(this, Msg, Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private delegate void SetStatusDelegate(string textFull, string textShort);
		public void SetStatus(string textFull, string textShort)
		{
			if (this.InvokeRequired)
			{
				SetStatusDelegate inv = new SetStatusDelegate(this.SetStatus);

				this.BeginInvoke(inv, new object[] { textFull, textShort });
			}
			else
			{
				Text = Constants.Name + " - " + textFull;

				{
					string t = textFull;
					if (t.IndexOf("\n") != -1)
						t = t.Substring(0, t.IndexOf("\n")).Trim();
					if (t.Length > 128)
						t = t.Substring(0, 128) + "...";
					mnuStatus.Text = "> " + t.Trim();
				}

				if (m_windowsNotifyIcon != null)
				{
					String tooltipText = textShort;
					if (tooltipText.Length > 120)
						tooltipText = tooltipText.Substring(0, 120) + "...";
					GuiUtils.SetNotifyIconText(m_windowsNotifyIcon, tooltipText);
				}
			}
		}

		private delegate void SetColorDelegate(string color);
		public void SetColor(string color)
		{
			if (this.InvokeRequired)
			{
				SetColorDelegate inv = new SetColorDelegate(this.SetColor);

				this.BeginInvoke(inv, new object[] { color });
			}
			else
			{
				Icon icon = m_iconGray;
				if (color == "green")
				{
					mnuStatus.Image = m_bitmapStatusGreen;
					icon = m_iconNormal;
					mnuConnect.Text = Messages.CommandDisconnect;
				}
				else if (color == "yellow")
				{
					mnuStatus.Image = m_bitmapStatusYellow;
					mnuConnect.Text = Messages.CommandCancel;
				}
				else
				{
					mnuStatus.Image = m_bitmapStatusRed;
					mnuConnect.Text = Messages.CommandConnect;
				}

				if (this.Icon != icon)
				{
					this.Icon = icon;
					if (m_windowsNotifyIcon != null)
						m_windowsNotifyIcon.Icon = icon;
				}
			}
		}

		private delegate void ShowWindowsNotificationDelegate(string level, string message);
		public void ShowWindowsNotification(string level, string message)
		{
			if (this.InvokeRequired)
			{
				ShowWindowsNotificationDelegate inv = new ShowWindowsNotificationDelegate(this.ShowWindowsNotification);

				this.BeginInvoke(inv, new object[] { level, message });
			}
			else
			{
				if (m_windowsNotifyIcon == null)
					return;

				if (level == "warning")
					m_windowsNotifyIcon.BalloonTipIcon = ToolTipIcon.Warning;
				else if (level == "error")
					m_windowsNotifyIcon.BalloonTipIcon = ToolTipIcon.Error;
				else if (level == "fatal")
					m_windowsNotifyIcon.BalloonTipIcon = ToolTipIcon.Error;
				else
					m_windowsNotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
				m_windowsNotifyIcon.BalloonTipText = message;
				m_windowsNotifyIcon.ShowBalloonTip(1000);
			}
		}

		private delegate void SwitchIconDelegate(string type);
		public void SwitchIcon(string type)
		{
			if (this.InvokeRequired)
			{
				SwitchIconDelegate inv = new SwitchIconDelegate(this.SwitchIcon);

				this.BeginInvoke(inv, new object[] { type });
			}
			else
			{
				Icon icon = m_iconGray;
				if (type == "connected")
					icon = m_iconNormal;

				if (this.Icon != icon)
				{
					this.Icon = icon;
					if (m_windowsNotifyIcon != null)
						m_windowsNotifyIcon.Icon = icon;
				}
			}
		}

		public void EnabledUi()
		{
			if (m_listViewServers == null) // 2.11.4
				return;

			// Main Menu
			mnuAbout.Enabled = (Core.UI.App.Manifest != null);
			mnuSettings.Enabled = (Core.UI.App.Manifest != null);
			mnuHomePage.Enabled = (Core.UI.App.Manifest != null);
			mnuUser.Enabled = (Core.UI.App.Manifest != null);
			mnuPorts.Enabled = (Core.UI.App.Manifest != null);

			m_cmdAbout.Enabled = (Core.UI.App.Manifest != null);
			m_cmdPreferences.Enabled = (Core.UI.App.Manifest != null);

			ConnectionInfo selectedConnection = null;
			if (m_listViewServers.SelectedItems.Count == 1)
				selectedConnection = (m_listViewServers.SelectedItems[0] as Controls.ListViewItemServer).Info;

			// Welcome			
			bool connected = Engine.IsConnected();
			bool waiting = Engine.IsWaiting();

			if (Engine.Instance.AirVPN != null)
			{
				lblLoginIcon.Visible = true;
				lblLogin.Visible = true;
				txtLogin.Visible = true;
				lblPassword.Visible = true;
				txtPassword.Visible = true;
				cmdLogin.Visible = true;
				chkRemember.Visible = true;

				bool airvpnLogged = Engine.IsLogged();

				if (airvpnLogged == false)
				{
					cmdLogin.Text = Messages.CommandLoginButton;
				}
				else
				{
					cmdLogin.Text = Messages.CommandLogout;
				}

				cmdLogin.Enabled = ((waiting == false) && (connected == false) && (txtLogin.Text.Trim() != "") && (txtPassword.Text.Trim() != ""));

				txtLogin.Enabled = (airvpnLogged == false);
				txtPassword.Enabled = (airvpnLogged == false);
				lblKey.Visible = ((airvpnLogged == true) && (cboKey.Items.Count > 1));
				cboKey.Visible = ((airvpnLogged == true) && (cboKey.Items.Count > 1));
			}
			else
			{
				lblLoginIcon.Visible = false;
				lblLogin.Visible = false;
				txtLogin.Visible = false;
				lblPassword.Visible = false;
				txtPassword.Visible = false;
				cmdLogin.Visible = false;
				lblKey.Visible = false;
				cboKey.Visible = false;
				chkRemember.Visible = false;
			}

			cmdConnect.Enabled = Engine.Instance.CanConnect();

			// Providers
			cmdProviderAdd.Enabled = true;
			cmdProviderRemove.Enabled = (lstProviders.SelectedItems.Count > 0);
			cmdProviderEdit.Enabled = (lstProviders.SelectedItems.Count == 1);

			// Connections
			cmdServersConnect.Enabled = ((selectedConnection != null) && (selectedConnection.CanConnect()));
			mnuServersConnect.Enabled = cmdServersConnect.Enabled;

			cmdServersWhiteList.Enabled = (m_listViewServers.SelectedItems.Count > 0);
			mnuServersWhiteList.Enabled = cmdServersWhiteList.Enabled;
			cmdServersBlackList.Enabled = cmdServersWhiteList.Enabled;
			mnuServersBlackList.Enabled = cmdServersBlackList.Enabled;
			cmdServersUndefined.Enabled = cmdServersWhiteList.Enabled;
			mnuServersUndefined.Enabled = cmdServersUndefined.Enabled;
			cmdServersMore.Enabled = (m_listViewServers.SelectedItems.Count == 1);
			mnuServersMore.Enabled = cmdServersMore.Enabled;

			cmdServersRename.Enabled = ((m_listViewServers.SelectedItems.Count == 1) && ((m_listViewServers.SelectedItems[0] as ListViewItemServer).Info.Provider is Core.Providers.OpenVPN));
			mnuServersRename.Enabled = cmdServersRename.Enabled;

			cmdAreasWhiteList.Enabled = (m_listViewAreas.SelectedItems.Count > 0);
			mnuAreasWhiteList.Enabled = cmdAreasWhiteList.Enabled;
			cmdAreasBlackList.Enabled = cmdAreasWhiteList.Enabled;
			mnuAreasBlackList.Enabled = cmdAreasBlackList.Enabled;
			cmdAreasUndefined.Enabled = cmdAreasWhiteList.Enabled;
			mnuAreasUndefined.Enabled = cmdAreasUndefined.Enabled;

			cmdLogsCommand.Visible = Engine.Storage.GetBool("advanced.expert");

			if ((Engine.Instance.NetworkLockManager != null) && (Engine.Instance.NetworkLockManager.IsActive()))
			{
				cmdLockedNetwork.Text = Messages.NetworkLockButtonActive;
				imgLockedNetwork.Image = m_bitmapNetlockOn;
			}
			else
			{
				cmdLockedNetwork.Text = Messages.NetworkLockButtonDeactive;
				imgLockedNetwork.Image = m_bitmapNetlockOff;
			}

			bool networkCanEnabled = ((Engine.Instance.NetworkLockManager != null) && (Engine.Instance.NetworkLockManager.CanEnabled()));
			cmdLockedNetwork.Visible = networkCanEnabled;
			imgLockedNetwork.Visible = networkCanEnabled;

			m_tabMain.SetPageVisible(1, Engine.Storage.GetBool("advanced.providers"));
		}

		// Force when need to update icons, force refresh etc.		
		private delegate void OnRefreshUiDelegate(Engine.RefreshUiMode mode);
		public void OnRefreshUi(Engine.RefreshUiMode mode)
		{
			if (this.IsHandleCreated == false)
				return;

			if (m_lockCoordUpdate)
				return;

			if (this.InvokeRequired)
			{
				OnRefreshUiDelegate inv = new OnRefreshUiDelegate(this.OnRefreshUi);

				//this.Invoke(inv, new object[] { mode });
				this.BeginInvoke(inv, new object[] { mode });
			}
			else
			{
				if (m_formReady == false) // To avoid useless calling that Windows.Forms do when initializing controls 
					return;

				// For refresh Mono-Linux
				if (Platform.Instance.IsLinuxSystem())
				{
					Invalidate();
					Update();
					Refresh();
				}

				// lock (Engine) // TOCLEAN 2.9
				{
					if ((mode == Core.Engine.RefreshUiMode.MainMessage) || (mode == Core.Engine.RefreshUiMode.Full))
					{
						// Status message
						String text1 = Engine.WaitMessage;
						lblWait1.Text = text1;

						if (Engine.IsWaiting())
						{
							pnlWelcome.Visible = false;
							pnlWaiting.Visible = true;
							pnlConnected.Visible = false;
							cmdCancel.Visible = Engine.IsWaitingCancelAllowed();
						}
						else if (Engine.IsConnected())
						{
							pnlWelcome.Visible = false;
							pnlWaiting.Visible = false;
							pnlConnected.Visible = true;

							lblConnectedServerName.Text = Engine.CurrentServer.DisplayName;
							lblConnectedLocation.Text = Engine.CurrentServer.GetLocationForList();
							txtConnectedExitIp.Text = Engine.ConnectionActive.ExitIPs.ToString();
							string iconFlagCode = Engine.CurrentServer.CountryCode;
							Image iconFlag = null;
							if (imgCountries.Images.ContainsKey(iconFlagCode))
							{
								iconFlag = imgCountries.Images[iconFlagCode];
								lblConnectedCountry.Image = iconFlag;
							}
							else
								lblConnectedCountry.Image = null;
						}
						else
						{
							pnlWelcome.Visible = true;
							pnlWaiting.Visible = false;
							pnlConnected.Visible = false;
						}

						cmdCancel.Enabled = (Engine.IsWaitingCancelPending() == false);
						mnuConnect.Enabled = cmdCancel.Enabled;

						// Repaint
						Invalidate();

						EnabledUi();
					}

					if ((mode == Core.Engine.RefreshUiMode.Log) || (mode == Core.Engine.RefreshUiMode.Full))
					{
						lock (Engine.LogEntries)
						{
							while (Engine.LogEntries.Count > 0)
							{
								LogEntry l = Engine.LogEntries[0];
								Engine.LogEntries.RemoveAt(0);

								Log(l);
							}
						}

						if (Engine.IsWaiting())
						{
							lblWait2.Text = Engine.Logs.GetLogDetailTitle();
						}
					}

					if ((mode == Core.Engine.RefreshUiMode.Stats) || (mode == Core.Engine.RefreshUiMode.Full))
					{
						if (Engine.IsConnected())
						{
							txtConnectedSince.Text = Engine.Stats.GetValue("VpnStart");

							txtConnectedDownload.Text = UtilsString.FormatBytes(Engine.ConnectionActive.BytesLastDownloadStep, true, false);
							txtConnectedUpload.Text = UtilsString.FormatBytes(Engine.ConnectionActive.BytesLastUploadStep, true, false);
						}
					}

					if (mode == Core.Engine.RefreshUiMode.Full)
					{
						m_listViewServers.UpdateList();
						m_listViewAreas.UpdateList();
					}
				}


			}
		}

		private delegate void OnStatsChangeDelegate(StatsEntry entry);
		public void OnStatsChange(StatsEntry entry)
		{
			if (this.InvokeRequired)
			{
				OnStatsChangeDelegate inv = new OnStatsChangeDelegate(this.OnStatsChange);
				this.BeginInvoke(inv, new object[] { entry });
			}
			else
			{
				if (m_statsItems.ContainsKey(entry.Key))
				{
					ListViewItemStats item = m_statsItems[entry.Key];
					if (item.SubItems.Count == 1)
						item.SubItems.Add("");
					item.SubItems[1].Text = entry.Text;
				}
			}
		}

		private delegate void OnProviderManifestFailedDelegate(Eddie.Core.Provider provider);
		public void OnProviderManifestFailed(Eddie.Core.Provider provider)
		{
			if (this.InvokeRequired)
			{
				OnProviderManifestFailedDelegate inv = new OnProviderManifestFailedDelegate(this.OnProviderManifestFailed);
				this.BeginInvoke(inv, new object[] { provider });
			}
			else
			{
				WindowProviderNoBootstrap dlg = new WindowProviderNoBootstrap();
				dlg.Provider = provider as Core.Providers.Service;
				dlg.ShowDialog(this);
			}
		}

		private delegate void OnFrontMessageDelegate(string message);
		public void OnFrontMessage(string message)
		{
			if (this.InvokeRequired)
			{
				OnFrontMessageDelegate inv = new OnFrontMessageDelegate(this.OnFrontMessage);

				//this.Invoke(inv, new object[] { mode });
				this.BeginInvoke(inv, new object[] { message });
			}
			else
			{
				Eddie.Forms.Forms.FrontMessage dlg = new Forms.FrontMessage();
				dlg.Message = message;
				dlg.Show();
				dlg.Activate();
			}
		}

		private delegate void OnMessageInfoDelegate(string message);
		public void OnMessageInfo(string message)
		{
			if (this.InvokeRequired)
			{
				OnMessageInfoDelegate inv = new OnMessageInfoDelegate(this.OnMessageInfo);
				this.Invoke(inv, new object[] { message });
			}
			else
			{
				MessageBox.Show(this, message, Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private delegate void OnMessageErrorDelegate(string message);
		public void OnMessageError(string message)
		{
			if (this.InvokeRequired)
			{
				OnMessageErrorDelegate inv = new OnMessageErrorDelegate(this.OnMessageError);
				this.Invoke(inv, new object[] { message });
			}
			else
			{
				MessageBox.Show(this, message, Constants.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private delegate void OnShowTextDelegate(string title, string data);
		public void OnShowText(string title, string data)
		{
			if (this.InvokeRequired)
			{
				OnShowTextDelegate inv = new OnShowTextDelegate(this.OnShowText);
				this.Invoke(inv, new object[] { title, data });
			}
			else
			{
				ShowText(this, title, data);
			}
		}

		public void ShowText(Form parent, string title, string data)
		{
			Forms.TextViewer Dlg = new Forms.TextViewer();
			Dlg.Title = title;
			Dlg.Body = data;
			Dlg.ShowDialog(parent);
		}

		private delegate bool AskYesNoDelegate(string message);
		public bool AskYesNo(string message)
		{
			if (this.InvokeRequired)
			{
				AskYesNoDelegate inv = new AskYesNoDelegate(this.AskYesNo);
				return (bool)this.Invoke(inv, new object[] { message });
			}
			else
			{
				return MessageBox.Show(this, message, Constants.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
			}
		}

		private delegate Credentials OnAskCredentialsDelegate();
		public Credentials OnAskCredentials()
		{
			if (this.InvokeRequired)
			{
				OnAskCredentialsDelegate inv = new OnAskCredentialsDelegate(this.OnAskCredentials);
				return this.Invoke(inv, new object[] { }) as Credentials;
			}
			else
			{
				Forms.WindowCredentials Dlg = new Forms.WindowCredentials();
				if (Dlg.ShowDialog(this) == DialogResult.OK)
					return Dlg.Credentials;
				else
					return null;
			}
		}

		private delegate void OnSystemReportDelegate(string step, string text, int perc);
		public void OnSystemReport(string step, string text, int perc)
		{
			if (this.InvokeRequired)
			{
				OnSystemReportDelegate inv = new OnSystemReportDelegate(this.OnSystemReport);
				this.Invoke(inv, new object[] { step, text, perc });
			}
			else
			{
				if ((m_windowReport == null) || (m_windowReport.IsDisposed == true))
				{
					m_windowReport = new Forms.WindowReport();
				}

				m_windowReport.Visible = true;
				m_windowReport.Activate();
				m_windowReport.Focus();
				m_windowReport.SetStep(step, text, perc);
			}
		}

		private delegate void OnLoggedUpdateDelegate(XmlElement xmlKeys);
		public void OnLoggedUpdate(XmlElement xmlKeys)
		{
			if (this.InvokeRequired)
			{
				OnLoggedUpdateDelegate inv = new OnLoggedUpdateDelegate(this.OnLoggedUpdate);

				this.BeginInvoke(inv, new object[] { xmlKeys });
			}
			else
			{
				cboKey.Items.Clear();
				foreach (XmlElement xmlKey in xmlKeys.ChildNodes)
				{
					cboKey.Items.Add(xmlKey.GetAttribute("name"));
				}

				if (cboKey.Items.Contains(Engine.Instance.Storage.Get("key")) == true)
				{
					cboKey.SelectedItem = Engine.Instance.Storage.Get("key");
				}
				else
				{
					if (cboKey.Items.Count > 0)
					{
						cboKey.SelectedIndex = 0;
						Engine.Instance.Storage.Set("key", cboKey.Items[0] as string);
					}
					else
					{
						Engine.Instance.Storage.Set("key", "");
					}
				}

			}
		}

		private delegate void OnPostManifestUpdateDelegate();
		public void OnPostManifestUpdate()
		{
			if (this.InvokeRequired)
			{
				OnPostManifestUpdateDelegate inv = new OnPostManifestUpdateDelegate(this.OnPostManifestUpdate);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				mnuServersRefresh.Enabled = true;
				cmdServersRefresh.Enabled = true;
			}
		}

		private delegate void OnMenuStatusDelegate();
		public void OnMenuStatus()
		{
			if (this.InvokeRequired)
			{
				OnMenuStatusDelegate inv = new OnMenuStatusDelegate(this.OnMenuStatus);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				Restore();
			}
		}

		private delegate void OnMenuConnectDelegate();
		public void OnMenuConnect()
		{
			if (this.InvokeRequired)
			{
				OnMenuConnectDelegate inv = new OnMenuConnectDelegate(this.OnMenuConnect);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				if (Engine.IsWaiting())
				{
					Disconnect();
				}
				else if (Engine.IsConnected())
				{
					Disconnect();
				}
				else if (Engine.CanConnect())
				{
					Connect();
				}
				else
				{
					Restore();
				}
			}
		}

		private delegate void OnMenuRestoreDelegate();
		public void OnMenuRestore()
		{
			if (this.InvokeRequired)
			{
				OnMenuRestoreDelegate inv = new OnMenuRestoreDelegate(this.OnMenuRestore);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				if (this.Visible == false)
					Restore();
				else
					this.WindowState = FormWindowState.Minimized;
			}
		}

		private delegate void OnMenuExitDelegate();
		public void OnMenuExit()
		{
			if (this.InvokeRequired)
			{
				OnMenuExitDelegate inv = new OnMenuExitDelegate(this.OnMenuExit);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				Close();
			}
		}

		private delegate void OnShowPreferencesDelegate();
		public void OnShowPreferences()
		{
			if (Core.UI.App.Manifest == null)
				return;

			if (this.InvokeRequired)
			{
				OnShowPreferencesDelegate inv = new OnShowPreferencesDelegate(this.OnShowPreferences);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				Forms.Settings Dlg = new Forms.Settings();
				Dlg.ShowDialog(this);

				EnabledUi();
			}
		}

		private delegate void OnShowAboutDelegate();
		public void OnShowAbout()
		{
			if (Core.UI.App.Manifest == null)
				return;

			if (this.InvokeRequired)
			{
				OnShowAboutDelegate inv = new OnShowAboutDelegate(this.OnShowAbout);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				Forms.About dlg = new Forms.About();
				dlg.ShowDialog(this);
			}
		}

		private delegate void OnShowMenuDelegate();
		public void OnShowMenu()
		{
			if (this.InvokeRequired)
			{
				OnShowMenuDelegate inv = new OnShowMenuDelegate(this.OnShowMenu);

				this.BeginInvoke(inv, new object[] { });
			}
			else
			{
				ShowMenu();
			}
		}




		public bool TermsOfServiceCheck(bool force)
		{
			bool show = force;
			if (show == false)
				show = (Engine.Storage.GetBool("gui.tos") == false);

			if (show)
			{
				Forms.Tos Dlg = new Forms.Tos();
				if (Dlg.ShowDialog(this) == DialogResult.OK)
				{
					Engine.Storage.SetBool("gui.tos", true);
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return true;
			}
		}

		private String LogsGetBody(bool selectedOnly)
		{
			lock (this)
			{
				StringBuilder buffer = new StringBuilder();

				for (int i = 0; i < lstLogs.Items.Count; i++)
				{
					bool skip = false;
					if ((selectedOnly) && (lstLogs.Items[i].Selected == false))
						skip = true;

					if (skip == false)
					{
						buffer.Append((lstLogs.Items[i] as ListViewItemLog).Info.GetStringLines() + "\n");
					}
				}

				return Platform.Instance.NormalizeString(buffer.ToString());
			}
		}

		private void LogsDoCopy(bool selectedOnly)
		{
			String t = LogsGetBody(selectedOnly);
			if (t.Trim() != "")
			{
				GuiUtils.ClipboardSetText(t);

				ShowMessageInfo(Messages.LogsCopyClipboardDone);
			}
		}

		private void LogsDoSave(bool selectedOnly)
		{
			String t = LogsGetBody(selectedOnly);
			if (t.Trim() != "")
			{
				using (SaveFileDialog sd = new SaveFileDialog())
				{
					sd.FileName = Engine.Logs.GetLogSuggestedFileName();
					sd.Filter = Messages.FilterTextFiles;

					if (sd.ShowDialog(this) == DialogResult.OK)
					{
						using (StreamWriter sw = new StreamWriter(sd.FileName))
						{
							sw.Write(t);
							sw.Flush();
							//sw.Close();	// because of "using"
						}

						ShowMessageInfo(Messages.LogsSaveToFileDone);
					}
				}
			}
		}

		public bool NetworkLockKnowledge()
		{
			string Msg = Messages.NetworkLockWarning;

			return AskYesNo(Msg);
		}

		public void NetworkLockActivation()
		{
			if (NetworkLockKnowledge())
			{
				Engine.Instance.NetLockIn();
			}
		}

		public void NetworkLockDeactivation()
		{
			Engine.NetLockOut();
		}
	}
}