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
using Android.Graphics;
using System.Net;
using Android.Support.V4.Util;
using System.Threading;
using Android.Support.V4.Graphics.Drawable;
using Android.Content.Res;
using Android.Media;

namespace KaraokeApp.Helper
{
    class BitmapHelper
    {

		private const float RADIUS = 10.0f;

        private static LruCache m_memoryCache;
        public BitmapHelper()
        {
            

        }
        
        private static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
					imageBitmap = getRoundedShape(imageBitmap);
                }
            }

            return imageBitmap;
        }
        public static  void LoadImage(Activity activity,string url, ImageView imageView)
        {
            int cacheSize = 1 * 1024 * 1024;
            m_memoryCache = new LruCache(cacheSize);
            new Thread(new ThreadStart(() =>
            {
                Bitmap bitmap=getBitmapFromMemCache(url);
                if (bitmap == null)
                {
                    bitmap=GetImageBitmapFromUrl(url);
                    addBitmapToMemoryCache(url, bitmap);
                }
                activity.RunOnUiThread(() => ShowImage(imageView, bitmap));
            })).Start();
        }
        public static void ShowImage(ImageView imageView,Bitmap bitmap)
        {
            imageView.SetImageBitmap(bitmap);
        }
        public static void addBitmapToMemoryCache(string key, Bitmap bitmap)
        {
            if (getBitmapFromMemCache(key) == null)
            {
                m_memoryCache.Put(key, bitmap);
            }
        }
        public static Bitmap getBitmapFromMemCache(String key)
        {
            return (Bitmap)m_memoryCache.Get(key);
        }

		public static Bitmap getRoundedShape(Bitmap scaleBitmapImage)
		{
			int targetWidth = scaleBitmapImage.Width;
			int targetHeight = scaleBitmapImage.Height;
			Bitmap targetBitmap = Bitmap.CreateBitmap(targetWidth,
				targetHeight, Bitmap.Config.Argb8888);

			Canvas canvas = new Canvas(targetBitmap);

			var xferPaint = new Paint(PaintFlags.AntiAlias);
			xferPaint.Color = Color.Red;
			canvas.DrawRoundRect(new RectF(0, 0, targetWidth, targetHeight), RADIUS, RADIUS, xferPaint);

			xferPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));

			Bitmap result = Bitmap.CreateBitmap(targetWidth,
				targetHeight, Bitmap.Config.Argb8888);
			Canvas resultCanvas = new Canvas(result);
			resultCanvas.DrawBitmap(scaleBitmapImage, 0, 0, null);
			resultCanvas.DrawBitmap(targetBitmap, 0, 0, xferPaint);

			return result;
		}


    }
}