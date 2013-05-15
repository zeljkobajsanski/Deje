package rs.zeks.deje.activities;

import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.R.id;
import android.location.Location;
import android.location.LocationManager;
import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.view.Menu;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

public class PretragaLokacijeActivity extends Activity {

	private TextView adresa;
	private static final int REQUEST_IZABERI_ADRESU = 1;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_pretraga_lokacije);
		adresa = (TextView)findViewById(id.adresa);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_pretraga_lokacije, menu);
		return true;
	}
	
	public void pronadjiAdresu(View view) {
		String adresa = this.adresa.getText().toString();
		if (adresa != null && adresa.length() > 0) {
			Intent intent = new Intent(this, PrikazAdresaActivity.class);
			intent.putExtra(Constants.ADRESA, adresa);
			startActivityForResult(intent, REQUEST_IZABERI_ADRESU);
		} else {
			Toast.makeText(this, "Unestite adresu za pretragu", Toast.LENGTH_SHORT).show();
		}
	}
	
	public void locirajMe(View view) {
		LocationManager lManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
		boolean providerEnabled = lManager.isProviderEnabled(Constants.LOCATION_PROVIDER);
		if (providerEnabled) {
			Location location = lManager.getLastKnownLocation(Constants.LOCATION_PROVIDER);
			if (location != null) {
				Intent intent = new Intent();
				intent.putExtra(Constants.ADRESA, "Vaša pozicija je ažurirana");
				intent.putExtra(Constants.LATITUDA, location.getLatitude());
				intent.putExtra(Constants.LONGITUDA, location.getLongitude());
				setResult(RESULT_OK, intent);
				finish();
			}
		}
	}
	
	

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if (REQUEST_IZABERI_ADRESU == requestCode) {
			setResult(resultCode, data);
			finish();
		}
	}
	
	

}
