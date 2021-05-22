using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace WhosOnFirst
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window, INotifyPropertyChanged
   {
      private List<string> Teammates;
      private static Random RNG = new Random();
      private string _currentTeammate;
      public string CurrentTeammate
      {
         get { return _currentTeammate; }
         set 
         { 
            _currentTeammate = value;
            NotifyPropertyChanged("CurrentTeammate");
         }
      }

      private string ManagerName;
      

      public event PropertyChangedEventHandler PropertyChanged;

      public void NotifyPropertyChanged(string propertyName)
      {
         if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }

      public MainWindow()
      {
         this.CurrentTeammate = "foo";
         this.DataContext = this;
         InitializeComponent();
         LoadTeammatesAndManager();
         LoadNextTeammate();
      }


      private void LoadTeammatesAndManager()
      {
         string teammatesFromConfig = ConfigurationManager.AppSettings["TeamMembersList"]?.ToString();
         if(teammatesFromConfig != null && teammatesFromConfig.Length > 0)
         {
            this.Teammates = teammatesFromConfig.Split(',').ToList();
            this.Teammates.Shuffle();
         }
         else
         {
            MessageBoxResult result = MessageBox.Show(this, "Teammates could not be read from config.");
            this.Close();
            System.Windows.Application.Current.Shutdown();
         }
         

         this.ManagerName = ConfigurationManager.AppSettings["ManagerName"]?.ToString();
        }

      private void LoadNextTeammate()
      {
         if(this.Teammates?.Count > 0)
         {
            this.CurrentTeammate = this.Teammates.First();
            this.Teammates.RemoveAt(0);
         }
         else
         {
            if(this.ManagerName?.Length > 0)
            {
               this.CurrentTeammate = this.ManagerName;
            }
            btnReset.IsEnabled = true;
         }
      }

      private void Reset()
      {
         LoadTeammatesAndManager();
         LoadNextTeammate();
         btnReset.IsEnabled = false;
      }

      private void ExitApplication()
      {
         System.Windows.Application.Current.Shutdown();
      }



      private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
      {
         if (!btnReset.IsEnabled)
         {
            LoadNextTeammate();
         }
      }

      private void btnReset_Click(object sender, RoutedEventArgs e)
      {
         Reset();
      }

      private void Window_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Space)
         {
            if (!btnReset.IsEnabled)
            {
               LoadNextTeammate();
            }
         }
         if(e.Key == Key.Enter || e.Key == Key.Return)
         {
            if (btnReset.IsEnabled)
            {
               Reset();
            }
         }
         if(e.Key == Key.Escape)
         {
            ExitApplication();
         }
      }
   }

   public static class ThreadSafeRandom
   {
      [ThreadStatic] private static Random Local;

      public static Random ThisThreadsRandom
      {
         get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
      }
   }

   static class MyExtensions
   {
      public static void Shuffle<T>(this IList<T> list)
      {
         int n = list.Count;
         while (n > 1)
         {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
         }
      }
   }
}
