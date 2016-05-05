using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 http://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

//http://www.c-sharpcorner.com/UploadFile/020f8f/universal-windows-platform-and-extended-splash-screen/

namespace BeaConnect_mobile.Views
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    /// 

    // Define methods and constructor   

    public sealed partial class ExtendedSplash : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.  
        internal bool dismissed = false; // Variable to track splash screen dismissal status.  
        internal Frame rootFrame;

        private SplashScreen splash; // Variable to hold the splash screen object.  
        private double ScaleFactor; //Variable to hold the device scale factor (use to determine phone screen resolution)

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();
            DismissExtendedSplash();

            // Listen for window resize events to reposition the extended splash screen image accordingly.  
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...  
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            ScaleFactor = (double)DisplayInformation.GetForCurrentView().ResolutionScale / 100;

            splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.  
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.  
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }

            // Restore the saved session state if necessary  
            RestoreStateAsync(loadState);
        }

        async void RestoreStateAsync(bool loadState)
        {
            if (loadState)
                await SuspensionManager.RestoreAsync();
        }

        // Position the extended splash screen image in the same location as the system splash screen image.  
        void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.Left);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Top);
            extendedSplashImage.Height = splashImageRect.Height / ScaleFactor;
            extendedSplashImage.Width = splashImageRect.Width / ScaleFactor;
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...  
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.  
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).  
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
        }

        async void DismissExtendedSplash()
        {
            await Task.Delay(TimeSpan.FromSeconds(5)); // set your desired delay  
            rootFrame = new Frame();
            MainPage mainPage = new MainPage();
            rootFrame.Content = mainPage;
            Window.Current.Content = rootFrame;
            rootFrame.Navigate(typeof(MainPage)); // call MainPage  
        }
    }
}

