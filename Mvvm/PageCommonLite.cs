using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Practic.Framework.Support;

namespace Practic.Framework.mvvm
{
    public class PageCommonLite : PhoneApplicationPage
    {

		public bool IsLoading
		{
			get { return (bool)GetValue(IsLoadingProperty); }
			set { SetValue(IsLoadingProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsLoading.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsLoadingProperty =
			DependencyProperty.Register("IsLoading", typeof(bool), typeof(PageCommonLite), new PropertyMetadata(false, OnLoadingChanged));

    	private static void OnLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    	{
    		var currentPage = (PageCommonLite) d;

    		var isLoading = (bool) e.NewValue;

			if(currentPage.LoadingBar == null)
			{
				currentPage.LoadingBar = InitializeSystemTray(currentPage);
			}

    		currentPage.LoadingBar.IsVisible = isLoading;
    	}

		private static ProgressIndicator InitializeSystemTray(PageCommonLite currentPage)
    	{
    		SystemTray.SetIsVisible(currentPage, true);
			SystemTray.SetOpacity(currentPage, 0.0);

    		var loadingBar = new ProgressIndicator() {IsIndeterminate = true};

			SystemTray.SetProgressIndicator(currentPage, loadingBar);

    		return loadingBar;
    	}

    	private ProgressIndicator LoadingBar { get; set; }


    	public PageCommonLite()
        {
            LayoutUpdated += PageCommonLiteLayoutUpdated;
        }

        private void PageCommonLiteLayoutUpdated(object sender, EventArgs e)
        {
            LayoutUpdated -= PageCommonLiteLayoutUpdated;
            FirstLayoutUpdated(sender, e);
        }
        
        protected sealed override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            PageOnNavigatedTo(e);
        }

        protected sealed override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            PageOnNavigatingFrom(e);
        }
        
        protected sealed override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
			if(e.NavigationMode == NavigationMode.Back)
			{
				SystemTray.SetProgressIndicator(this, null);
			    SystemTray.IsVisible = false;
				LoadingBar = null;
			}
            
            PageOnNavigatedFrom(e);
            GC.Collect();
        }

        protected virtual void FirstLayoutUpdated   (object sender, EventArgs eventArgs)                    {}
        protected virtual void PageOnNavigatedTo    (NavigationEventArgs navigationEventArgs)               {}
        protected virtual void PageOnNavigatingFrom (NavigatingCancelEventArgs navigatingCancelEventArgs)   {}
        protected virtual void PageOnNavigatedFrom  (NavigationEventArgs navigationEventArgs)               {}

        ~PageCommonLite()
        {
            DebugUtils.SignalObjectCollected(GetType());
        }
    }
}
