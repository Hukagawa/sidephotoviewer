﻿using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SidePhotoViewer.Views
{
    public partial class SSViewPage : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSViewPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        /// <summary>
        /// フォルダ選択ボタン押下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_openfolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // 生成：インスタンス
            var dlg = new CommonOpenFileDialog();

            // 設定：初期フォルダ
            dlg.InitialDirectory = @"c:\";
            // 設定：フォルダ選択(trueでフォルダ、falseでファイル)
            dlg.IsFolderPicker = true;
            // 設定：タイトル
            dlg.Title = "フォルダ選択";

            // 表示：ダイアログ
            var ret = dlg.ShowDialog();
            // フォルダパス
            string folder_path = "";
            // 確認：ダイアログ戻り値
            if (ret == CommonFileDialogResult.Ok)
            {
                folder_path = dlg.FileName;
            }
            // 取得：フォルダ内ファイル一覧
            string[] file_names_all = Directory.GetFiles(folder_path);
            var file_names_picture = new List<string>();
            var extension_list = new List<string>{".jpg", ".png", ".bmp", ".tif"};
            
            // テスト用
            string file_names_combined = "";
            foreach(string str in file_names_all)
            {
                // 拡張子比較
                string extension = System.IO.Path.GetExtension(str);
                if (extension_list.Contains(extension))
                {
                    file_names_combined += str + "\n";
                }
            }
            textBlock1.Text = file_names_combined;

            // 表示：画像
            //image_main.Source = new BitmapImage(new Uri(file_names_all[0]));


        }
    }
}
