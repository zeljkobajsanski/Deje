package rs.zeks.deje.activities;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.R.id;
import rs.zeks.deje.model.GrupaArtikla;
import rs.zeks.deje.services.ServiceProxy;

import android.os.AsyncTask;
import android.os.Bundle;
import android.app.ListActivity;
import android.content.Intent;
import android.view.Menu;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.Toast;

public class GrupeArtikalaActivity extends ListActivity {

	private ProgressBar progressBar;
	private List<GrupaArtikla> grupeArtikala = new ArrayList<GrupaArtikla>();
	private String latituda;
	private String longituda;
	private String udaljenost;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_grupe_artikala);
		latituda = getIntent().getStringExtra(Constants.LATITUDA);
		longituda = getIntent().getStringExtra(Constants.LONGITUDA);
		udaljenost = getIntent().getStringExtra(Constants.UDALJENOST);
		progressBar = (ProgressBar)findViewById(id.progressBar1);
		new UcitajGrupeArtikalaTask().execute(latituda, longituda, udaljenost);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_grupe_artikala, menu);
		return true;
	}
	
	@Override
	protected void onListItemClick(ListView l, View v, int position, long id) {
		GrupaArtikla izabranaGrupa = grupeArtikala.get(position);
		Intent intent = new Intent(this, ArtikliActivity.class);
		intent.putExtra(Constants.LATITUDA, latituda);
		intent.putExtra(Constants.LONGITUDA, longituda);
		intent.putExtra(Constants.UDALJENOST, udaljenost);
		intent.putExtra(Constants.ID_GRUPE_ARTIKLA, String.valueOf(izabranaGrupa.getId()));
		startActivity(intent);
	}

	class UcitajGrupeArtikalaTask extends AsyncTask<String, Void, List<GrupaArtikla>> {

		private Exception exception;
		@Override
		protected List<GrupaArtikla> doInBackground(String... arg0) {
			List<GrupaArtikla> grupeArtikala = null;
			try {
				grupeArtikala = new ServiceProxy().pretraziGrupeArtikala(arg0[0], arg0[1], arg0[2]);
			} catch (IOException e) {
				this.exception = e;
			}
			return grupeArtikala;
		}

		@Override
		protected void onPostExecute(List<GrupaArtikla> result) {
			if (result != null) {
				GrupeArtikalaActivity.this.grupeArtikala = result;
				if (result.size() == 0) {
					Toast.makeText(GrupeArtikalaActivity.this, R.string.nema_rezultata, Toast.LENGTH_SHORT).show();
					GrupeArtikalaActivity.this.progressBar.setVisibility(View.INVISIBLE);
				}
				ArrayAdapter<GrupaArtikla> adapter = new ArrayAdapter<GrupaArtikla>(GrupeArtikalaActivity.this, android.R.layout.simple_list_item_1, grupeArtikala);
				GrupeArtikalaActivity.this.setListAdapter(adapter);
			}
			GrupeArtikalaActivity.this.progressBar.setVisibility(View.INVISIBLE);
			if (exception != null) {
				Toast.makeText(GrupeArtikalaActivity.this, "Greška prilikom pretrage. Proverite konekciju", Toast.LENGTH_LONG).show();
			}
		}

		@Override
		protected void onPreExecute() {
			GrupeArtikalaActivity.this.progressBar.setVisibility(View.VISIBLE);
		}
	}
}
