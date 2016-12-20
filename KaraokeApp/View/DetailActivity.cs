
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace KaraokeApp
{
    [Activity(Label = "DetailActivity", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class DetailsActivity : ActivityBase
	{
		private TextView _txvName, _txvSinger, _txvDescription;
		private WebView _wvVideo;
		private Button _btnChoose;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Detail);
			// Create your application here
			FindControl();
			bool result = GetDataFromMain();
			if (!result)
				Toast.MakeText(this, "Problem", ToastLength.Short);

			AddEvent();
		}

		//Pause video when the Activity on Pause

		protected override void OnPause()
		{
			base.OnPause();
			_wvVideo.OnPause();
		}

		private void FindControl()
		{
			_txvName = FindViewById<TextView>(Resource.Id.txvName);
			_txvSinger = FindViewById<TextView>(Resource.Id.txvSinger);
			_txvDescription = FindViewById<TextView>(Resource.Id.txvDesciption);
			_btnChoose = FindViewById<Button>(Resource.Id.btnChoose);

			_wvVideo = FindViewById<WebView>(Resource.Id.wvVideo);
			// Config WebView
			WebSettings settings = _wvVideo.Settings;
			settings.JavaScriptEnabled = true;

			_wvVideo.Settings.LoadWithOverviewMode = true;
			_wvVideo.Settings.UseWideViewPort = true;
			_wvVideo.SetWebChromeClient(new WebChromeClient());
		}

		private void AddEvent()
		{
			_btnChoose.Click += (sender, e) => 
			{
				//Toast.MakeText(this, _txvName.Length().ToString(), ToastLength.Long);
			};
		}

		private bool GetDataFromMain()
		{
			NavigationService nav = (NavigationService)ServiceLocator.Current.GetInstance<INavigationService>();
			var song = nav.GetAndRemoveParameter<Song>(Intent);

			if (song != null)
			{
				_txvName.Text = song.Name;
				_txvDescription.Text = "Name Length: " + song.Name.Length.ToString();
				//TODO Singer and Description
				LoadVideo(song.Link);
				return true;
			}
			return false;
		}

		private void LoadVideo(string url)
		{
			url = Vm.ProcessLink(url);	
			_wvVideo.LoadUrl("https://www.youtube.com/embed/" + url);
		}

		DetailViewModel Vm = App.Locator.Detai;
	}
}