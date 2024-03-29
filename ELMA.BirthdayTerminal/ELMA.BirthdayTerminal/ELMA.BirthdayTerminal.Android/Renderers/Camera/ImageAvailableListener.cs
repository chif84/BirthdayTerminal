﻿using Android.Media;
using System;

namespace ELMA.BirthdayTerminal.Droid.Renderers.Camera
{
	public class ImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
	{
		public event EventHandler<byte[]> Photo;

		public void OnImageAvailable(ImageReader reader)
		{
			Image image = null;

			try
			{
				image = reader.AcquireLatestImage();
				var buffer = image.GetPlanes()[0].Buffer;
				var imageData = new byte[buffer.Capacity()];
				buffer.Get(imageData);

				Photo?.Invoke(this, imageData);
			}
			catch (Exception)
			{
			}
			finally
			{
				image?.Close();
			}
		}
	}
}
