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

namespace App_Contacts
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(detailsPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LstContacts.ItemsSource = App.contacts;
        }

        private void LstContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = int.Parse(LstContacts.SelectedValue.ToString());
            this.Frame.Navigate(typeof(detailsPage),id);
        }

        private void Search()
        {
            string key = TxtKeywords.QueryText.ToLower();
            var result = from c in App.contacts.AsEnumerable() where CheckKeywords(c, key) select c;
            LstContacts.ItemsSource = result.ToList();
        }
        private bool CheckKeywords (Contact c , string key)
        {
            //if (c.ContactName.ToLower().Contains(key) || c.Phone.Contains(key))
            //{
            //    return true;
            //}
            string groupname = CbbGroups.SelectedValue.ToString();
            if (!groupname.Equals("All") && !c.GroupName.Equals(groupname))
            {
                return false;
            }
            else
            {
                foreach (string item in key.Split(' '))
                {
                    if (String.IsNullOrEmpty(item)) continue;
                    if (!c.ContactName.ToLower().Contains(item))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
        private void TxtKeywords_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            string key = TxtKeywords.QueryText.ToLower();
            var result = from c in App.contacts.AsEnumerable() where CheckKeywords(c , key) select c;
            LstContacts.ItemsSource = result.ToList();
        }

        private void CbbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string groupname = CbbGroups.SelectedValue.ToString();
            if (groupname.Equals("All"))
            {
                LstContacts.ItemsSource = App.contacts;
            }
            else
            {
                var result = from c in App.contacts
                             where c.GroupName.Equals(groupname)
                             select c;
                LstContacts.ItemsSource = result.ToList();
            }
        }
    }
}
