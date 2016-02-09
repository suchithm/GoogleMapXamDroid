using Android.App; 
using Android.OS; 
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System;

namespace GoogleMapApp
{
	[Activity ( Label = "Google Map", MainLauncher = true , Icon = "@drawable/icon" )]			
	public class GoogleMapActivity : Activity
	{  
		GoogleMap map;
		const double latitude=12.9543;
		const double longitude=77.5924; 
		protected override void OnCreate ( Bundle bundle )
		{
			base.OnCreate ( bundle );
			SetContentView ( Resource.Layout.GoogleMapLayout ); 
			FnSetUpGoogleMap();
			FnUpdateCameraPosition();
			FnMarkOnMap("Location", new LatLng(latitude, longitude), Resource.Drawable.markerIcon);
		}

		bool FnSetUpGoogleMap()
		{
			if (null != map)
				return false;

			var frag =  (MapFragment) FragmentManager.FindFragmentById(Resource.Id.map);

			var mapReadyCallback = new OnMapReadyClass();

			mapReadyCallback.MapReadyAction += delegate(GoogleMap googleMap)
			{
				map = googleMap; 
				if (map != null)
					map.MapType = GoogleMap.MapTypeNormal; 
			};

			frag.GetMapAsync(mapReadyCallback); 

			return true;
		}

		async void FnUpdateCameraPosition()
		{ 
			if (map != null)
			{
				//To initialize the map  
				CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
				builder.Target(new LatLng(latitude,longitude)); //Target to some location hardcoded
				builder.Zoom(15); //Zoom multiplier
				builder.Bearing(45);//bearing is the compass measurement clockwise from North
				builder.Tilt(40); //tilt is the viewing angle from vertical
				CameraPosition cameraPosition = builder.Build();
				CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
				map.AnimateCamera(cameraUpdate);
			}
		}
		async void FnMarkOnMap(string title,LatLng pos, int resourceId )
		{  
					try
					{
						var marker = new MarkerOptions();
						marker.SetTitle(title); 
						marker.SetPosition(pos); //Resource.Drawable.BlueDot
						marker.SetIcon(BitmapDescriptorFactory.FromResource(resourceId));
						map.AddMarker(marker);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					} 
		}

	}

	//OnMapReadyClass
	public class OnMapReadyClass :Java.Lang.Object,IOnMapReadyCallback
	{ 
		public GoogleMap Map { get; private set; }
		public event Action<GoogleMap> MapReadyAction;

		public void OnMapReady (GoogleMap googleMap)
		{
			Map = googleMap; 

			if ( MapReadyAction != null )
				MapReadyAction (Map);
		}
	}
}


