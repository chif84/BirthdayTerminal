using Android;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using ELMA.BirthdayTerminal.Droid.Renderers.Camera;
using ELMA.BirthdayTerminal.Renderers;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewServiceRenderer))]
namespace ELMA.BirthdayTerminal.Droid.Renderers.Camera
{
	public class CameraViewServiceRenderer : ViewRenderer<CameraView, CameraDroid>
	{
		private CameraDroid _camera;
		private readonly Context _context;

		public CameraViewServiceRenderer(Context context) : base(context)
		{
			_context = context;
		}



		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			var permissions = CameraPermissions(); //TODO вычистить пермишены
			_camera = new CameraDroid(Context);

			if (Control == null)
			{
				if (permissions)
				{
					_camera.OpenCamera();

					SetNativeControl(_camera);
				}
			}

			if (e.NewElement != null && _camera != null)
			{
                Element.OnTakeClick += OnTakeClick;
                _camera.Photo += OnPhoto;
			}
		}


        private void OnTakeClick(object sender, object obj)
        {
            if ((bool)obj)
            {
                _camera?.TakePhoto();
            }
            else
            {
                
            }
        }

        private async void OnPhoto(object sender, ImageSource imgSource)
		{
		    Device.BeginInvokeOnMainThread(() =>
		    {
                TakePicturePage.OnPhotoCaptured(imgSource);
		    });
		}

		protected override void Dispose(bool disposing)
		{
			_camera.Photo -= OnPhoto;

			base.Dispose(disposing);
		}

		private bool CameraPermissions()
		{
			const string permission = Manifest.Permission.Camera;

			if ((int)Build.VERSION.SdkInt < 23 || ContextCompat.CheckSelfPermission(Android.App.Application.Context, permission) == Permission.Granted)
			{
				return true;
			}

			return false;
		}

		private async Task<ImageSource> RotateImageToPortrait(ImageSource imgSource)
		{
			var imagesourceHandler = new StreamImagesourceHandler();
			var photoTask = imagesourceHandler.LoadImageAsync(imgSource, _context);

			var photo = await photoTask;

			var matrix = new Matrix();

			matrix.PreRotate(-90);
			photo = Bitmap.CreateBitmap(photo, 0, 0, photo.Width, photo.Height, matrix, false);
			matrix.Dispose();

			var stream = new MemoryStream();
			photo.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
			stream.Seek(0L, SeekOrigin.Begin);

			return ImageSource.FromStream(() => stream);
		}
	}
}
