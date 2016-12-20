using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Graphics;
using System.Net;
using Square.Picasso;
using KaraokeApp.Helper;
using Android.Views.Animations;
using KaraokeApp.ViewModel;

namespace KaraokeApp
{
    public class SongAdapter : RecyclerView.Adapter
    {
        List<Song> listSong;

        public List<Song> ListSong
        {
            get { return listSong; }
            set { listSong = value; }
        }
        Activity _activity;


        public SongAdapter(Activity activity)
        {
           
            this._activity = activity;
        }

        public override int ItemCount
        {
            get
            {
                return listSong==null?0:listSong.Count;
            }
        }
      
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SongViewHolder songHolder = holder as SongViewHolder;

			songHolder.txtSongName.Text = CheckNameLength(listSong[position].Name);
            songHolder.txtSinger.Text = "Chi dan";
			songHolder.Link = listSong[position].Link;

            Animation fadeInAnimation = AnimationUtils.LoadAnimation(_activity, Resource.Animation.fade);
            
            songHolder.imgSong.StartAnimation(fadeInAnimation);
            
            //Picasso.With(_activity).Load(listSong[position].Image).Into(songHolder.imgSong);
            BitmapHelper.LoadImage(_activity,listSong[position].Image, songHolder.imgSong);
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    
                }
            }

            return imageBitmap;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View v = inflater.Inflate(Resource.Layout.itemSong, parent, false);
            return new SongViewHolder(v);
        }

		private string CheckNameLength(string name)
		{
			int MAX_LENGTH_OF_NAME = 55;
			if (name.Length > MAX_LENGTH_OF_NAME)
			{
				name = name.Substring(0 , MAX_LENGTH_OF_NAME - 3);
				name += "...";
			}
			return name;
		}
    }

    public class SongViewHolder : RecyclerView.ViewHolder
    {
        public ImageView imgSong;
        public TextView txtSongName, txtSinger, txtDuration;
		public string Link;
        protected Song song;

		//MainViewmodel
		//VM

        public SongViewHolder(View itemView)
            : base(itemView)
        {
            imgSong = itemView.FindViewById<ImageView>(Resource.Id.imgPicture);
            txtSongName = itemView.FindViewById<TextView>(Resource.Id.txtSongName);
            txtSinger = itemView.FindViewById<TextView>(Resource.Id.txtSinger);
            txtDuration = itemView.FindViewById<TextView>(Resource.Id.txtDuration);

            itemView.Click += (sender, e) =>
            {
				song = new Song(Link, txtSongName.Text);
				Vm.NavigateToDetail(song);
				//Toast.MakeText(itemView.Context, Link + " clicked!", ToastLength.Short).Show();
            };
        }
		private MainViewModel Vm = App.Locator.Main;
    }
}