using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SidePhotoViewer.Views
{
    public partial class SSViewPage : Page, INotifyPropertyChanged
    {
        public HorizontalAlignment Strech { get; private set; }
        BitmapImage picture_left;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SSViewPage()
        {
            InitializeComponent();
            DataContext = this;

            picture_left = new BitmapImage();
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
            var extension_list = new List<string>{".jpg", ".png", ".bmp", ".tif"};

            // 取得：StackPanelの幅
            var stack_panel_width = stackPanelPicture.ActualWidth;

            // 取得：すべての画像データのリスト
            var file_names_picture = new List<Uri>();
            var picture_list = new List<BitmapImage>();
            foreach (string uri in file_names_all)
            {
                // 拡張子比較
                string extension = System.IO.Path.GetExtension(uri);
                if (extension_list.Contains(extension))
                {
                    file_names_picture.Add(new Uri(uri));
                }
            }

            // 取得：左ペイン幅
            var grid_left_width = gridLeft.Width;

            // 取得：右ペイン幅
            var grid_right_widht = gridRight.Width;

            // 表示：左ペイン画像
            picture_left = new BitmapImage(file_names_picture[0]);
            picture_left.DecodePixelWidth = (int)grid_left_width;
            //picture_left.UriSource = file_names_picture[0];
            imageMain.Source = picture_left;

            // 右ペインの画像を削除する
            stackPanelPicture.Children.Clear();

            // 表示：右ペイン画像（動的にImageを作成する）
            foreach (Uri picture in file_names_picture)
            {
                // Imageコントロール：サムネイルの表示用
                var bitmapimage = new BitmapImage(picture);
                bitmapimage.DecodePixelWidth = (int)stack_panel_width;
                //bitmapimage.UriSource = picture;
                var image_view = new Image();
                image_view.Source = bitmapimage;
                string uri = picture.ToString();
                // Nameだと文字列の制限があるためUid
                image_view.Uid = uri;

                stackPanelPicture.Children.Add(image_view);
                
            }

        }

        private void stackPanelPicture_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement fe = e.Source as FrameworkElement;
            if(fe.Uid != null)
            {
                textBlock1.Text = fe.Uid;
                picture_left = new BitmapImage(new Uri(fe.Uid));
                imageMain.Source = picture_left;
            }
        }
    }
}
