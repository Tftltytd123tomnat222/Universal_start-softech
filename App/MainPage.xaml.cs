using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (TxtUserName.Text.Equals("admin"))
            {
                if (TxtPassword.Text.Equals("123"))
                {
                    lblLogin.Text = "Login successful";
                }
                else
                {
                    lblLogin.Text = "MẬT KHẨU KHÔNG CHÍNH XÁC?";
                }
            }
            else
            {
                lblLogin.Text = "TÀI KHOẢN KHÔNG TỒN TẠI?";
            }
        }
    }
}
