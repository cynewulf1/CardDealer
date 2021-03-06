﻿using System;
using System.Windows;
using CardDealer.ViewModel;

namespace CardDealer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                View.CardTable cardTable = new View.CardTable();
                CardDeckViewModel vm = new CardDeckViewModel();
                cardTable.DataContext = vm;
                cardTable.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
