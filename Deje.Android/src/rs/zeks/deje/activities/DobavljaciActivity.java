package rs.zeks.deje.activities;

import java.io.IOException;
import java.util.Collection;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;

import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.model.Dobavljac;
import rs.zeks.deje.services.ServiceProxy;
import android.R.id;
import android.os.AsyncTask;
import android.os.Bundle;
import android.content.Context;
import android.content.Intent;
import android.support.v4.app.FragmentActivity;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TabHost;
import android.widget.TextView;
import android.widget.Toast;

public class DobavljaciActivity extends FragmentActivity implements ListView.OnItemClickListener {

	private ProgressBar progressBar;
	private Dobavljac[] dobavljaci;
	private GoogleMap mapa;
	String latituda;
	String longituda;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_dobavljaci);
		TabHost tabs = (TabHost)findViewById(id.tabhost);
		tabs.setup();
		
		TabHost.TabSpec spec = tabs.newTabSpec("tag1");
		spec.setContent(R.id.tab1);
		spec.setIndicator("Dobavljaèi");
		tabs.addTab(spec);
		
		spec = tabs.newTabSpec("tag2");
		spec.setContent(R.id.tab2);
		spec.setIndicator("Mapa");
		tabs.addTab(spec);
		progressBar = (ProgressBar)findViewById(R.id.progressBar1);
		progressBar.setIndeterminate(true);
		Intent intent = getIntent();
		latituda = intent.getStringExtra(Constants.LATITUDA);
		longituda = intent.getStringExtra(Constants.LONGITUDA);
		String udaljenost = intent.getStringExtra(Constants.UDALJENOST);
		String id = intent.getStringExtra(Constants.ID_ARTIKLA);
		String tip = intent.getStringExtra(Constants.TIP_ARTIKLA);
		ListView list = (ListView)findViewById(R.id.listaDobavljaca);
		list.setOnItemClickListener(this);
		inicializujMapu();

		new PretraziDobavljaceTask().execute(latituda, longituda, udaljenost, id, tip);
	}
	
	

	@Override
	protected void onResume() {
		super.onResume();
		inicializujMapu();
	}
	
	@Override
	public void onItemClick(AdapterView<?> arg0, View arg1, int position, long id) {
		Dobavljac dobavljac = dobavljaci[position];
		Intent intent = new Intent(this, PregledDobavljacaActivity.class);
		intent.putExtra(Constants.ID_DOBAVLJACA, dobavljac.getId());
		startActivity(intent);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_dobavljaci, menu);
		return true;
	}
	
	private void inicializujMapu() {
		if (mapa == null) {
			mapa = ((SupportMapFragment) getSupportFragmentManager().findFragmentById(R.id.map)).getMap();
			if (mapa != null) {
				mapa.setMyLocationEnabled(true);
				mapa.moveCamera(CameraUpdateFactory.newLatLng(new LatLng(Double.parseDouble(latituda), Double.parseDouble(longituda))));
				mapa.setOnInfoWindowClickListener(new GoogleMap.OnInfoWindowClickListener() {
					
					@Override
					public void onInfoWindowClick(Marker marker) {
						Intent intent = new Intent(DobavljaciActivity.this, PregledDobavljacaActivity.class);
						String naziv = marker.getTitle();
						for(Dobavljac d : dobavljaci) {
							if (d.getNaziv().equals(naziv)) {
								intent.putExtra(Constants.ID_DOBAVLJACA, d.getId());
								startActivity(intent);
								break;
							}
						}
					}
				});
			}
			
		}
	}
	
	private void prikaziNaMapi() {
		if (mapa != null) {
			mapa.clear();
			for(Dobavljac dobavljac : dobavljaci) {
				MarkerOptions opt = new MarkerOptions().position(new LatLng(dobavljac.getLatituda(), dobavljac.getLongituda()))
													.title(dobavljac.getNaziv())
													.snippet(dobavljac.getVrsta());
				mapa.addMarker(opt);
			}
		}
	}
	
	class PretraziDobavljaceTask extends AsyncTask<String, Void, Collection<Dobavljac>> {

		private Exception exception;
		@Override
		protected Collection<Dobavljac> doInBackground(String... arg) {
			Collection<Dobavljac> dobavljaci = null;
			try {
				dobavljaci = new ServiceProxy().pretraziDobavljace(arg[0], arg[1], arg[2], arg[3], arg[4]);
			} catch (IOException e) {
				exception = e;
			}
			return dobavljaci;
		}

		@Override
		protected void onPreExecute() {
			progressBar.setVisibility(View.VISIBLE);
		}
		
		@Override
		protected void onPostExecute(Collection<Dobavljac> result) {
			if (result != null) {
				dobavljaci = result.toArray(new Dobavljac[result.size()]);
				ListView listaDobavljaca = (ListView)findViewById(R.id.listaDobavljaca);
				listaDobavljaca.setAdapter(new DobavljaciArrayAdapter(DobavljaciActivity.this, dobavljaci));
				prikaziNaMapi();
			}
			progressBar.setVisibility(View.INVISIBLE);
			if (exception != null) {
				Toast.makeText(DobavljaciActivity.this, "Greška prilikom pretrage. Proverite konekciju", Toast.LENGTH_LONG).show();
			}
		}
	}
	
	class DobavljaciArrayAdapter extends ArrayAdapter<Dobavljac> {

		public DobavljaciArrayAdapter(Context context, Dobavljac[] dobavljaci) {
			super(context, R.layout.row_dobavljac, R.id.naziv, dobavljaci);
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			View row = convertView;
			if (row == null) {
				LayoutInflater inflater = getLayoutInflater();
				row = inflater.inflate(R.layout.row_dobavljac, parent, false);
			}
			Dobavljac dobavljac = dobavljaci[position];
			TextView naziv = (TextView)row.findViewById(R.id.nazivDobavljaca);
			naziv.setText(dobavljac.getNaziv());
			TextView vrsta = (TextView)row.findViewById(R.id.vrstaDobavljaca);
			vrsta.setText(dobavljac.getVrsta());
			TextView opis = (TextView)row.findViewById(R.id.opisDobavljaca);
			opis.setText(dobavljac.getOpis());
			TextView udaljenost = (TextView)row.findViewById(R.id.udaljenost);
			udaljenost.setText("Udaljenost " + dobavljac.getUdaljenost() + " m");
			return row;
		}
		
		
		
	}



	

}
