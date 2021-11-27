﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UICore;

namespace ObsMaster.ViewModel
{
    public partial class MainVM
    {
        public MainWindow MainWnd { get; set; } 

        //  private Dialog.MonitorDialog m_monitorWnd;


        //  创建场景
        public void ShowCreateScene()
        {
            var sceneWnd = new Dialog.CreateSceneDialog();
            sceneWnd.Owner = MainWnd;
            sceneWnd.ShowDialog();

            if (sceneWnd.iResult == 0)
            {   
                ObsCore.CreateScene(sceneWnd.sSceneName);
            }
        }

        //  设置对话框
        private DelegateCommand _settingDialogCmd;
        public DelegateCommand SettingDialogCmd { 
            get {
                return _settingDialogCmd ?? (_settingDialogCmd = new DelegateCommand((obj) =>
                {
                    var dlg = new Dialog.SettingDialog();
                    dlg.DataContext = this;
                    dlg.Owner = MainWnd;   
                    
                    dlg.ShowDialog();
                }));
            } 
        }     
        

        //  屏幕捕捉
        public ObservableCollection<Model.MMonitorItem> VMonitorItems { get; set; } = new ObservableCollection<Model.MMonitorItem>();
        public void ShowMonitor()
        {
            var monitorWnd = new Dialog.MonitorDialog();
            monitorWnd.DataContext = this;
            monitorWnd.Owner = MainWnd;

            List<CLI_MinitorItem> ls = new List<CLI_MinitorItem>();
            ObsCore.GetMonitors(ls);

            VMonitorItems.Clear();
            foreach (var v in ls)
            {
                VMonitorItems.Add(new Model.MMonitorItem() { Id = v.Id, Name = v.Name, });
            }

            monitorWnd.ShowDialog();
        }

        private DelegateCommand _monitorCaptureCmd; 
        public DelegateCommand MonitorCaptureCmd
        {
            get {
                return _monitorCaptureCmd ?? (_monitorCaptureCmd = new DelegateCommand((obj) =>
                {
                    var monitorWnd = new Dialog.MonitorDialog();
                    monitorWnd.DataContext = this;
                    monitorWnd.Owner = MainWnd;

                    List<CLI_MinitorItem> ls = new List<CLI_MinitorItem>();
                    ObsCore.GetMonitors(ls);

                    VMonitorItems.Clear();
                    foreach (var v in ls)
                    {
                        VMonitorItems.Add(new Model.MMonitorItem() { Id = v.Id, Name = v.Name, });
                    }

                    monitorWnd.ShowDialog();
                }));
            }
            
        }

        //  窗口捕捉
        public ObservableCollection<Model.MWindowItem> VCaptureWindowItems { get; set; } = new ObservableCollection<Model.MWindowItem>();
        public void ShowCaptureWindow()
        {
            var captureWnd = new Dialog.WinCaptureDialog();
            captureWnd.DataContext = this;
            captureWnd.Owner = MainWnd;

            List<CLI_WindowItem> ls = new List<CLI_WindowItem>();
            ObsCore.GetWindows(ls);

            VCaptureWindowItems.Clear();
            foreach(var v in ls)
            {
                VCaptureWindowItems.Add(new Model.MWindowItem() { Name = v.Name, WinName = v.WinName, });
            }

            captureWnd.ShowDialog();
        }

        private DelegateCommand _windowCaptureCmd;
        public DelegateCommand WindowCaptureCmd
        {
            get
            {
                return _windowCaptureCmd ?? (_windowCaptureCmd = new DelegateCommand((obj) =>
                {
                    var captureWnd = new Dialog.WinCaptureDialog();
                    captureWnd.DataContext = this;
                    captureWnd.Owner = MainWnd;

                    List<CLI_WindowItem> ls = new List<CLI_WindowItem>();
                    ObsCore.GetWindows(ls);

                    VCaptureWindowItems.Clear();
                    foreach (var v in ls)
                    {
                        VCaptureWindowItems.Add(new Model.MWindowItem() { Name = v.Name, WinName = v.WinName, });
                    }

                    captureWnd.ShowDialog();
                }));
            }
        }


        //  图片捕捉
        private DelegateCommand _pictureCaptureCmd;
        public DelegateCommand PictureCaptureCmd
        {
            get
            {
                return _pictureCaptureCmd ?? (_pictureCaptureCmd = new DelegateCommand((obj) =>
                {
                    var wnd = new Dialog.ASPictureDialog();
                    wnd.DataContext = this; 
                    wnd.Owner = MainWnd;
                    wnd.ShowDialog();
                    if (wnd.iResult == 0)
                    {
                        ObsCore.AddImageSource(wnd.FilePath, 100);
                    }
                }));
            }
        }

        //  文字捕捉
        private DelegateCommand _textCaptureCmd;
        public DelegateCommand TextCaptureCmd
        {
            get
            {
                return _textCaptureCmd ?? (_textCaptureCmd = new DelegateCommand((obj) =>
                {
                    try
                    {
                        var wnd = new Dialog.ASTextDialog();
                        wnd.DataContext = this;
                        wnd.Owner = MainWnd;
                        wnd.ShowDialog();
                        if (wnd.iResult == 0)
                        {
                            string text = wnd.DisplayContentTextBox.Text;
                            if (wnd.TextConvertComboBox.SelectedIndex == 1)
                                text = text.ToUpper();
                            else if (wnd.TextConvertComboBox.SelectedIndex == 2)
                                text = text.ToLower();

                            ObsCore.AddTextSource(new CLI_TextData()
                            {
                                Name = "文字",
                                Text = text,
                                Font = wnd.sFontFamilyName,
                                Color = ColorToUInt(wnd.ForegroundColor.Color),
                                Size = wnd.iFontSize,
                                Bold = wnd.bBold,
                                Italic = wnd.bItalic,
                                Extents = wnd.CustomGridSwitch.IsChecked == true,
                                ExtentsCx = wnd.CustomGridSwitch.IsChecked == true ? Convert.ToInt32(wnd.CustomGridWidth.Text) : 0,
                                ExtentsWrap = wnd.CustomGridAutoWrapCheckBox.IsChecked == true,
                                Align = (wnd.AlignModeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem).Tag.ToString(),
                                Valign = (wnd.ValignModeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem).Tag.ToString(),
                                OutlineSize = wnd.OutlineSwitch.IsChecked == true ? Convert.ToInt32(wnd.OutlineSizeTextBox.Text) : 0,
                                OutlineColor = ColorToUInt(wnd.OutlineColor.Color),
                                ScrollSpeed = Convert.ToInt32((wnd.ScrollSpeedComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem).Tag),
                                Opacity = Convert.ToInt32(wnd.OpacityTextBox.Text),
                                OutlineOpacity = Convert.ToInt32(wnd.OutlineOpacityTextBox.Text)
                            });
                        }
                    }
                    catch { }
                }));
            }
        }

        private uint ColorToUInt(System.Windows.Media.Color color)
        {
            try
            {
                uint colorref = Convert.ToUInt32(color.R + color.G * 256 + color.B * 256 * 256);
                return colorref;
            }
            catch { }
            return 0;
        }

        //  开始开播
        public void StartStream()
        {
            ObsCore.StartStream();
        }

        //  停止开播
        public void StopStream()
        {
            //  ObsCore.
        }

    }
}
