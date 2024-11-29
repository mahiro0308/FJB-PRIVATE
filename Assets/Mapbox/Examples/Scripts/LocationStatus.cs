namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class LocationStatus : MonoBehaviour
	{

		[SerializeField]

		private AbstractLocationProvider _locationProvider = null;
		void Start()
		{
			if (null == _locationProvider)
			{
				_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
			}
		}


		void Update()
		{
			Location currLoc = _locationProvider.CurrentLocation;

			if (currLoc.IsLocationServiceInitializing)
			{
			}
			else
			{
				if (!currLoc.IsLocationServiceEnabled)
				{
				}
				else
				{
					if (currLoc.LatitudeLongitude.Equals(Vector2d.zero))
					{
					}
					else
					{
					}
				}
			}

		}
	}
}
