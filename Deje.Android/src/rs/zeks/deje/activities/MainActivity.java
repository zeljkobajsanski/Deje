package rs.zeks.deje.activities;

import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.R.id;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

public class MainActivity extends Activity {

	private EditText nazivArtiklaEditText;
	private EditText udaljenost;
	private Button btnLocirajMe;
	private double latituda;
	private double longituda;
	
	private static final int GPS_SETTINGS_REQUEST = 1;
	
	public static final int LOCATION_INTENT_REQUEST = 2;
	
	private final LocationListener listener = new LocationListener() {

		private static final int TWO_MINUTES = 1000 * 60 * 2;
		
		private Location location;
		
		@Override
		public void onLocationChanged(Location location) {
			if (isBetterLocation(location, this.location)) {
				MainActivity.this.setLocation(location);
				this.location = location;
			}
		}

		@Override
		public void onProviderDisabled(String provider) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onProviderEnabled(String provider) {
			// TODO Auto-generated method stub
			
		}

		@Override
		public void onStatusChanged(String provider, int status, Bundle extras) {
			// TODO Auto-generated method stub
			
		}
		
		protected boolean isBetterLocation(Location location, Location currentBestLocation) {
		    if (currentBestLocation == null) {
		        // A new location is always better than no location
		        return true;
		    }

		    // Check whether the new location fix is newer or older
		    long timeDelta = location.getTime() - currentBestLocation.getTime();
		    boolean isSignificantlyNewer = timeDelta > TWO_MINUTES;
		    boolean isSignificantlyOlder = timeDelta < -TWO_MINUTES;
		    boolean isNewer = timeDelta > 0;

		    // If it's been more than two minutes since the current location, use the new location
		    // because the user has likely moved
		    if (isSignificantlyNewer) {
		        return true;
		    // If the new location is more than two minutes older, it must be worse
		    } else if (isSignificantlyOlder) {
		        return false;
		    }

		    // Check whether the new location fix is more or less accurate
		    int accuracyDelta = (int) (location.getAccuracy() - currentBestLocation.getAccuracy());
		    boolean isLessAccurate = accuracyDelta > 0;
		    boolean isMoreAccurate = accuracyDelta < 0;
		    boolean isSignificantlyLessAccurate = accuracyDelta > 200;

		    // Check if the old and new location are from the same provider
		    boolean isFromSameProvider = isSameProvider(location.getProvider(),
		            currentBestLocation.getProvider());

		    // Determine location quality using a combination of timeliness and accuracy
		    if (isMoreAccurate) {
		        return true;
		    } else if (isNewer && !isLessAccurate) {
		        return true;
		    } else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider) {
		        return true;
		    }
		    return false;
		}

		/** Checks whether two providers are the same */
		private boolean isSameProvider(String provider1, String provider2) {
		    if (provider1 == null) {
		      return provider2 == null;
		    }
		    return provider1.equals(provider2);
		}
	
	};
	
	
	
	@Override
	protected void onStop() {
		LocationManager lManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
		lManager.removeUpdates(listener);
		super.onStop();
	}

	@Override
	protected void onResume() {
		azurirajPoziciju();
		super.onResume();
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		btnLocirajMe = (Button)findViewById(id.btnLocirajMe);
		nazivArtiklaEditText = (EditText)findViewById(R.id.nazivArtikla);
		udaljenost = (EditText)findViewById(R.id.udaljenost);
		udaljenost.setText("2000");
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if (GPS_SETTINGS_REQUEST == requestCode) {
			/*LocationManager lManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
			Location location = lManager.getLastKnownLocation(Constants.LOCATION_PROVIDER);
			if (location != null) {
				setLocation(location);	
			}
			lManager.requestLocationUpdates(Constants.LOCATION_PROVIDER, 10000, 10, listener);*/
		} else if (LOCATION_INTENT_REQUEST == requestCode && resultCode == RESULT_OK) {
			String adresa = data.getStringExtra(Constants.ADRESA);
			latituda = data.getDoubleExtra(Constants.LATITUDA, 0);
			longituda = data.getDoubleExtra(Constants.LONGITUDA, 0);
			btnLocirajMe.setText(adresa);
		}
	}



	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_main, menu);
		return true;
	}
	
	public void pronadji(View view) {
		if (longituda == 0 || longituda == 0) {
			Toast.makeText(this, R.string.pretraga_nije_moguca, Toast.LENGTH_SHORT).show();
			return;
		}
		String nazivArtikla = nazivArtiklaEditText.getText().toString();
		Intent intent = null;
		if (nazivArtikla == null || nazivArtikla.length() == 0) {
			intent = new Intent(this, GrupeArtikalaActivity.class);
		} else {
			intent = new Intent(this, ArtikliActivity.class);
		}
		intent.putExtra(Constants.NAZIV_ARTIKLA, nazivArtikla);
		intent.putExtra(Constants.UDALJENOST, String.valueOf(getUdaljenost()));
		intent.putExtra(Constants.LATITUDA, String.valueOf(latituda));
		intent.putExtra(Constants.LONGITUDA, String.valueOf(longituda));
		startActivity(intent);
	}
	

	public void setLocation(Location location) {
		this.latituda = location.getLatitude();
		this.longituda = location.getLongitude();
		btnLocirajMe.setText(location != null ? R.string.pozicija_je_azurirana : R.string.pozicija_nije_pronadjena);
	}

	private int getUdaljenost(){
		int udaljenost = Integer.parseInt(this.udaljenost.getText().toString());
		return udaljenost;
	}
	
	private void azurirajPoziciju() {
		LocationManager lManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
		
		final boolean networkUkljucen = lManager.isProviderEnabled(LocationManager.NETWORK_PROVIDER);
		final boolean gpskUkljucen = lManager.isProviderEnabled(LocationManager.GPS_PROVIDER);
		if (!networkUkljucen || !gpskUkljucen) {
			Intent intent = new Intent(android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
			startActivityForResult(intent, GPS_SETTINGS_REQUEST);
		} else {
			/*Location location = lManager.getLastKnownLocation(LocationManager.NETWORK_PROVIDER);
			if (location != null) {
				setLocation(location);	
			}*/
			lManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, 10000, 10, listener);
			lManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 10000, 10, listener);
		}
		
	}
	
	public void locirajMe(View view) {
		startActivityForResult(new Intent(this, PretragaLokacijeActivity.class), LOCATION_INTENT_REQUEST);
	}

}
